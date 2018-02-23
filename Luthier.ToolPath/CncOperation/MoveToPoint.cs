using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public class MoveToPoint : CncOperationBase
    {
        internal double? x;
        internal double? y;
        internal double? z;
        internal int? feedRate;

        public MoveToPoint(double? x, double? y, double? z, int? feedRate)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.feedRate = feedRate;
        }

        public override string ToString()
        {
            return string.Format("Move to point: x = {0}, y = {1}, z = {2}, feedRate = {3}", x,y,z,feedRate);
        }

        public double? GetX() => x;
        public double? GetY() => y;
        public double? GetZ() => z;
        public int? GetFeedRate() => feedRate;

        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
