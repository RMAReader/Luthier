using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class LineSegment2D
    {
        public Point2D p1;
        public Point2D p2;

        public LineSegment2D(Point2D p1, Point2D p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public double Length() => (p2 - p1).L2Norm();

    }
}
