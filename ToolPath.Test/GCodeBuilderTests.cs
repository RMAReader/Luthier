using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Luthier.CncOperation;
using Luthier.CncTool;

namespace Luthier.Test
{
    [TestClass]
    public class GCodeBuilderTests
    {
        [TestMethod]
        public void Test_SetSpindleState()
        {
            var op1 = new SetSpindleState(EnumSpindleState.OnClockwise);
            var op2 = new SetSpindleState(EnumSpindleState.OnCounterClockwise);
            var op3 = new SetSpindleState(EnumSpindleState.Off);

            var gcode = new CncOperationLanguageVisitorGCode();

            Assert.AreEqual("M03", gcode.visit(op1));
            Assert.AreEqual("M04", gcode.visit(op2));
            Assert.AreEqual("M05", gcode.visit(op3));
        }
        [TestMethod]
        public void Test_SetSpindleSpeed()
        {
            var op = new SetSpindleSpeed(22000);
            
            var gcode = new CncOperationLanguageVisitorGCode();

            Assert.AreEqual("S22000", gcode.visit(op));
        }
        [TestMethod]
        public void Test_SetFeedRate()
        {
            var op = new SetFeedRate(500);

            var gcode = new CncOperationLanguageVisitorGCode();

            Assert.AreEqual("F500", gcode.visit(op));
        }
        [TestMethod]
        public void Test_SetToolLengthOffset()
        {
            var op1 = new SetToolLengthOffset(5);
            var op2 = new SetToolLengthOffset(null);

            var gcode = new CncOperationLanguageVisitorGCode();

            Assert.AreEqual("G43 H5", gcode.visit(op1));
            Assert.AreEqual("G49", gcode.visit(op2));
        }
        [TestMethod]
        public void Test_SetAbsolutePositioning()
        {
            var op = new SetAbsolutePositioning();

            var gcode = new CncOperationLanguageVisitorGCode();

            Assert.AreEqual("G90", gcode.visit(op));
        }
        [TestMethod]
        public void Test_ChangeTool()
        {
            var op1 = new ChangeTool(new BallNose());
            var op2 = new ChangeTool(new EndMill());

            var gcode = new CncOperationLanguageVisitorGCode();

            var str1 = gcode.visit(op1);
            var str2 = gcode.visit(op2);

            Assert.AreEqual(true, str1.StartsWith("(") && str1.EndsWith(")"));
            Assert.AreEqual(true, str2.StartsWith("(") && str2.EndsWith(")"));
        }
        [TestMethod]
        public void Test_MoveToPoint()
        {
            double x = 20.0f;
            double y = -50.5f;
            double z = 15.6f;
            int feedRate = 500;

            var op1 = new MoveToPoint(x,y,z,feedRate);
            var op2 = new MoveToPoint(x, y, z, null);
            var op3 = new MoveToPoint(x, y, null, feedRate);
            var op4 = new MoveToPoint(x, null, z, feedRate);
            var op5 = new MoveToPoint(null, y, z, feedRate);
            var op6 = new MoveToPoint(x, y, null, null);
            var op7 = new MoveToPoint(x, null, z, null);
            var op8 = new MoveToPoint(null, y, z, null);
            var op9 = new MoveToPoint(x, null, null, feedRate);
            var op10 = new MoveToPoint(null, y, null, feedRate);
            var op11 = new MoveToPoint(null, null, z, feedRate);
            var op12 = new MoveToPoint(x, null, null, null);
            var op13 = new MoveToPoint(null, y, null, null);
            var op14 = new MoveToPoint(null, null, z, null);
            var op15 = new MoveToPoint(null, null, null, feedRate);
            var op16 = new MoveToPoint(null, null, null, null);
            
            var gcode = new CncOperationLanguageVisitorGCode();

            Assert.AreEqual("G01X20.000Y-50.500Z15.600F500", gcode.visit(op1));
            Assert.AreEqual("G01X20.000Y-50.500Z15.600", gcode.visit(op2));
            Assert.AreEqual("G01X20.000Y-50.500F500", gcode.visit(op3));
            Assert.AreEqual("G01X20.000Z15.600F500", gcode.visit(op4));
            Assert.AreEqual("G01Y-50.500Z15.600F500", gcode.visit(op5));
            Assert.AreEqual("G01X20.000Y-50.500", gcode.visit(op6));
            Assert.AreEqual("G01X20.000Z15.600", gcode.visit(op7));
            Assert.AreEqual("G01Y-50.500Z15.600", gcode.visit(op8));
            Assert.AreEqual("G01X20.000F500", gcode.visit(op9));
            Assert.AreEqual("G01Y-50.500F500", gcode.visit(op10));
            Assert.AreEqual("G01Z15.600F500", gcode.visit(op11));
            Assert.AreEqual("G01X20.000", gcode.visit(op12));
            Assert.AreEqual("G01Y-50.500", gcode.visit(op13));
            Assert.AreEqual("G01Z15.600", gcode.visit(op14));
            Assert.AreEqual("", gcode.visit(op15));
            Assert.AreEqual("", gcode.visit(op16));

        }
    }
}
