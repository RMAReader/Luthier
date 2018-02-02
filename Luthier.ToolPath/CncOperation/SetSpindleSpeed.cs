using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public class SetSpindleSpeed : CncOperationBase
    {
        internal int spindleSpeed;

        public SetSpindleSpeed(int spindleSpeed)
        {
            this.spindleSpeed = spindleSpeed;
        }
        public override string ToString()
        {
            return string.Format("Set spindle speed: {0}", spindleSpeed);
        }
        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
