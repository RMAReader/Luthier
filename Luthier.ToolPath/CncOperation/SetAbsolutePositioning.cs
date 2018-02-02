using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    /*
     * sets state of toolpath so that points represent absolute positions opposed to
     * positions relative to previous point
     */
    public class SetAbsolutePositioning : CncOperationBase
    {
        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
