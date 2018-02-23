using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using opennurbs_CLI;

namespace opennurbs_CLI.Tests
{
    [TestClass]
    public class NurbsCurveTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            int dummy = 1;

            var curve = new opennurbs_CLI.NurbsCurve(2, 0, 3, 3);

            curve.SetCV(0, 3, new double[] { 0, 0 });
            curve.SetCV(1, 3, new double[] { 1, 0 });
            curve.SetCV(2, 3, new double[] { 1, 1 });

            for (int i = 0; i < curve.KnotCount; i++) curve.SetKnot(i, i);

            var points = new List<double[]>();
            for (int i = 0; i < 3; i++)
            {
                points.Add(curve.GetCV(i, 3));
            }

            var knots = new List<double>();
            for (int i = 0; i < curve.KnotCount; i++) knots.Add(curve.GetKnot(i));

            var domain = curve.Domain;

            var p1 = new double[2]; curve.Evaluate(1.0, 1, p1);
            var p2 = new double[2]; curve.Evaluate(1.2, 1, p2);
            var p3 = new double[2]; curve.Evaluate(1.5, 1, p3);
            var p4 = new double[2]; curve.Evaluate(1.9, 1, p4);
            var p5 = new double[2]; curve.Evaluate(2.0, 1, p5);
            var degree1 = curve.Degree;
            var nCV1 = curve.CVCount;

            curve.InsertKnot(1.5, 1);

            var p21 = curve.Evaluate(1.0, 1);
            var p22 = curve.Evaluate(1.2, 1);
            var p23 = curve.Evaluate(1.5, 1);
            var p24 = curve.Evaluate(1.9, 1);
            var p25 = curve.Evaluate(2.0, 1);
            var degree2 = curve.Degree;
            var knots2 = curve.KnotCount;
            var nCV2 = curve.CVCount;

            curve.IncreaseDegree(4);

            var p31 = curve.Evaluate(1.0, 1);
            var p32 = curve.Evaluate(1.2, 1);
            var p33 = curve.Evaluate(1.5, 1);
            var p34 = curve.Evaluate(1.9, 1);
            var p35 = curve.Evaluate(2.0, 1);
            var degree3 = curve.Degree;
            var knots3 = curve.KnotCount;
            var nCV3 = curve.CVCount;
        }
    }
}
