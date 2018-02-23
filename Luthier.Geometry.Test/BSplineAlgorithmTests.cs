using Luthier.Geometry.BSpline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class BSplineAlgorithmTests
    {


        [TestMethod]
        public void Curve_InsertPoints()
        {
            var curve = new Curve(
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
            var curve = new Curve(
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
        }


        [TestMethod]
        public void Curve_CloseBack()
        {
            var curve = new Curve(
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
            var c1 = new Curve(
                new List<Point2D>
                {
                    new Point2D(-1,0),
                    new Point2D(1,0),
                    new Point2D(1,1),
                },
                Knot.CreateUniformOpen(2, 6)
            );
            var c2 = new Curve(
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
    }
}
