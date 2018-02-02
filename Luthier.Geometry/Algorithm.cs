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


 
    }

    
}
