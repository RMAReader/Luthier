using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Core;
using Luthier.Geometry;

namespace Luthier
{
    public class ScanLinePath2D
    {
        private List<PathSection2D> path;
        private readonly List<Polygon2D> boundary;
        private readonly double step;

        public ScanLinePath2D(List<Polygon2D> boundary, double step)
        {
            this.boundary = boundary;
            this.step = step;
            BuildPath();
        }

        public int Count { get => path.Count; }

        public List<PathSection2D> Sections { get => path; }

        private void BuildPath()
        {
            path = new List<PathSection2D>();

            var scanLines = ConstructScanLineSet();

            while (scanLines.Count > 0) path.Add(ExtractRegion(ref scanLines));

        }



        private List<ScanLine> ConstructScanLineSet()
        {
            var result = new List<ScanLine>();

            foreach (double y in RangeHelper.Between(
                boundary.Min(r => r.MinY),
                boundary.Max(r => r.MaxY),
                step))
            {
                List<double> intersects = new List<double>();

                foreach (var polygon in boundary) intersects.AddRange(polygon.IntersectXAxis(y));

                if (intersects.Count > 0) result.Add(new ScanLine(y, intersects));
            }
            return result;
        }


        private PathSection2D ExtractRegion(ref List<ScanLine> data)
        {
            var region = new PathSection2D();

            ScanLine segment = null;

            foreach (var scanline in data)
            {
                segment = scanline.ExtractOverlappingSegment(segment, step);
                if (segment == null) break;

                var p1 = new Point2D(segment.X[0].p1, segment.Y);
                var p2 = new Point2D(segment.X[0].p2, segment.Y);

                if (region.Count > 0 && Math.Abs(region.Last().X - p1.X) < Math.Abs(region.Last().X - p2.X))
                {
                    region.Add(p1);
                    region.Add(p2);
                }
                else
                {
                    region.Add(p2);
                    region.Add(p1);
                }
            }

            data = data.Where(s => s.IsEmpty == false).ToList();

            return region;
        }


    }

}
