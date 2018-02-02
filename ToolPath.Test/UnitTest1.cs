using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.ToolPath;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Luthier.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var op1 = new MoveToPoint(x: null, y: 1.0f, z: 2.0f, feedRate: 500);
            var op2 = new SetSpindleSpeed(22000);
            var op3 = new ChangeTool(new BallNose());
            var op4 = new SetFeedRate(1000);
            var op5 = new SetAbsolutePositioning();
            var op6 = new SetSpindleState(EnumSpindleState.OnClockwise);

            List<ICncOperation> operations = new List<ICncOperation>();
            operations.Add(op1);
            operations.Add(op2);
            operations.Add(op3);
            operations.Add(op4);

            var st1 = op1.ToString();
            var st2 = op2.ToString();
            var st3 = op3.ToString();

            var gcode = new CncOperationLanguageVisitorGCode();
            var g1 = gcode.visit(op1);
            var g2 = gcode.visit(op2);
            var g3 = gcode.visit(op3);
            var g4 = gcode.visit(op4);
            var g5 = gcode.visit(op5);
            var g6 = gcode.visit(op6);

            var path = new Luthier.ToolPath.ToolPath();
            path.MoveToPoint(x: null, y: 1.0f, z: 2.0f, feedRate: 500);
            path.SetSpindleSpeed(22000);
            path.SetTool(new BallNose());
            path.SetFeedRate(1000);
            path.SetAbsolutePositioning();
            path.SetSpindleState(EnumSpindleState.OnClockwise);

            var gcodelist = new List<string>();
            foreach(ICncOperation op in path)
            {
                gcodelist.Add(op.accept(gcode));
            }

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            var bigPath = new Luthier.ToolPath.ToolPath();
            for(int i = 0; i < 600; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    bigPath.MoveToPoint(i, j, null, null);
                }
            }

            var t1 = sw.ElapsedMilliseconds;
            sw.Restart();

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Richard\Documents\Development\ToolPath\TestData\toolpath.txt"))
            {
                foreach (ICncOperation op in bigPath)
                {
                    file.WriteLine(op.accept(gcode));
                }
            }
           

            var t2 = sw.ElapsedMilliseconds;
        

        }
    }
}
