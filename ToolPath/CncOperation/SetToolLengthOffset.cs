using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public class SetToolLengthOffset : CncOperationBase
    {
        internal int? toolTableIndex;
        public SetToolLengthOffset(int? toolTableIndex)
        {
            this.toolTableIndex = toolTableIndex;
        }
        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
