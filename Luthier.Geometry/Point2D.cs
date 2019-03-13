using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{

    public class Point
    {
        public double[] Data;

        public Point() { }
        public Point(int dimension) { Data = new double[dimension]; }

        public Point2D AsPoint2D() => new Point2D(Data);
        public Point3D AsPoint3D() => new Point3D(Data);
    }


    public class Point2D : Point
    {
        public double X { get => Data[0]; set => Data[0] = value; }
        public double Y { get => Data[1]; set => Data[1] = value; }
        public double this[int i] { get => Data[i]; set => Data[i] = value; }

        public Point2D() { Data = new double[] { 0, 0 }; }
        public Point2D(double x, double y){ Data = new double[] { x, y };}
        public Point2D(double[] data) { Data = new double[] { data[0], data[1] }; }

        public double L2Norm() => (double) Math.Sqrt(X * X + Y * Y);

        public Point2D ToNormalised() => this * (1 / L2Norm());

        public double Distance(Point2D other) => (this - other).L2Norm();

        public static Point2D operator +(Point2D left, Point2D right) => new Point2D (left.X + right.X, left.Y + right.Y);
        
        public static Point2D operator -(Point2D left, Point2D right) => new Point2D (left.X - right.X, left.Y - right.Y);
        
        public static Point2D operator *(Point2D left, Point2D right) => new Point2D (left.X * right.X, left.Y * right.Y);
        
        public static Point2D operator *(Point2D left, double a) => new Point2D (left.X * a, left.Y * a);
        
        public static Point2D operator *(double a, Point2D right) => new Point2D (a * right.X, a * right.Y);

        //public static Point2D operator *(Point2D left, double a) => new Point2D((double)(left.x * a), (double)(left.y * a));

        //public static Point2D operator *(double a, Point2D right) => new Point2D((double)(a * right.x), (double)(a * right.y));

        public static Point2D operator /(Point2D left, double a) => new Point2D((double)(left.X / a), (double)(left.Y / a));


        public override string ToString() => $"({X},{Y})"; 


        public override bool Equals(object obj)
        {
            if(obj is Point2D)
            {
                var point = (Point2D) obj;
                return (point.X == X && point.Y == Y);
            }
            return false;
        }
    }

    public class Point3D : Point
    {
        public double X { get => Data[0]; set => Data[0] = value; }
        public double Y { get => Data[1]; set => Data[1] = value; }
        public double Z { get => Data[2]; set => Data[2] = value; }
        public double this[int i] { get => Data[i]; set => Data[i] = value; }

        public Point3D() { Data = new double[] { 0, 0, 0 }; }
        public Point3D(double x, double y, double z) { Data = new double[] { x, y, z }; }
        public Point3D(double[] data) { Data = new double[] { data[0], data[1], data[2] }; }

        public override string ToString() => $"({X},{Y},{Z})";

    }


}
