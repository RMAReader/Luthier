using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public class SetFeedRate : CncOperationBase
    {
        internal int feedRate;

        public SetFeedRate(int feedRate)
        {
            this.feedRate = feedRate;
        }
        public override string ToString()
        {
            return string.Format("Set feed rate: {0}", feedRate);
        }
        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
