using Luthier.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class Polygon2D
    {
        private List<Point2D> points;

        public Polygon2D()
        {
            this.points = new List<Point2D>();
        }
        public Polygon2D(List<Point2D> points)
        {
            this.points = points;
        }

        public void Add(Point2D p) => points.Add(p);
        public void Add(IEnumerable<Point2D> p) => points.AddRange(p);

        public double MinX { get => points.Min(p => p.X); }
        public double MaxX { get => points.Max(p => p.X); }

        public double MinY { get => points.Min(p => p.Y); }
        public double MaxY { get => points.Max(p => p.Y); }

        public List<double> IntersectYAxis()
        {
            return new List<double>();
        }
        public List<double> IntersectXAxis(double y)
        {
            List<double> intersects = new List<double>();
            for(int i=0; i<points.Count; i++)
            {
                int j = (i + 1 < points.Count) ? i + 1 : 0;
                double? x = Algorithm.intersection_line_y(y, points[i], points[j]);
                if (x.HasValue) intersects.Add(x.Value);
            }
            return intersects;
        }

        public double Distance(Point2D p) => points.Min(x => x.Distance(p));

        public List<Point2D> GetPoints() => points;

        public Point2D Centre
        {
            get
            {
                var centre = new Point2D(0, 0);
                points.Select(x => centre + x);
                return centre / points.Count;
            }
        }



        /*
            The signed area can be used to determine whether points are clockwise or anti-clockwise - its sign indicates direction
            https://stackoverflow.com/questions/1165647/how-to-determine-if-a-list-of-polygon-points-are-in-clockwise-order
        */
        public double SignedArea()
        {
            double sa = 0;
            int j = 0;
            for(int i = 0; i < points.Count; i++)
            {
                j = i + 1;
                if (j == points.Count) j = 0;
                sa += (points[j].X - points[i].X) * (points[j].Y + points[i].Y);
            }
            return sa / 2;
        }


        public void Reverse()
        {
            points.Reverse();
        }

        public void Translate(Point2D p)
        {
            for (int i = 0; i < points.Count; i++) points[i] += p;
        }

        public void RemoveRedundantPoints(double radius)
        {
            points = Algorithm.RemoveRedundantPointsClosed(points, radius);
        }

    }
}
