using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class Point2D
    {
        public double x;
        public double y;

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double L2Norm() => (double) Math.Sqrt(x * x + y * y);

        public Point2D ToNormalised() => this * (1 / L2Norm());

        public double Distance(Point2D other) => (this - other).L2Norm();

        public static Point2D operator +(Point2D left, Point2D right) => new Point2D (left.x + right.x, left.y + right.y);
        
        public static Point2D operator -(Point2D left, Point2D right) => new Point2D (left.x - right.x, left.y - right.y);
        
        public static Point2D operator *(Point2D left, Point2D right) => new Point2D (left.x * right.x, left.y * right.y);
        
        public static Point2D operator *(Point2D left, double a) => new Point2D (left.x * a, left.y * a);
        
        public static Point2D operator *(double a, Point2D right) => new Point2D (a * right.x, a * right.y);

        //public static Point2D operator *(Point2D left, double a) => new Point2D((double)(left.x * a), (double)(left.y * a));

        //public static Point2D operator *(double a, Point2D right) => new Point2D((double)(a * right.x), (double)(a * right.y));

        public static Point2D operator /(Point2D left, double a) => new Point2D((double)(left.x / a), (double)(left.y / a));


        public override string ToString() => string.Format("Point2D({0},{1})", x, y);


        public override bool Equals(object obj)
        {
            if(obj is Point2D)
            {
                var point = (Point2D) obj;
                return (point.x == x && point.y == y);
            }
            return false;
        }
    }


}
