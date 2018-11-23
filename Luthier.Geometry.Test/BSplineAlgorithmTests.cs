using Luthier.Geometry.Nurbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Luthier.Geometry.Optimization;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using System.Diagnostics;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class BSplineAlgorithmTests
    {

      

   

        [Ignore("To be implemented")]
        [TestMethod]
        public void BasisFunction_EvaluateDerivative_Deboor()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };

            double t = 3.2;
            int knotIX = 1;

            double n0 = Nurbs.Algorithm.BasisFunction_EvaluateDerivative_Deboor(0, 0, degree, knotIX, ref knot, t);
            double d1n0 = Nurbs.Algorithm.BasisFunction_EvaluateDerivative_Deboor(1, 0, degree, knotIX, ref knot, t);
            double d2n0 = Nurbs.Algorithm.BasisFunction_EvaluateDerivative_Deboor(2, 0, degree, knotIX, ref knot, t);
           
        }


        [Ignore("Obsolete")]
        [TestMethod]
        public void CurveSpan_Evaluate_Deboor()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };
            double[] cvDataBlock = new double[] { 0, 0, 1, 0, 1.5, 1 };
            int cvStride = 2;

            double t = 3.2;
            int knotIX = 1;

            var cxt = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, knotIX, ref knot, ref cvDataBlock, 0, cvStride, t);
            var cyt = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, knotIX, ref knot, ref cvDataBlock, 1, cvStride, t);

            Assert.AreEqual(0.47828571428571437, cxt);
            Assert.AreEqual(0.001904761904761908, cyt);

        }

        [Ignore("Obsolete")]
        [TestMethod]
        public void CurveSpan_EvaluateFirstDerivative_Deboor()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };
            double[] cvDataBlock = new double[] { 0, 0, 1, 0, 1.5, 1 };
            int cvStride = 2;

            double t = 3.2;
            int knotIX = 1;

            var cxt = Nurbs.Algorithm.CurveSpan_EvaluateFirstDerivative_Deboor(degree, knotIX, ref knot, ref cvDataBlock, 0, cvStride, t);
            var cyt = Nurbs.Algorithm.CurveSpan_EvaluateFirstDerivative_Deboor(degree, knotIX, ref knot, ref cvDataBlock, 1, cvStride, t);

            Assert.AreEqual(0.38285714285714284, cxt);
            Assert.AreEqual(0.019047619047619063, cyt);
        }

        [Ignore("Obsolete")]
        [TestMethod]
        public void CurveSpan_EvaluateDerivatives_Deboor()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };
            double[] cvDataBlock = new double[] { 0, 0, 1, 0, 1.5, 1 };
            int cvStride = 2;

            double t = 3.2;
            int knotIX = 1;

            var cxtder0 = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(0, degree, knotIX, ref knot, ref cvDataBlock, 0, cvStride, t);
            var cxtder1 = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(1, degree, knotIX, ref knot, ref cvDataBlock, 0, cvStride, t);
            var cxtder2 = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(2, degree, knotIX, ref knot, ref cvDataBlock, 0, cvStride, t);

            Assert.AreEqual(0.47828571428571437, cxtder0);
            Assert.AreEqual(0.38285714285714284, cxtder1);
            Assert.AreEqual(-0.12857142857142859, cxtder2);

        }


        [TestMethod]
        public void Curve_InsertPoints()
        {
            var curve = new BSplineCurve(
                new List<Point2D>
                {
                    new Point2D(0,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );

            var points = curve.ToLines(10);

            curve.InsertKnots(new List<double> { 2, 2, 2.2, 2.8 });

            var p2 = curve.ToLines(10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(points[i].x, p2[i].x, 0.000001);
                Assert.AreEqual(points[i].y, p2[i].y, 0.000001);
            }

        }



        [TestMethod]
        public void Curve_CloseFront()
        {
            var curve = new BSplineCurve(
                new List<Point2D>
                {
                    new Point2D(0,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );

            var points = curve.ToLines(10);

            curve.CloseFront();

            var p2 = curve.ToLines(10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(points[i].x, p2[i].x, 0.000001);
                Assert.AreEqual(points[i].y, p2[i].y, 0.000001);
            }
            Assert.AreEqual(curve.Evaluate(2).x, p2[0].x, 0.000001);
            Assert.AreEqual(curve.Evaluate(2).y, p2[0].y, 0.000001);
        }


        [TestMethod]
        public void Curve_CloseBack()
        {
            var curve = new BSplineCurve(
                new List<Point2D>
                {
                    new Point2D(0,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );

            var points = curve.ToLines(10);

            curve.CloseBack();

            var p2 = curve.ToLines(10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(points[i].x, p2[i].x, 0.000001);
                Assert.AreEqual(points[i].y, p2[i].y, 0.000001);
            }
        }


        [TestMethod]
        public void Curve_CurveIntersection()
        {
            var c1 = new BSplineCurve(
                new List<Point2D>
                {
                    new Point2D(-1,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );
            var c2 = new BSplineCurve(
                new List<Point2D>
                {
                    new Point2D(0.5f,-1),
                    new Point2D(0.5f,0),
                    new Point2D(0.5f,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );
            var intersections = Intersection.GetIntersection(c1, c2, new Point2D(0, 0), 0.00001);
        }

        [TestMethod]
        public void Curve_Rhino()
        {
            //var curve = new NurbsCurve(2, 3);

            //curve.Points[0] = new ControlPoint(0, 0, 0);
            //curve.Points[1] = new ControlPoint(0, 1, 0);
            //curve.Points[2] = new ControlPoint(1, 1, 0);

            //for (int i = 0; i < curve.Knots.Count; i++)
            //{
            //    curve.Knots[i] = i;
            //}

            //var p1 = curve.PointAt(2);
            //var p2 = curve.PointAt(2.2);
            //var p3 = curve.PointAt(2.8);
            //var p4 = curve.PointAt(3);
        }

        [TestMethod]
        public void NurbsEvaluationSingleSpan_UnevenKnot()
        {
            int degree = 2;
            var knot = new double[] { 0, 1, 3, 6, 10 };
            var cvx = new double[] { 0, 1, 1 };
            var cvy = new double[] { 0, 0, 1 };
            var cvblock = new double[] { 0, 0, 1, 0, 1, 1 };

            var curve = new BSplineCurve(
               new List<Point2D>
               {
                    new Point2D(0,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
               },
               new Knot() {p = degree, data = new List<double>(knot) }
               //Knot.CreateUniformOpen(2, 6)
            );

            var _px = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 2, ref knot, ref cvblock, 0, 2, 3.2);
            var _py = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 2, ref knot, ref cvblock, 1, 2, 3.2);


            var _p1 = curve.Evaluate(3.2);


            int n = 10000000;
            double[] px = new double[n];
            double[] py = new double[n];
             Point2D[] p = new Point2D[n];

            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

     
            for (int i = 0; i < n; i++)
            {
                double t = 2 + (double)i / n;
                px[i] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 2, ref knot, ref cvblock, 0, 2, t);
                py[i] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 2, ref knot, ref cvblock, 1, 2, t);
            }

            var t1 = sw.ElapsedMilliseconds;

            //sw.Restart();
            //double[] tarray = new double[n];
            //for (int i = 0; i < n; i++)
            //{
            //    double t = 2 + (double)i / n;
            //    px[i] = BSpline.Algorithm.Evaluate_Span_DeboorInlined(degree, 2, ref knot, ref cvblock, 0, 2, t);
            //    py[i] = BSpline.Algorithm.Evaluate_Span_DeboorInlined(degree, 2, ref knot, ref cvblock, 1, 2, t);
            //}

            var t2 = sw.ElapsedMilliseconds;
            sw.Restart();

            for (int i = 0; i < n; i++)
            {
                double t = 2 + (double)i / n;
                p[i] = curve.Evaluate(t);
            }

            var t3 = sw.ElapsedMilliseconds;

           
            File.WriteAllLines(@"C:\Users\Richard\Documents\Development\Luthier\TestResult.txt", new string[] { string.Format("t1 = {0}, t2 = {1}, t3 = {2}", t1, t2, t3) });
        }



        [TestMethod]
        public void DerivativeEvaluation_Test1()
        {
            int degree = 2;
            var knot = new double[] { 0, 1, 3, 6 };
            var cvblock = new double[] { 0, 0, 1, 0, 1.5, 1 };

            double h = 1E-6;

            double px = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree,  1, ref knot, ref cvblock, 0, 2, 1.2);
            double pxh = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree,  1, ref knot, ref cvblock, 0, 2, 1.2 + h);
            double forwardDiffDeriv = (pxh - px) / h;

            double derivpx = Nurbs.Algorithm.CurveSpan_EvaluateFirstDerivative_Deboor(degree, 1, ref knot, ref cvblock, 0, 2, 1.2);

            double deriv0 = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(0, degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            double deriv1 = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(1, degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            double deriv2 = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(2, degree, 1, ref knot, ref cvblock, 0, 2, 1.2);

            //timings
            int iterations = 1000000;
            var sw = new Stopwatch();
            sw.Restart();
            var timings = new List<string>();

            var result = new double[iterations];
            for(int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"Evaluate_CurveSpan_Deboor: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_EvaluateFirstDerivative_Deboor(degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"EvaluateDerivative_CurveSpan_Deboor: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(0, degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"EvaluateHigherDerivative(0): {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(1, degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"EvaluateHigherDerivative(1): {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_EvaluateDerivative_Deboor(2, degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"EvaluateHigherDerivative(2): {iterations} calculations in {sw.ElapsedMilliseconds}ms");


            knot = new double[] { 0, 0, 1, 3, 6, 6 };
            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor2(degree, 2, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"Evaluate_CurveSpan_Deboor2: {iterations} calculations in {sw.ElapsedMilliseconds}ms");


        

        }


        [TestMethod]
        public void BenchmarkTest_BasisFunction_Evaluation()
        {

            int degree = 2;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };
            int iterations = 1000000;
            var sw = new Stopwatch();
            sw.Restart();
            var timings = new List<string>();
            double[] result = new double[iterations * parameters.Length];

            sw.Restart();
            int k = 0;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    result[k++] = Nurbs.Algorithm.BasisFunction_Evaluate(4, degree, knot, parameters[j]);
                }
            }
            timings.Add($"BasisFunction_Evaluate_DegreeTwo: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            k = 0;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    result[k++] = Nurbs.Algorithm.deboor_value(4, degree, knot, parameters[j]);
                }
            }
            timings.Add($"deboor_value: {iterations} calculations in {sw.ElapsedMilliseconds}ms");


            sw.Restart();
            k = 0;
            double[] values = new double[3];
            int[] indices = new int[3];
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    Nurbs.Algorithm.BasisFunction_EvaluateAllNonZero_DegreeTwo(knot, parameters[j], ref values, ref indices);
                    result[k++] = values[0];
                }
            }
            timings.Add($"BasisFunction_EvaluateAllNonZero_DegreeTwo: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

        }

        [TestMethod]
        public void BenchmarkTest_CurveSpan_Evaluation()
        {

            int degree = 2;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };
            double[] cvDataBlock = new double[] { 0.5, 1, 3, 6, 10, 15, 21 };

            double[] parameters = new double[] { 0.1 };
            int iterations = 10000000;
            var sw = new Stopwatch();
            sw.Restart();
            var timings = new List<string>();
            double[] result = new double[iterations * parameters.Length];

            sw.Restart();
            int k = 0;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    result[k++] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 2, ref knot, ref cvDataBlock, 0, 2, parameters[j]);
                }
            }
            timings.Add($"CurveSpan_Evaluate_Deboor: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            k = 0;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    result[k++] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor2(degree, 2, ref knot, ref cvDataBlock, 0, 2, parameters[j]);
                }
            }
            timings.Add($"CurveSpan_Evaluate_Deboor2: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

        }


        [TestMethod]
        public void BenchmarkTest_NurbsCurve_Evaluation()
        {
           
            NurbsCurve curve = new NurbsCurve(3, false, 3, 3);
            curve.SetCV(0, new double[] { 0, 0, 2 });
            curve.SetCV(1, new double[] { 1, 0, 2 });
            curve.SetCV(2, new double[] { 1, 1, 0 });
            curve.knot = new double[] { -1, 0, 1, 2, 3, 4 };

            double[] parameters = new double[] { 0.5 * (curve.Domain.Min + curve.Domain.Max) };
            int iterations = 1000000;
            var sw = new Stopwatch();
            sw.Restart();
            var timings = new List<string>();

            sw.Restart();
            int k = 0;
            double[] result = new double[3];
            for (int i = 0; i < iterations; i++)
            {
                curve.Evaluate(parameters[0], result);
            }
            timings.Add($"curve.Evaluate: {iterations} calculations in {sw.ElapsedMilliseconds}ms");


            NurbsCurve curve1 = curve.DeepCopy();
            curve1.knot = new double[] { -1, 0, 1, 2, 3, 4 };
            sw.Restart();
            k = 0;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    //curve1.Evaluate(parameters[0], 0, result);
                }
            }
            timings.Add($"CurveSpan_Evaluate_Deboor2: {iterations} calculations in {sw.ElapsedMilliseconds}ms");


            NurbsCurve curve2 = curve1.DeepCopy();
            parameters = new double[iterations];
            for (int i = 0; i < iterations; i++)
            {
                parameters[i] = 0.5 * (curve.Domain.Min + curve.Domain.Max);
            }
            sw.Restart();
            curve2.SetEvaluationPoints(parameters);
           
            timings.Add($"SetEvaluationPoints: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            
            curve2.RecalculateAtEvaluationPoints();
            timings.Add($"RecalculateAtEvaluationPoints: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

            sw.Restart();

            curve2.RecalculateAtEvaluationPoints();
            timings.Add($"RecalculateAtEvaluationPoints: {iterations} calculations in {sw.ElapsedMilliseconds}ms");

        }


        [TestMethod]
        public void ArrayCopy_BufferCopy_Speed_Test()
        {
            int n = 200;
            double[] data = new double[n];
            for (int i = 0; i < n; i++) data[i] = n;


            var sw = new System.Diagnostics.Stopwatch();
            int r = 1000000;

            sw.Restart();

            sw.Restart();
            for (int i=0; i<r; i++)
            {
                data[0] = i;
                double[] copy = new double[n];
                Array.Copy(data, copy, n);
            }

            long t1 = sw.ElapsedMilliseconds;
            sw.Restart();
 

            for (int i = 0; i < r; i++)
            {
                data[0] = i;
                double[] copy = new double[n];
                for (int j = 0; j < n; j++) copy[j] = data[j];
            }

            long t3 = sw.ElapsedMilliseconds;
            sw.Restart();


            for (int i = 0; i < r; i++)
            {
                data[0] = i;
                double[] copy = new double[n];
                Buffer.BlockCopy(data, 0, copy, 0, n * sizeof(double));
            }

            long t2 = sw.ElapsedMilliseconds;

            File.WriteAllLines(@"C:\Users\Richard\Documents\Development\Luthier\TestResult.txt", new string[] { string.Format("t1 = {0}, t2 = {1}, t3 = {2}", t1, t2, t3) });

        }


        [TestMethod]
        public void NurbsCurveNearestPointTest()
        {
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 6);
            curve.SetCV(0, new double[] { 0, -0.81 });
            curve.SetCV(1, new double[] { 0, 0.81 });
            curve.SetCV(2, new double[] { 1.2, 2 });
            curve.SetCV(3, new double[] { 2.8, 2 });
            curve.SetCV(4, new double[] { 4, 0.81 });
            curve.SetCV(5, new double[] { 4, -0.81 });

            var cloudPoints = new List<double[]>();
            int numberOfPoints = 10000;
            Random rnd = new Random();
            for (int i = 0; i <= numberOfPoints; i++)
            {
                cloudPoints.Add(new double[] 
                {
                    2 - 2 * Math.Cos((double)i / numberOfPoints * Math.PI),
                    2 * Math.Sin((double)i / numberOfPoints * Math.PI)
                });
            }
            var cloud = new PointCloud(cloudPoints);

            var result1 = curve.NearestSquaredDistanceLinear(cloud, 16384);
            var result2 = curve.NearestSquaredDistanceBinaryTree(cloud, 16384);
            var result3 = curve.NearestSquaredDistanceBinaryTreeExact(cloud, 32);

            double s2 = result2.Distances.Sum();
            double s3 = result3.Distances.Sum();

       

        }

        [TestMethod]
        public void NurbsCurveTreeTest()
        {
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 6);
            curve.SetCV(0, new double[] { 0, -1 });
            curve.SetCV(1, new double[] { 0, 1 });
            curve.SetCV(2, new double[] { 1, 3 });
            curve.SetCV(3, new double[] { 3, 3 });
            curve.SetCV(4, new double[] { 4, 1 });
            curve.SetCV(5, new double[] { 4, -1 });

            var sw = new Stopwatch();
            sw.Restart();

            var tree = new NurbsCurveBinaryTree(curve, 100000);

            long t1 = sw.ElapsedMilliseconds;

        }

        
        [TestMethod]
        public void NurbsCurveNearestPoint()
        {
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 6);
            curve.SetCV(0, new double[] { 0, -1 });
            curve.SetCV(1, new double[] { 0, 1 });
            curve.SetCV(2, new double[] { 1, 3 });
            curve.SetCV(3, new double[] { 3, 3 });
            curve.SetCV(4, new double[] { 4, 1 });
            curve.SetCV(5, new double[] { 4, -1 });

            var sw = new Stopwatch();
            sw.Restart();

            double[] point = new double[] { 0, 3 };
            double[] tarray = new double[10000];
            for (int i = 0; i < tarray.Length; i++)
            {
                
                var solver = new NurbsCurveNearestPoint(curve, point);

                double[] c0 = new double[curve._dimension];
                double[] c1 = new double[curve._dimension];
                double[] c2 = new double[curve._dimension];

                tarray[i] = solver.NearestSquaredDistanceNewtonRaphson(2.2, 2.3, ref c0, ref c1, ref c2);// curve.Domain.Min, curve.Domain.Max);
            }
            long t1 = sw.ElapsedMilliseconds;

        }


        private double[] Enumerate(double min, double max, int count)
        {
            double[] result = new double[count];
            for (int i=0; i<count; i++)
            {
                double t = (1 - (double)i / count) * min + (double)i / count * max;
            }
            return result;
        }



        [TestMethod]
        public void NurbsCurveFitter()
        {
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 6);
            curve.SetCV(0, new double[] { 0, -2 });
            curve.SetCV(1, new double[] { 0, 2 });
            curve.SetCV(2, new double[] { 1, 2 });
            curve.SetCV(3, new double[] { 3, 2 });
            curve.SetCV(4, new double[] { 4, 2 });
            curve.SetCV(5, new double[] { 4, -2 });

            var cvDataBlock = new double[curve.cvDataBlock.Length];
            Array.Copy(curve.cvDataBlock, cvDataBlock, cvDataBlock.Length);

            var cloudPoints = new List<double[]>();
            int numberOfPoints = 5000;
            Random rnd = new Random();
            for (int i = 0; i <= numberOfPoints; i++)
            {
                cloudPoints.Add(new double[]
                {
                    2 - 2 * Math.Cos((double)i / numberOfPoints * Math.PI),
                    2 * Math.Sin((double)i / numberOfPoints * Math.PI)
                });
            }
            var cloud = new PointCloud(cloudPoints);

            try
            {
                Array.Copy(cvDataBlock, curve.cvDataBlock, cvDataBlock.Length);
                var fitter = new NurbsCurveFitterAccordNet();

                fitter.Fit(curve, cloud);

                
            }
            catch { }

            try
            {
                Array.Copy(cvDataBlock, curve.cvDataBlock, cvDataBlock.Length);
                var fitter2 = new NurbsCurveFitterQuadraticApproximation();

                fitter2.Fit(curve, cloud);
            }
            catch { }

            try
            {
                Array.Copy(cvDataBlock, curve.cvDataBlock, cvDataBlock.Length);
                var fitter3 = new NursCurveFitterNewton();

                fitter3.Fit(curve, cloud);
            }
            catch { }
        }
    }
}
