using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.CncTool;
using Luthier.ToolPath.CncOperation;

namespace Luthier.CncOperation
{
    public class ChangeTool : CncOperationBase
    {
        internal BaseTool newTool;

        public ChangeTool(BaseTool newTool)
        {
            this.newTool = newTool;
        }

        public override string ToString()
        {
            return string.Format("Change tool to {0}", newTool);
        }

        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }
}
