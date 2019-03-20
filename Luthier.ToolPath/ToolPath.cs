using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.ToolPath.CncOperation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.ToolPath
{
    public class ToolPath : IEnumerable<CncOperationBase>
    {
        internal List<CncOperationBase> operations;

        public ToolPath()
        {
            operations = new List<CncOperationBase>();
        }

        internal void Add(CncOperationBase operation) => operations.Add(operation);

        public void SetAbsolutePositioning() => Add(new SetAbsolutePositioning());
        public void SetSpindleState(EnumSpindleState spindleSate) => Add(new SetSpindleState(spindleSate));
        public void SetSpindleSpeed(int spindleSpeed) => Add(new SetSpindleSpeed(spindleSpeed));
        public void SetFeedRate(int feedRate) => Add(new SetFeedRate(feedRate));
        public void SetTool(BaseTool tool) => Add(new ChangeTool(tool));
        public void MoveToPoint(double? x, double? y, double? z, int? feedRate) => Add(new MoveToPoint(x, y, z, feedRate));


        public void SavePathAsCncMachineCode(ICncOperationLanguageVisitor language, string filepath)
        {
            var cncLanguage = new System.Text.StringBuilder();
            foreach (CncOperationBase op in operations)
            {
                cncLanguage.AppendLine(language.visit(op));
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath))
            {
                file.Write(cncLanguage.ToString());
            }
        }

        public string ToGCode()
        {
            var gcode = new System.Text.StringBuilder();
            var language = new CncOperationLanguageVisitorGCode();
            foreach (CncOperationBase op in operations)
            {
                gcode.AppendLine(language.visit(op));
            }
            return gcode.ToString();
        }


        public IEnumerator GetEnumerator()
        {
            return operations.GetEnumerator();
        }

        IEnumerator<CncOperationBase> IEnumerable<CncOperationBase>.GetEnumerator()
        {
            return operations.GetEnumerator();
        }
    }
}
