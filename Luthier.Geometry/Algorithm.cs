using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{


    public static class Algorithm
    {

        public static double? intersection_line_y(double y, Point2D p1, Point2D p2)
        {
            if((p1.y != p2.y && p1.y <= y && y <= p2.y) || (p1.y != p2.y && p2.y <= y && y <= p1.y))
            {
                double s = (y - p1.y) / (p2.y - p1.y);
                return (1 - s) * p1.x + s * p2.x;
            }
            return null;
        }

        public static double? distance_to_nearest_line_contact(double radius, Point2D centre, Point2D direction, Point2D p1, Point2D p2)
        {
            //step 1: rotate so direction is positive x axis 
            var d = direction.ToNormalised();
            var p_1 = (p1 - centre);
            var p_2 = (p2 - centre);
            var q1 = new Point2D(d.x * p_1.x + d.y * p_1.y, -d.y * p_1.x + d.x * p_1.y);
            var q2 = new Point2D(d.x * p_2.x + d.y * p_2.y, -d.y * p_2.x + d.x * p_2.y);

            var t = (q2 - q1).ToNormalised();
            var d1 = intersection_line_y(t.x, q1, q2) + t.y * radius;
            var d2 = intersection_line_y(-t.x, q1, q2) - t.y * radius;

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


        //public static List<Point2D> offset_path(List<Point2D> path, double tool_radius, bool deep_corners, bool open)
        //{
        //    int j, k;
        //    var offset_path = new List<Point2D>(path.Count);

        //    int count = open ? path.Count - 2 : path.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        j = (i + 1) % path.Count;
        //        k = (i + 2) % path.Count;

        //        var v1 = path[i];
		      //  var v2 = path[j];
		      //  var v3 = path[k];

		      //  var n1 = new Point2D(v1.y - v2.y, v2.x - v1.x).ToNormalised() * tool_radius; 
        //        var n2 = new Point2D(v2.y - v3.y, v3.x - v2.x).ToNormalised() * tool_radius;

		      //  var p1 = new Point2D(v1.x + n1.x, v1.y + n1.y);
		      //  var p2 = new Point2D(v2.x + n1.x, v2.y + n1.y);
		      //  var q1 = new Point2D(v2.x + n2.x, v2.y + n2.y);
		      //  var q2 = new Point2D(v3.x + n2.x, v3.y + n2.y);

		      //  var intersect = Intersection.GetIntersection(p1, p2, q1, q2);

		      //  if (i == 0 && open) offset_path.Add(p1);

        //        if (intersect != null)
        //        {
        //            offset_path.Add(intersect.Point);
        //            if (deep_corners && IsInsideCorner(n1, q2 - q1))
        //            {
        //                var corner = v2 + (intersect.Point - v2).ToNormalised() * tool_radius;
        //                offset_path.Add(corner);
        //                offset_path.Add(intersect.Point);
        //            }
        //        }
		      //  if (k == path.Count - 1 && open){
			     //   offset_path.Add(q2);
		      //  }
	       // }
        //    return offset_path;

        //}

        //private static bool IsInsideCorner(Point2D n1, Point2D t2)
        //{
        //    return (n1.x * t2.x + n1.y * t2.y > 0);
        //}

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



        public static List<Point2D> RemoveRedundantPointsClosed(List<Point2D> path, double radius)
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
    }

    
}
