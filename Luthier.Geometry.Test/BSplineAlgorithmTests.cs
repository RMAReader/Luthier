using Luthier.Geometry.BSpline;
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

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class BSplineAlgorithmTests
    {


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
        public void evaluation_Test1()
        {
            int degree = 2;
            var knot = new double[] { 1000, 1, 2, 3, 4 };
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
               Knot.CreateUniformOpen(2, 6)
            );

            var _px = BSpline.Algorithm.Evaluate_CurveSpan_Deboor(degree, 2, ref knot, ref cvblock, 0, 2, 2.2);
            var _py = BSpline.Algorithm.Evaluate_CurveSpan_Deboor(degree, 2, ref knot, ref cvblock, 1, 2, 2.2);


            var _p1 = curve.Evaluate(2.2);


            int n = 10000000;
            double[] px = new double[n];
            double[] py = new double[n];
             Point2D[] p = new Point2D[n];

            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

     
            for (int i = 0; i < n; i++)
            {
                double t = 2 + (double)i / n;
                px[i] = BSpline.Algorithm.Evaluate_CurveSpan_Deboor(degree, 2, ref knot, ref cvblock, 0, 2, t);
                py[i] = BSpline.Algorithm.Evaluate_CurveSpan_Deboor(degree, 2, ref knot, ref cvblock, 1, 2, t);
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
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 5);
            curve.SetCV(0, new double[] { 0, 0 });
            curve.SetCV(1, new double[] { 1, 1 });
            curve.SetCV(2, new double[] { 2, 0 });
            curve.SetCV(3, new double[] { 3, -1 });
            curve.SetCV(4, new double[] { 4, 0 });

            var cloudPoints = new List<double[]>();
            int numberOfPoints = 1000;
            Random rnd = new Random();
            for (int i = 0; i < numberOfPoints; i++)
            {
                cloudPoints.Add(new double[] 
                {
                    i / numberOfPoints +  rnd.NextDouble() * 0.1,
                    rnd.NextDouble() * 0.1
                });
            }
            var cloud = new PointCloud(cloudPoints);

            var nearestPoints = new NurbsCurveNearestPoint();
            var result = nearestPoints.Calculate(curve, cloud);

            var objFunc = new NurbsCurveNearstPointObjFunc(curve, cloud);

            var obj = ObjectiveFunction.Gradient(objFunc.Value, objFunc.Gradient);
            var solver = new BfgsBMinimizer(1e-10, 1e-10, 1e-10, maximumIterations: 1000);
            var lowerBound = new DenseVector(new[] { -5.0, -5.0, -5.0, -5.0, -5.0, -5.0, -5.0, -5.0, -5.0, -5.0 });
            var upperBound = new DenseVector(new[] { 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0 });
            var initialGuess = new DenseVector(curve.cvDataBlock);

            var optresult = solver.FindMinimum(obj, lowerBound, upperBound, initialGuess);
        }
    }
}
