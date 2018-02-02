using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class LineSegment1D
    {
        public double p1;
        public double p2;

        public LineSegment1D(double p1, double p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public double Min { get => Math.Min(p1, p2); }
        public double Max { get => Math.Max(p1, p2); }

        public double ClosestPoint(double x) => Math.Min(Math.Abs(p1 - x), Math.Abs(p2 - x));
        public double FurthestPoint(double x) => Math.Max(Math.Abs(p1 - x), Math.Abs(p2 - x));
    }
}
