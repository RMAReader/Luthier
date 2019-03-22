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

        public double GetDuration()
        {
            return GetFeedRateDistances().Sum(x => (x.Key > 0) ? x.Value / x.Key : 0);
        }

        public double GetDistance()
        {
            return GetFeedRateDistances().Sum(x => x.Value);
        }

        public Dictionary<int, double> GetFeedRateDurations()
        {
            return GetFeedRateDistances().ToDictionary(x => x.Key, y => (y.Key > 0) ? y.Value / y.Key : 0);
        }

        public Dictionary<int, double> GetFeedRateDistances()
        {
            var result = new Dictionary<int, double>();
            int feedRate = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            foreach (CncOperationBase op in operations)
            {
                double dx = 0;
                double dy = 0;
                double dz = 0;

                SetFeedRate newFeedRate = op as SetFeedRate;
                if (newFeedRate != null)
                {
                    feedRate = newFeedRate.feedRate;
                }

                MoveToPoint newPoint = op as MoveToPoint;
                if (newPoint != null)
                {
                    if (newPoint.GetX().HasValue)
                    {
                        dx = newPoint.GetX().Value - x;
                        x = newPoint.GetX().Value;
                    }
                    if (newPoint.GetY().HasValue)
                    {
                        dy = newPoint.GetY().Value - y;
                        y = newPoint.GetY().Value;
                    }
                    if (newPoint.GetZ().HasValue)
                    {
                        dz = newPoint.GetZ().Value - z;
                        z = newPoint.GetZ().Value;
                    }

                    if (newPoint.GetFeedRate().HasValue) feedRate = newPoint.GetFeedRate().Value;

                }

                if (!result.ContainsKey(feedRate))
                {
                    result.Add(feedRate, 0);
                }

                result[feedRate] = result[feedRate] + Math.Sqrt(dx * dx + dy * dy + dz * dz);

            }
            return result;
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
