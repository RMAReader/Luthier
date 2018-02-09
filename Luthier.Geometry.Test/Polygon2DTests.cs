using System;
using System.Collections.Generic;
using Luthier.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Luthier.Test.Geometry
{

 
    [TestClass]
    public class Polygon2DTests
    {

        private Dictionary<string, Point2D> QuadrantTranslations(double minX, double minY, double maxX, double maxY)
        {
            return new Dictionary<string, Point2D>
            {
                { "To Quadrant I", new Point2D(1 - minX, 1 - minY)},
                { "To Quadrant II", new Point2D(minX - maxX - 2, 0)},
                { "To Quadrant III", new Point2D(0, minY - maxY - 2)},
                { "To Quadrant IV", new Point2D(2 + maxX - minX, 0)},
                { "To Centre", new Point2D(0.5 * (minX - maxX - 2), -0.5 * (minY - maxY - 2))},
            };
        }

        private Polygon2D Polygon_AntiClockwiseConvex()
        {
            return new Polygon2D(
                new List<Point2D>
                {
                    new Point2D(0,0),
                    new Point2D(0,10),
                    new Point2D(10,0),
                });
        }
        private Polygon2D Polygon_AntiClockwiseConcave()
        {
            return new Polygon2D(
                new List<Point2D>
                {
                   new Point2D(0,0),
                    new Point2D(0,10),
                    new Point2D(1,1),
                    new Point2D(10,0),
                });
        }


        [TestMethod]
        public void SignedArea_AntiClockWiseConvex()
        {
            var polygon = Polygon_AntiClockwiseConvex();

            foreach (var translation in QuadrantTranslations(polygon.MinX, polygon.MinY, polygon.MaxX, polygon.MaxY).Values)
            {
                polygon.Translate(translation);
                Assert.AreEqual(50, polygon.SignedArea());
            }
        }


        [TestMethod]
        public void SignedArea_ClockWiseConvex()
        {
            var polygon = Polygon_AntiClockwiseConvex();
            polygon.Reverse();

            foreach (var translation in QuadrantTranslations(polygon.MinX, polygon.MinY, polygon.MaxX, polygon.MaxY).Values)
            {
                polygon.Translate(translation);
                Assert.AreEqual(-50, polygon.SignedArea());
            }
        }


        [TestMethod]
        public void SignedArea_AntiClockWiseConcave()
        {
            var polygon = Polygon_AntiClockwiseConcave();

            foreach (var translation in QuadrantTranslations(polygon.MinX, polygon.MinY, polygon.MaxX, polygon.MaxY).Values)
            {
                polygon.Translate(translation);
                Assert.AreEqual(10, polygon.SignedArea());
            }
        }


        [TestMethod]
        public void SignedArea_ClockWiseConcave()
        {
            var polygon = Polygon_AntiClockwiseConcave();
            polygon.Reverse();

            foreach (var translation in QuadrantTranslations(polygon.MinX, polygon.MinY, polygon.MaxX, polygon.MaxY).Values)
            {
                polygon.Translate(translation);
                Assert.AreEqual(-10, polygon.SignedArea());
            }
        }
    }
}
