using Luthier.ToolPath.CncOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.CncOperation
{
    public interface ICncOperationLanguageVisitor
    {
        string visit(CncOperationBase operation);
        string visit(ChangeTool operation);
        string visit(MoveToPoint operation);
        string visit(SetAbsolutePositioning operation);
        string visit(SetFeedRate operation);
        string visit(SetSpindleSpeed operation);
        string visit(SetSpindleState operation);
        string visit(SetToolLengthOffset operation);
    }
}
