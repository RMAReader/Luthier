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
 
    }

    
}
