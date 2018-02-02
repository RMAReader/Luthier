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

        public double MinX { get => points.Min(p => p.x); }
        public double MaxX { get => points.Max(p => p.x); }

        public double MinY { get => points.Min(p => p.y); }
        public double MaxY { get => points.Max(p => p.y); }

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

    }
}
