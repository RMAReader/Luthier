using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Luthier.Geometry;

namespace Luthier.Test.Geometry
{
    [TestClass]
    public class AlgorithmTests
    {

        [TestMethod]
        public void intersection_line_yTest1()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(10, 10);
            double y = 0;

            Assert.AreEqual(0, Algorithm.intersection_line_y(y, p1, p2));

        }

        [TestMethod]
        public void intersection_line_yTest2()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(10, 10);
            double y = 10;

            Assert.AreEqual(10, Algorithm.intersection_line_y(y, p1, p2));

        }

        [TestMethod]
        public void intersection_line_yTest3()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(10, 10);
            double y = 5;

            Assert.AreEqual(5, Algorithm.intersection_line_y(y, p1, p2));

        }

        [TestMethod]
        public void intersection_line_yTest4()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(10, 10);
            double y = -1;

            Assert.AreEqual(null, Algorithm.intersection_line_y(y, p1, p2));

        }

        [TestMethod]
        public void intersection_line_yTest5()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(10, 10);
            double y = 11;

            Assert.AreEqual(null, Algorithm.intersection_line_y(y, p1, p2));

        }

        [TestMethod]
        public void intersection_line_yTest6()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(10, 0);
            double y = 0;

            Assert.AreEqual(null, Algorithm.intersection_line_y(y, p1, p2));

        }

        [TestMethod]
        public void LineSegmentIntersectionTest1()
        {
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(0, 3);
            Point2D q1 = new Point2D(-1, 0);
            Point2D q2 = new Point2D(3, 4);

            var res = Intersection.GetIntersection(p1, p2, q1, q2);

            Assert.AreEqual(0.666666,res.Parameter1,0.000001);
            Assert.AreEqual(0.75, res.Parameter2, 0.000001);
            Assert.AreEqual(0, res.Point.x, 0.000001);
            Assert.AreEqual(1, res.Point.y, 0.000001);
        }

        [TestMethod]
        public void distance_to_nearest_contactTest1()
        {
            Point2D p1 = new Point2D(-1, 10);
            Point2D p2 = new Point2D(1, 10);
            Point2D direction = new Point2D(0, 1);
            Point2D centre = new Point2D(0, 5);
            double radius = 1.5;

            Assert.AreEqual(3.5, Algorithm.distance_to_nearest_line_contact(radius, centre, direction, p1, p2));
        }

        [TestMethod]
        public void distance_to_nearest_contactTest2()
        {
            Point2D p1 = new Point2D(0, 10);
            Point2D p2 = new Point2D(10, 0);
            Point2D direction = new Point2D(0, 1);
            Point2D centre = new Point2D(0, 5);
            double radius = 1.0;

            Assert.AreEqual(5 - Math.Sqrt(2), (double)Algorithm.distance_to_nearest_line_contact(radius, centre, direction, p1, p2), 0.000001);
        }

        [TestMethod]
        public void distance_to_nearest_contactTest3()
        {
            Point2D p1 = new Point2D(10, 0);
            Point2D p2 = new Point2D(0, 10);
            Point2D direction = new Point2D(0, 1);
            Point2D centre = new Point2D(0, 5);
            double radius = 1.0;

            Assert.AreEqual(5 - Math.Sqrt(2), (double)Algorithm.distance_to_nearest_line_contact(radius, centre, direction, p1, p2), 0.000001);
        }

        [TestMethod]
        public void distance_to_nearest_contactTest4()
        {
            Point2D p1 = new Point2D(-1, 5.9);
            Point2D p2 = new Point2D(1, 6.0);
            Point2D direction = new Point2D(0, 1);
            Point2D centre = new Point2D(0, 5);
            double radius = 1.0;

            Assert.AreEqual(null, Algorithm.distance_to_nearest_line_contact(radius, centre, direction, p1, p2));
        }

        [TestMethod]
        public void offset_path_test1_open()
        {
            var path = new List<Point2D>
            {
                new Point2D(-10, 0),
                new Point2D(10, 0),
                new Point2D(10, 10),
                new Point2D(0, 20),
                new Point2D(0, 30)
            };

            var expected = new List<Point2D>
            {
                new Point2D(-10, 1),
                new Point2D(9, 1),
                new Point2D(9, 9.585786437626906),
                new Point2D(-1,19.5857864376269),
                new Point2D(-1,30)
            };

            var actual = Algorithm.offset_path(path, 1, false, true);
            AssertAreEqual(expected, actual, 0.000001);
        }

        [TestMethod]
        public void offset_path_test2_open_deepCorners()
        {
            var path = new List<Point2D>
            {
                new Point2D(-10, 0),
                new Point2D(10, 0),
                new Point2D(10, 10),
                new Point2D(0, 20),
                new Point2D(0, 30)
            };

            var expected = new List<Point2D>
            {
                new Point2D(-10, 1),
                new Point2D(9, 1),
                new Point2D(9.29289321881345,0.707106781186547),
                new Point2D(9, 1),
                new Point2D(9,9.58578643762691),
                new Point2D(9.07612046748871,9.61731656763491),
                new Point2D(9,9.58578643762691),
                new Point2D(-1,19.5857864376269),
                new Point2D(-1,30)
            };

            var actual = Algorithm.offset_path(path, 1, true, true);
            AssertAreEqual(expected, actual, 0.000001);
        }

        [TestMethod]
        public void offset_path_test3_closed()
        {
            var path = new List<Point2D>
            {
                new Point2D(-10, 0),
                new Point2D(10, 0),
                new Point2D(10, 10),
                new Point2D(0, 20),
                new Point2D(0, 30)
            };

            var expected = new List<Point2D>
            {
                new Point2D(9, 1),
                new Point2D(9, 9.585786437626906),
                new Point2D(-1,19.5857864376269),
                new Point2D(-1,23.8377223398316),
                new Point2D(-8.61257411327721,1)
            };

            var actual = Algorithm.offset_path(path, 1, false, false);
            AssertAreEqual(expected, actual, 0.000001);
        }

        [TestMethod]
        public void offset_path_test4_closed_deepCorners()
        {
            var path = new List<Point2D>
            {
                new Point2D(-10, 0),
                new Point2D(10, 0),
                new Point2D(10, 10),
                new Point2D(0, 20),
                new Point2D(0, 30)
            };

            var expected = new List<Point2D>
            {
                new Point2D(9, 1),
                new Point2D(9.29289321881345,0.707106781186547),
                new Point2D(9, 1),
                new Point2D(9,9.58578643762691),
                new Point2D(9.07612046748871,9.61731656763491),
                new Point2D(9,9.58578643762691),
                new Point2D(-1,19.5857864376269),
                new Point2D(-1,23.8377223398316),
                new Point2D(-0.160182243006967,29.0129125423625),
                new Point2D(-1,23.8377223398316),
                new Point2D(-8.61257411327721,1),
                new Point2D(-9.18875781482444,0.584710284663765),
                new Point2D(-8.61257411327721,1)
            };

            var actual = Algorithm.offset_path(path, 1, true, false);
            AssertAreEqual(expected, actual, 0.000001);
        }


        [TestMethod]
        public void SplitPathRemoveOverLaps_test1()
        {
            var path = new List<Point2D>
            {
                new Point2D(-10, 0),
                new Point2D(-0.1, 0),
                new Point2D(0, 0.1),
                new Point2D(0, 10),
             };

            var actual = Algorithm.offset_path(path, 1, false, false);
            var splitPath = Algorithm.SplitPathRemoveOverLaps(actual);

            var polygon = new Polygon2D(path);
            var sign = Math.Sign(polygon.SignedArea());

            var newPath = new List<Polygon2D>();
            foreach(var split in splitPath)
            {
                var poly = new Polygon2D(split);
                if (Math.Sign(poly.SignedArea()) == sign)
                {
                    newPath.Add(poly);
                }
            }
        }



        void AssertAreEqual(Point2D expected, Point2D actual, double precision)
        {
            Assert.AreEqual(expected.x, actual.x, precision);
            Assert.AreEqual(expected.y, actual.y, precision);
        }
        void AssertAreEqual(List<Point2D> expected, List<Point2D> actual, double precision)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for(int i = 0; i< expected.Count; i++)
            {
                AssertAreEqual(expected[i], actual[i], precision);
            }
        }
    }
}
