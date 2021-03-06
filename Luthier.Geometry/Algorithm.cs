﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{


    public static class Algorithm
    {
        public static double DistancePointToLine(double[] p1, double[] l1, double[] l2)
        {
            double[] tangent = l2.Subtract(l1);
            tangent.NormaliseThis();

            double[] v = p1.Subtract(l1);
            double t = v.DotProduct(tangent);
            double[] p = l1.Add(tangent.Multiply(t));
            
            return p1.Subtract(p).L2Norm();
        }


        public static double? intersection_line_y(double y, Point2D p1, Point2D p2)
        {
            if((p1.Y != p2.Y && p1.Y <= y && y <= p2.Y) || (p1.Y != p2.Y && p2.Y <= y && y <= p1.Y))
            {
                double s = (y - p1.Y) / (p2.Y - p1.Y);
                return (1 - s) * p1.X + s * p2.X;
            }
            return null;
        }

        public static double? distance_to_nearest_line_contact(double radius, Point2D centre, Point2D direction, Point2D p1, Point2D p2)
        {
            //step 1: rotate so direction is positive x axis 
            var d = direction.ToNormalised();
            var p_1 = (p1 - centre);
            var p_2 = (p2 - centre);
            var q1 = new Point2D(d.X * p_1.X + d.Y * p_1.Y, -d.Y * p_1.X + d.X * p_1.Y);
            var q2 = new Point2D(d.X * p_2.X + d.Y * p_2.Y, -d.Y * p_2.X + d.X * p_2.Y);

            var t = (q2 - q1).ToNormalised();
            var d1 = intersection_line_y(t.X, q1, q2) + t.Y * radius;
            var d2 = intersection_line_y(-t.X, q1, q2) - t.Y * radius;

            if (d1 >= 0 && d2 >= 0)
            {
                return (d2 > d1) ? d1 : d2;
            }
            else if (d1 >= 0 && d2 == null)
            {
                return d1;
            }
            else if (d2 >= 0 && d1 == null)
            {
                return d2;
            }
            return null;
        }


        public static List<double[]> offset_path(List<double[]> path, double offsetDistance, bool deep_corners, bool open)
        {
            int j, k;

            var offset_path = new List<double[]>(path.Count);

            int count = open ? path.Count - 2 : path.Count;

            for (int i = 0; i < count; i++)
            {
                j = (i + 1) % path.Count;
                k = (i + 2) % path.Count;

                var v1 = new double[] { path[i][0], path[i][1] };
                var v2 = new double[] { path[j][0], path[j][1] };
                var v3 = new double[] { path[k][0], path[k][1] };

                var n1 = new double[] { v1[1] - v2[1], v2[0] - v1[0] }.Normalise().Multiply(offsetDistance);
                var n2 = new double[] { v2[1] - v3[1], v3[0] - v2[0] }.Normalise().Multiply(offsetDistance);

                var p1 = v1.Add(n1); 
                var p2 = v2.Add(n1); 
                var q1 = v2.Add(n2); 
                var q2 = v3.Add(n2);

                var intersect = Intersection.GetIntersection2D(p1, p2, q1, q2);

                if (i == 0 && open) offset_path.Add(p1);

                if (intersect != null)
                {
                    var ip = new double[] { intersect.Point.X, intersect.Point.Y };
                    offset_path.Add(ip);
                    if (deep_corners && IsInsideCorner(n1, q2.Subtract(q1)))
                    {
                        var corner = v2.Add(ip.Subtract(v2).Normalise().Multiply(offsetDistance));
                        offset_path.Add(corner);
                        offset_path.Add(ip);
                    }
                }
                if (k == path.Count - 1 && open)
                {
                    offset_path.Add(q2);
                }
            }
            return offset_path;

        }

        private static bool IsInsideCorner(double[] n1, double[] t2)
        {
            return (n1[0] * t2[0] + n1[1] * t2[1] > 0);
        }

        public static List<Point2D> AdjustedCurvaturePath(List<double[]> path, double curvaturePercentage)
        {
            if (path.Count < 3) return path.Select(x => new Point2D(x)).ToList();

            var result = new List<Point2D>(path.Count);

            result.Add(new Point2D(path[0]));
            result.Add(new Point2D(path[1]));

            for (int i = 2; i < path.Count; i++)
            {
                var p0 = new Point2D(path[i - 2]);
                var p1 = new Point2D(path[i - 1]);
                var p2 = new Point2D(path[i]);

                var r0 = result[i - 2];
                var r1 = result[i - 1];

                var r2 = GetNextPoint(p0, p1, p2, r0, r1, curvaturePercentage);

                result.Add(r2);
            }

            return result;
        }

        public static Point2D GetNextPoint(Point2D p0, Point2D p1, Point2D p2, Point2D r0, Point2D r1, double curvaturePercentage)
        {
            var pt = p1 - p0;
            var pn = new Point2D(-pt.Y, pt.X);

            var a = (p1 - p0).L2Norm();
            var b = (p2 - p1).L2Norm();
            var c = (p2 - p0).L2Norm();

            var sign = Math.Sign(pn.Dot(p2 - p1));
            double theta = (Math.PI - Math.Acos((a * a + b * b - c * c) / (2 * a * b))) * curvaturePercentage;

            var rt = (r1 - r0).ToNormalised();
            var rn = new Point2D(-rt.Y, rt.X);

            return r1 + b * (rt * Math.Cos(theta) + rn * Math.Sin(theta) * sign);
        }

        //public static List<List<Point2D>> SplitPathRemoveOverLaps(List<Point2D> polygon)
        //{
        //    for (int i = 0; i < polygon.Count; i++)
        //    {
        //        int i2 = (i + 1) % polygon.Count;

        //        for (int j = i + 2; j < polygon.Count; j++)
        //        {
        //            int j2 = (j + 1) % polygon.Count;

        //            if (j == i || j == i2 || j2 == i || j2 == i2) continue;

        //            var intersect = Intersection.GetIntersection(polygon[i], polygon[i2], polygon[j], polygon[j2]);
        //            if (intersect != null 
        //                && 0 <= intersect.Parameter1 && intersect.Parameter1 <= 1 
        //                && 0 <= intersect.Parameter2 && intersect.Parameter2 <= 1)
        //            {
        //                var p1 = new List<Point2D>();
        //                for (int k = 0; k <= i; k++) p1.Add(polygon[k]);
        //                p1.Add(intersect.Point);
        //                for (int k = j + 1; k < polygon.Count; k++) p1.Add(polygon[k]);

        //                var p2 = new List<Point2D>();
        //                p2.Add(intersect.Point);
        //                for (int k = i + 1; k <= j; k++) p2.Add(polygon[k]);

        //                var result = new List<List<Point2D>>();
        //                result.AddRange(SplitPathRemoveOverLaps(p1));
        //                result.AddRange(SplitPathRemoveOverLaps(p2));
        //                return result;
        //            }
        //        }

        //    }
        //    return new List<List<Point2D>> { polygon };
        //}



        public static List<Point2D> RemoveDuplicatePointsClosed(List<Point2D> path, double radius)
        {
            var result = new List<Point2D>();
            int j = 0;
            result.Add(path[j]);
            for (int i = 1; i < path.Count; i++)
            {
                if((path[i] - path[j]).L2Norm() >= radius)
                {
                    result.Add(path[i]);
                    j = i;
                }
            }
            if((result[0] - result[result.Count - 1]).L2Norm() < radius)
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;
        }

        public static List<Point2D> RemoveRedundantPointsClosed2(List<Point2D> path, double maxDistance, double minAngle)
        {
            int numberOfPoints = path.Count;
            List<Point2D> result = RemoveDuplicatePointsClosed(path, 0.002);

            for(int i = 0; i < 10; i++)
            { 
                numberOfPoints = result.Count;
                result = RemoveRedundantMidPoints(result, maxDistance, minAngle);

                if (result.Count == numberOfPoints)
                    break;
            }
            
            return result;
        }

        public static List<Point2D> RemoveRedundantMidPoints(List<Point2D> path, double maxDistance, double minAngle)
        {
            var result = new List<Point2D>();

            result.Add(path[0]);

            for(int i = 0; i < path.Count - 2; i++)
            {
                if(KeepMiddlePoint(path[i], path[i+1], path[i+2], maxDistance, minAngle))
                {
                    result.Add(path[i + 1]);
                }
                else
                {
                    result.Add(path[i + 2]);
                    i++;
                }
            }

            result.Add(path.Last());

            return result;
        }


        private static bool KeepMiddlePoint(Point2D p1, Point2D p2, Point2D p3, double maxDistance, double minAngle)
        {
            var a = (p2 - p1);
            var b = (p2 - p3);
            var c = (p3 - p1);

            var t = c.ToNormalised();
            var n = new Point2D(-t.Y, t.X);

            double d = Math.Abs((p2 - p1).Dot(n));

            double angle = Math.Acos((a.Dot(a) + b.Dot(b) - c.Dot(c)) / (2 * a.L2Norm() * b.L2Norm()));

            return d > maxDistance || angle < minAngle;
        }
    }

    
}
