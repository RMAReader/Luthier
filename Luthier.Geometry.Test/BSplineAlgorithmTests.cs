using Luthier.Geometry.BSpline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class BSplineAlgorithmTests
    {


        [TestMethod]
        public void Curve_InsertPoints()
        {
            var curve = new NurbsCurve(
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
            var curve = new NurbsCurve(
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
            var curve = new NurbsCurve(
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
            var c1 = new NurbsCurve(
                new List<Point2D>
                {
                    new Point2D(-1,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );
            var c2 = new NurbsCurve(
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

            var curve = new NurbsCurve(
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
        public void NurbsSurface_To_Opennurbs_Evaluation_Basic()
        {
            int cvCount0 = 3;
            int cvCount1 = 3;
            int order0 = 3;
            int order1 = 3;

            var s1 = new NurbsSurface(2, false, order0, order1, cvCount0, cvCount1);
            s1.SetCV(0, 0, new double[] { 0, 0 });
            s1.SetCV(1, 0, new double[] { 1, 0.5 });
            s1.SetCV(2, 0, new double[] { 2, 0 });
            s1.SetCV(0, 1, new double[] { -1, 5 });
            s1.SetCV(1, 1, new double[] { 1, 7 });
            s1.SetCV(2, 1, new double[] { 3, 5 });
            s1.SetCV(0, 2, new double[] { -2, 20 });
            s1.SetCV(1, 2, new double[] { 1, 15 });
            s1.SetCV(2, 2, new double[] { 5, 10 });

            for (int i = 0; i < s1.KnotCount(0); i++)
            {
                s1.SetKnot(0, i, i);
            }
            for (int i = 0; i < s1.KnotCount(1); i++)
            {
                s1.SetKnot(1, i, i);
            }

            opennurbs_CLI.NurbsSurface s2 = new opennurbs_CLI.NurbsSurface(2, 0, order0, order1, cvCount0, cvCount1);
            for(int i=0; i< 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    s2.SetCV(i, j, 3, s1.GetCV(i, j));
                }
            }
            for (int i = 0; i < s1.KnotCount(0); i++) s2.SetKnot(0, i, s1.GetKnot(0, i));
            for (int i = 0; i < s1.KnotCount(1); i++) s2.SetKnot(1, i, s1.GetKnot(1, i));


            var u = new double[] {  0.9,1.0, 1.1, 1.2, 1.5, 1.8, 1.9, 1.999999,2.1 };
            var v = new double[] {  0.9,1.0, 1.1, 1.2, 1.5, 1.8, 1.9, 1.999999,2.1 };
            for(int i=0; i<u.Length; i++)
            {
                for (int j = 0; j < v.Length; j++)
                {
                    var p1 = s1.Evaluate(u[i], v[j]);
                    var p2 = s2.Evaluate(u[i], v[j], 1);

                    Assert.AreEqual(p1[0], p2[0], 0.00001);
                    Assert.AreEqual(p1[1], p2[1], 0.00001);
                }
            }
            

        }


        [TestMethod]
        public void NurbsSurface_To_Opennurbs_Evaluation_RepeatedKnot()
        {
            int cvCount0 = 3;
            int cvCount1 = 3;
            int order0 = 3;
            int order1 = 3;

            var s1 = new NurbsSurface(2, false, order0, order1, cvCount0, cvCount1);
            s1.SetCV(0, 0, new double[] { 0, 0 });
            s1.SetCV(1, 0, new double[] { 1, 0.5 });
            s1.SetCV(2, 0, new double[] { 2, 0 });
            s1.SetCV(0, 1, new double[] { -1, 5 });
            s1.SetCV(1, 1, new double[] { 1, 7 });
            s1.SetCV(2, 1, new double[] { 3, 5 });
            s1.SetCV(0, 2, new double[] { -2, 20 });
            s1.SetCV(1, 2, new double[] { 1, 15 });
            s1.SetCV(2, 2, new double[] { 5, 10 });

            var knotU = new double[] { 0, 0, 1, 1 };
            for (int i = 0; i < s1.KnotCount(0); i++)
            {
                s1.SetKnot(0, i, knotU[i]);
            }
            var knotV = new double[] { 0, 0, 1, 1 };
            for (int i = 0; i < s1.KnotCount(1); i++)
            {
                s1.SetKnot(1, i, knotV[i]);
            }

            opennurbs_CLI.NurbsSurface s2 = new opennurbs_CLI.NurbsSurface(2, 0, order0, order1, cvCount0, cvCount1);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    s2.SetCV(i, j, 3, s1.GetCV(i, j));
                }
            }
            for (int i = 0; i < s1.KnotCount(0); i++) s2.SetKnot(0, i, s1.GetKnot(0, i));
            for (int i = 0; i < s1.KnotCount(1); i++) s2.SetKnot(1, i, s1.GetKnot(1, i));


            var u = new double[] { 0.9, 1.0, 1.1, 1.2, 1.5, 1.8, 1.9, 1.999999, 2.1 };
            var v = new double[] { 0.9, 1.0, 1.1, 1.2, 1.5, 1.8, 1.9, 1.999999, 2.1 };
            for (int i = 0; i < u.Length; i++)
            {
                for (int j = 0; j < v.Length; j++)
                {
                    var p1 = s1.Evaluate(u[i], v[j]);
                    var p2 = s2.Evaluate(u[i], v[j], 1);

                    Assert.AreEqual(p1[0], p2[0], 0.00001);
                    Assert.AreEqual(p1[1], p2[1], 0.00001);
                }
            }


        }
    }
}
