using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Core;
using Luthier.Geometry;

namespace Luthier
{
    public class ScanLine
    {
        public double Y;
        public List<LineSegment1D> X;

        public ScanLine(double y, List<double> x)
        {
            if (x.Count % 2 != 0) throw new ArgumentException("Number of points must be a multiple of 2");

            this.Y = y;
            x.Sort();
            
            this.X = new List<LineSegment1D>();
            for (int i = 0; i < x.Count; i += 2) this.X.Add(new LineSegment1D(x[i], x[i+1]));
        }

        public ScanLine(double y, List<LineSegment1D> x)
        {
            this.Y = y;
            this.X = x;
        }

        public bool IsEmpty { get => (X.Count == 0); }

        public ScanLine ExtractOverlappingSegment(ScanLine scanline, double maxDistance)
        {
            if(scanline == null) return new ScanLine(Y, new List<LineSegment1D> { X.ExtractAt(0) });
            if (Math.Abs(this.Y - scanline.Y) < maxDistance)
            { 
                for(int i=0; i< X.Count; i++)
                {
                    if (Overlap(X[i], scanline.X[0])) return new ScanLine(Y, new List<LineSegment1D> { X.ExtractAt(i) });
                }
            }
            return null;
        }
        private bool Overlap(LineSegment1D line1, LineSegment1D line2) => !(line1.Max < line2.Min || line2.Max < line1.Min);

    }




}
