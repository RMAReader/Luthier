﻿using Luthier.Geometry.BSpline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class Intersection
    {

        public class LineSegmentIntersection
        {
            public double Parameter1;
            public double Parameter2;
            public Point2D Point;

            public bool LineSegmentsIntersect() => (0 <= Parameter1 && Parameter1 <= 1 && 0 <= Parameter2 && Parameter2 <= 1);
        }


        public class CurveIntersection
        {
            public double Parameter1;
            public Curve curve1;
            public double Parameter2;
            public Curve curve2;
            public Point2D Point;
        }



        public static LineSegmentIntersection GetIntersection(Point2D p1, Point2D p2, Point2D q1, Point2D q2)
        {
            double a = p1.x - p2.x;
            double b = p1.y - p2.y;
            double c = q1.x - q2.x;
            double d = q1.y - q2.y;

            double det = a * d - b * c;
            if (det == 0) { return null; }

            double e = q2.x - p2.x;
            double f = q2.y - p2.y;

            double g = (d * e - c * f) / det;
            double h = (b * e - a * f) / det;

            return new LineSegmentIntersection()
            {
                Parameter1 = g,
                Parameter2 = h,
                Point = new Point2D(g * p1.x + (1 - g) * p2.x, g * p1.y + (1 - g) * p2.y)
            };

        }


        public static List<CurveIntersection> GetIntersectionList(Curve c1, Curve c2, double Error)
        {

            var result = new List<CurveIntersection>();

            int max_itr = 100;

            var d1 = c1.DeepCopy();
            var d2 = c2.DeepCopy();

            d1.CloseFront();
            d1.CloseBack();

            d2.CloseFront();
            d2.CloseBack();

            for (int itr = 1; itr <= max_itr; itr++)
            {

                var intersections = new List<Tuple<LineSegmentIntersection, int, int>>();
                for (int i = 0; i < d1.points.Count() - 1; i++)
                {
                    for (int j = 0; j < d2.points.Count() - 1; j++)
                    {
                        var intersection = GetIntersection(d1.points[i], d1.points[i + 1], d2.points[j], d2.points[j + 1]);
                        if (intersection != null && intersection.LineSegmentsIntersect())
                        {
                            intersections.Add(new Tuple<LineSegmentIntersection, int, int>(intersection, i, j));
                        }
                    }
                }

                var knots1 = new List<double>();
                var knots2 = new List<double>();
                foreach (var intersection in intersections)
                {
                    double t1 = 0;
                    for (int k = intersection.Item2; k < intersection.Item2 + d1.knot.p + 1; k++)
                    {
                        t1 += intersection.Item1.Parameter1 * d1.knot.data[k] + (1 - intersection.Item1.Parameter1) * d1.knot.data[k + 1];
                    }
                    knots1.Add(t1 / (d1.knot.p + 1));

                    double t2 = 0;
                    for (int k = intersection.Item3; k < intersection.Item3 + d2.knot.p + 1; k++)
                    {
                        t2 += intersection.Item1.Parameter2 * d2.knot.data[k] + (1 - intersection.Item1.Parameter2) * d2.knot.data[k + 1];
                    }
                    knots2.Add(t2 / (d2.knot.p + 1));
                }

                d1.InsertKnots(knots1);
                d2.InsertKnots(knots2);

                double resid = 0;
                for (int i = 0; i < knots1.Count; i++)
                {
                    var p1 = d1.Evaluate(knots1[i]);
                    var p2 = d2.Evaluate(knots2[i]);
                    resid += (p2 - p1).L2Norm();
                }

                if (resid < Error)
                {
                    for (int i = 0; i < knots1.Count; i++)
                    {
                        result.Add(new CurveIntersection
                        {
                            Point = d1.Evaluate(knots1[i]),
                            Parameter1 = knots1[i],
                            curve1 = c1,
                            Parameter2 = knots2[i],
                            curve2 = c2
                        });
                    }
                    break;
                }
            }
            return result;
        }



        public static CurveIntersection GetIntersection(Curve c1, Curve c2, Point2D centre, double Error)
        {
            if (c1 == null || c2 == null) return null;

            int max_itr = 100;

            var d1 = c1.DeepCopy();
            var d2 = c2.DeepCopy();

            d1.CloseFront();
            d1.CloseBack();

            d2.CloseFront();
            d2.CloseBack();

            for (int itr = 1; itr <= max_itr; itr++)
            {

                var intersections = new List<Tuple<LineSegmentIntersection,int,int>>();
                for (int i = 0; i < d1.points.Count() - 1; i++)
                {
                    for (int j = 0; j < d2.points.Count() - 1; j++)
                    {
                        var intersection = GetIntersection(d1.points[i], d1.points[i + 1], d2.points[j], d2.points[j + 1]);
                        if (intersection != null && intersection.LineSegmentsIntersect())
                        {
                            intersections.Add(new Tuple<LineSegmentIntersection, int, int>(intersection,i,j));
                        }
                    }
                }

                if (intersections.Count == 0) return null;

                var closestIntersection = intersections.Select(x => new { x, distance = centre.Distance(x.Item1.Point) }).OrderBy(x => x.distance).First().x;

                double t1 = 0;
                for (int k = closestIntersection.Item2; k < closestIntersection.Item2 + d1.knot.p + 1; k++)
                {
                    t1 += closestIntersection.Item1.Parameter1 * d1.knot.data[k] + (1 - closestIntersection.Item1.Parameter1) * d1.knot.data[k + 1];
                }
                t1 /= (d1.knot.p + 1);

                double t2 = 0;
                for (int k = closestIntersection.Item3; k < closestIntersection.Item3 + d2.knot.p + 1; k++)
                {
                    t2 += closestIntersection.Item1.Parameter2 * d2.knot.data[k] + (1 - closestIntersection.Item1.Parameter2) * d2.knot.data[k + 1];
                }
                t2 /= (d2.knot.p + 1);
                

                d1.InsertKnots(new List<double> { t1 });
                d2.InsertKnots(new List<double> { t2 });

                var p1 = d1.Evaluate(t1);
                var p2 = d2.Evaluate(t2);

                if ((p2 - p1).L2Norm() < Error)
                {
                    return new CurveIntersection
                    {
                        Point = p1,
                        Parameter1 = t1,
                        curve1 = c1,
                        Parameter2 = t2,
                        curve2 = c2
                    };
                }
            }
            return null;
        }



    }
}