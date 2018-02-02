using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public class SetSpindleState : CncOperationBase
    {
        internal EnumSpindleState? state;
        
        public SetSpindleState(EnumSpindleState? state)
        {
            this.state = state;
        }
        public override string accept(ICncOperationLanguageVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    public enum EnumSpindleState
    {
        OnClockwise,
        OnCounterClockwise,
        Off
    }
  
}
