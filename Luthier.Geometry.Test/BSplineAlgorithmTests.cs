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

        [TestMethod]
        public void Find_Knot_Span()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };

            double[] t = { 0, 1, 1.5, 3, 3.2, 6, 6.5, 10, 11 };
            int[] expected = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            for(int i = 0; i < expected.Length; i++)
            {
                int knotIX = Nurbs.Algorithm.Find_Knot_Span(degree, knot, t[i]);
                Assert.AreEqual(expected[i], knotIX);
            }
 
        }


        private void TestNurbsBasisFunction(double expected, int i, int degree, double[] knot, double t)
        {
            //Assert.AreEqual(0, Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(0, degree, ref knot, t));
            Assert.AreEqual(0, Nurbs.Algorithm.deboor_value(0, degree, knot, t));
        }


        [TestMethod]
        public void BasisFunction_Evaluate_Deboor_Degree_Zero()
        {
            int degree = 0;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N0(t)==0 for all t since [k0, k1) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                TestNurbsBasisFunction(expected, 0, degree, knot, t);
            }

            //N1(t)==0 for all t since [k1, k2) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                TestNurbsBasisFunction(expected, 1, degree, knot, t);
            }

            //N2(t)==1 for t in [0, 0.3), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 1 : 0;
                TestNurbsBasisFunction(expected, 2, degree, knot, t);
            }

            //N3(t)==1 for t in [0.3, 0.5), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.3 <= t && t < 0.5) ? 1 : 0;
                TestNurbsBasisFunction(expected, 3, degree, knot, t);
            }

            //N4(t)==0 for all t since [k4, k5) = [0.5, 0.5) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                TestNurbsBasisFunction(expected, 4, degree, knot, t);
            }

            //N5(t)==1 for  t in [k5, k6) = [0.5, 0.6), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 1 : 0;
                TestNurbsBasisFunction(expected, 5, degree, knot, t);
            }

            //N6(t)==1 for  t in [k6, k7) = [0.6, 1.0), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.6 <= t && t < 1.0) ? 1 : 0;
                TestNurbsBasisFunction(expected, 6, degree, knot, t);
            }

            //N7(t)==0  for all t since [k7, k8) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                TestNurbsBasisFunction(expected, 7, degree, knot, t);
            }

            //N8(t)==0  for all t since [k8, k9) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                TestNurbsBasisFunction(expected, 8, degree, knot, t);
            }

        }


        [TestMethod]
        public void BasisFunction_Evaluate_Deboor_Degree_One()
        {
            int degree = 1;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N0(t)==0 for all t since [k0, k2) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                TestNurbsBasisFunction(expected, 0, degree, knot, t);
            }

            //N1(t) == 0 for t in [k1, k2) = [0, 0), and t == 1 - t/0.3 for t in [k2, k3) = [0, 0.3)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 1 - t / 0.3 : 0;
                TestNurbsBasisFunction(expected, 1, degree, knot, t);
            }

           
        }

        [TestMethod]
        public void BasisFunction_Evaluate_Deboor()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };

            double t = 3.2;
            
            double n0 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(0, degree, ref knot, t);
            double n1 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(1, degree, ref knot, t);
            double n2 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(2, degree, ref knot, t);

            Assert.AreEqual(0.52266666666666661, n0);
            Assert.AreEqual(0.47542857142857148, n1);
            Assert.AreEqual(0.001904761904761908, n2);

        }

        [Ignore("To be implemented")]
        [TestMethod]
        public void BasisFunction_EvaluateFirstDerivative_Deboor()
        {
            int degree = 2;
            double[] knot = new double[] { 1, 3, 6, 10 };

            double t = 3.2;
            int knotIX = 1;

            double n0 = Nurbs.Algorithm.BasisFunction_EvaluateFirstDerivative_Deboor(0, degree, knotIX, ref knot, t);
            double n1 = Nurbs.Algorithm.BasisFunction_EvaluateFirstDerivative_Deboor(1, degree, knotIX, ref knot, t);
            double n2 = Nurbs.Algorithm.BasisFunction_EvaluateFirstDerivative_Deboor(2, degree, knotIX, ref knot, t);

            Assert.AreEqual(0.52266666666666661, n0);
            Assert.AreEqual(0.47542857142857148, n1);
            Assert.AreEqual(0.001904761904761908, n2);

        }

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
        public void BSplineBasisFunction()
        {
            int degree = 2;
            var knot = new double[] { 0, 1, 3, 6, 10, 15 };
            var cvx = new double[] { 0, 1, 1 };
            var cvy = new double[] { 0, 0, 1 };
            var cvblock = new double[] { 0, 0, 1, 0, 1, 1 };

            double t = 3.2;
            //deboor_value coded to use "long" knot vector
            double[,] N0t = new double[3, knot.Length * 2];
            for(int i=0; i<knot.Length - 1; i++)
            {
                for(int j=0; j<2; j++)
                {
                    t = (1-(double)j/2)*knot[i] + (double)j/2*knot[i + 1];
                    N0t[0, i] = Nurbs.Algorithm.deboor_value(0, degree, knot, t);
                    N0t[1, i] = Nurbs.Algorithm.deboor_value(1, degree, knot, t);
                    N0t[2, i] = Nurbs.Algorithm.deboor_value(2, degree, knot, t);
                }
            }
            var n0 = Nurbs.Algorithm.deboor_value(0, degree, knot, t);
            var n1 = Nurbs.Algorithm.deboor_value(1, degree, knot, t);
            var n2 = Nurbs.Algorithm.deboor_value(2, degree, knot, t);

             var ct1FromBasis = cvblock[0] * n0 + cvblock[2] * n1 + cvblock[4] * n2;

            knot = new double[] { 1, 3, 6, 10 };
            var cv0 = new double[] { 1, 0, 0 };
            var ct0 = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 1, ref knot, ref cv0, 0, 1, t);
            var ct01 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(0, degree,ref knot, t);

            var cv1 = new double[] { 0, 1, 0 };
            var ct1 = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 1, ref knot, ref cv1, 0, 1, t);
            var ct11 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(1, degree, ref knot, t);

            var cv2 = new double[] { 0, 0, 1 };
            var ct2 = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 1, ref knot, ref cv2, 0, 1, t);
            var ct21 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(2, degree, ref knot, t);

            int iterations = 1000000;
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            for(int i=0; i<iterations; i++)
            {
                n0 = Nurbs.Algorithm.deboor_value(0, degree, knot, t);
            }
            long t1 = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                cv0 = new double[] { 1, 0, 0 };
                ct0 = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 1, ref knot, ref cv0, 0, 1, t);
            }
            long t2 = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                ct01 = Nurbs.Algorithm.BasisFunction_Evaluate_Deboor(0, degree,  ref knot, t);
            }
            long t3 = sw.ElapsedMilliseconds;


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

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                result[i] = Nurbs.Algorithm.CurveSpan_Evaluate_Deboor(degree, 1, ref knot, ref cvblock, 0, 2, 1.2);
            }
            timings.Add($"Evaluate_CurveSpan_Deboor: {iterations} calculations in {sw.ElapsedMilliseconds}ms");


        

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
            var result3 = curve.NearestSquaredDistanceBinaryTreeExact(cloud, 256);

            double s2 = result2.Distances.Sum();
            double s3 = result3.Distances.Sum();

            var lowerBound = new DenseVector(new double[] { -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50 });
            var upperBound = new DenseVector(new double[] { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 });
            var initialGuess = new DenseVector(curve.cvDataBlock);

            var objFunc = new NurbsCurveSquaredDistanceObjFunc(curve, cloud, EndConstraint.FixedPositionFixedTangent);
            var objvalue = ObjectiveFunction.Value(objFunc.Value);
            var fdgof = new ForwardDifferenceGradientObjectiveFunction(objvalue, lowerBound, upperBound);

            var sw = new Stopwatch();
            sw.Restart();

            var cgresult = ConjugateGradientMinimizer.Minimum(fdgof, initialGuess, gradientTolerance: 1e-2, maxIterations: 10000);

            var t1 = sw.ElapsedMilliseconds;
            //var solver = new BfgsBMinimizer(1e-5, 1e-5, 1e-5, maximumIterations: 100);
            //var optresult = solver.FindMinimum(fdgof, lowerBound, upperBound, initialGuess);

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

                tarray[i] = solver.NearestSquaredDistanceNewtonRaphson(2.2, 2.3);// curve.Domain.Min, curve.Domain.Max);
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
    }
}
