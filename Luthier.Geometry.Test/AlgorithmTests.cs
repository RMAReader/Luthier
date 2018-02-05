﻿using System;
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
    }
}