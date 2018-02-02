using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry;

namespace Luthier
{
    public class PathSection2D
    {
        private List<Point2D> points = new List<Point2D>();


        public Point2D First() => points.First();
        public Point2D Last() => points.Last();
        public void Add(Point2D p) => points.Add(p);
        public int Count { get => points.Count; }
        public List<Point2D>  Points { get => points;}
    }

}
