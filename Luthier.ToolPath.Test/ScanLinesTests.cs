using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Luthier.Geometry;

namespace Luthier.Test
{
    [TestClass]
    public class ScanLinesTests
    {
        [TestMethod]
        public void Test_1()
        {
            int n = 1500;
            double r = 100f;
            var boundary1 = new Polygon2D();
            for (int i = 0; i < n; i++) boundary1.Add(new Point2D(r * (double)Math.Sin((double)i / n * Math.PI * 2), r * (double)Math.Cos((double)i / n * Math.PI * 2)));
            var boundary2 = new Polygon2D();
            for (int i = 0; i < n; i++) boundary2.Add(new Point2D(r *0.5f* (double)Math.Sin((double)i / n * Math.PI * 2), r *0.5f* (double)Math.Cos((double)i / n * Math.PI * 2)));
            List<Polygon2D> border = new List<Polygon2D> { boundary1, boundary2 };

            double step = 1;
            var path = new ScanLinePath2D(border, step);

        }


        [TestMethod]
        public void toolpath_build_scanlines()
        {

            double tolerance = 0.0001f;
            List<Polygon2D> border = new List<Polygon2D>();

            border.Add(new Polygon2D
                (new List<Point2D>
                {
                    new Point2D(0, 0),
                    new Point2D(0, 10),
                    new Point2D(10, 20),
                    new Point2D(20, 10),
                    new Point2D(20, 0),
                    new Point2D(5, 5),
                }
            ));
            border.Add(new Polygon2D
                (new List<Point2D>
                {
                    new Point2D(10, 10),
                    new Point2D(10, 15),
                    new Point2D(15, 10)
                }
            ));

            double step = 3;

            var path = new ScanLinePath2D(border, step);

            var expectedPath = new List<List<Point2D>>
            {
                new List<Point2D>{
                    new Point2D ( 0, 0),
                    new Point2D ( 20, 0),
                    new Point2D ( 21, 5),
                    new Point2D ( 1, 5),
                    new Point2D (2, 10),
                    new Point2D ( 8, 10),
                    new Point2D ( 8, 15),
                    new Point2D ( 3, 15),
                    new Point2D ( 4, 20),
                    new Point2D (  24, 20),
                    new Point2D (  25, 25),
                    new Point2D (5, 25),
                    new Point2D (6, 30),
                    new Point2D ( 26, 30),
                },
                new List<Point2D>{
                    new Point2D ( 9, 10),
                    new Point2D ( 22, 10),
                    new Point2D ( 23, 15),
                    new Point2D ( 9, 15),
                },
                new List<Point2D>{
                    new Point2D ( 40, 25),
                    new Point2D ( 50, 25),
                    new Point2D ( 59, 30),
                    new Point2D ( 46, 30),
                }
            };

            Assert.AreEqual(expectedPath.Count, path.Count);

        }
    }
}
