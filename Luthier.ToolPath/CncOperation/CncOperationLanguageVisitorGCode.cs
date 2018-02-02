using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.CncOperation;
using Luthier.ToolPath.CncOperation;

namespace Luthier.CncOperation
{
    public class CncOperationLanguageVisitorGCode : ICncOperationLanguageVisitor
    {

        public string visit(CncOperationBase op)
        {
            if (op is MoveToPoint) return visit(op as MoveToPoint);
            if (op is SetSpindleSpeed) return visit(op as SetSpindleSpeed);
            if (op is SetFeedRate) return visit(op as SetFeedRate);
            if (op is ChangeTool) return visit(op as ChangeTool);
            if (op is SetAbsolutePositioning) return visit(op as SetAbsolutePositioning);
            if (op is SetToolLengthOffset) return visit(op as SetToolLengthOffset);
            if (op is SetSpindleState) return visit(op as SetSpindleState);

            return "";
        }

        public string visit(MoveToPoint op)
        {
            StringBuilder sb = new StringBuilder();
            if (op.x.HasValue || op.y.HasValue || op.z.HasValue )
            {
                sb.Append("G01");
                if (op.x.HasValue) sb.AppendFormat("X{0:0.000}", op.x);
                if (op.y.HasValue) sb.AppendFormat("Y{0:0.000}", op.y);
                if (op.z.HasValue) sb.AppendFormat("Z{0:0.000}", op.z);
                if (op.feedRate.HasValue) sb.AppendFormat("F{0}", op.feedRate);
            }
            return sb.ToString();
        }
        public string visit(SetSpindleSpeed op)
        {
            return string.Format("S{0}", op.spindleSpeed);
        }
        public string visit(SetFeedRate op)
        {
            return string.Format("F{0}", op.feedRate);
        }
        public string visit(ChangeTool op)
        {
            return string.Format("({0})", op.ToString());
        }
        public string visit(SetAbsolutePositioning op)
        {
            return @"G90";
        }
        public string visit(SetToolLengthOffset op)
        {
            if (op.toolTableIndex.HasValue)
            {
                return string.Format("G43 H{0}", op.toolTableIndex);
            }
            return @"G49";
        }
        public string visit(SetSpindleState op)
        {
            return (op.state == EnumSpindleState.Off)? @"M05" :
                    (op.state == EnumSpindleState.OnClockwise)? @"M03" :
                    (op.state == EnumSpindleState.OnCounterClockwise)? @"M04" : @"";
        }
    }
}
