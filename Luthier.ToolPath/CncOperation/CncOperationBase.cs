using Luthier.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.ToolPath.CncOperation
{
    public class CncOperationBase : ICncOperation
    {
        public virtual string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
