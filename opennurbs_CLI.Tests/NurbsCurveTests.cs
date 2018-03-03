using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using opennurbs_CLI;

namespace opennurbs_CLI.Tests
{
    [TestClass]
    public class NurbsCurveTests
    {

        /*
         * returns 2nd degree curve with 3 points on unit square, uniform unclamped knot vector
         */
        private NurbsCurve GetSmallCurve()
        {
            var curve = new NurbsCurve(2, 0, 3, 3);

            curve.SetCV(0, 3, new double[] { 0, 0 });
            curve.SetCV(1, 3, new double[] { 1, 0 });
            curve.SetCV(2, 3, new double[] { 1, 1 });

            for (int i = 0; i < curve.KnotCount; i++) curve.SetKnot(i, i);

            return curve;
        }

        private List<double[]> EvaluatePoints(NurbsCurve curve, double from, double to, int n)
        {
            var result = new List<double[]>();
            for (int i = 0; i < n; i++)
            {
                double t = from * (1 - (double)i / (n - 1)) + to * (double)i / (n - 1);
                result.Add(curve.Evaluate(t,1));
            }
            return result;
        }
        private List<double[]> ControlPoints(NurbsCurve curve)
        {
            var result = new List<double[]>();
            for (int i = 0; i < curve.CVCount; i++)
            {
                result.Add(curve.GetCV(i,3));
            }
            return result;
        }
        private List<double> Knots(NurbsCurve curve)
        {
            var result = new List<double>();
            for (int i = 0; i < curve.KnotCount; i++)
            {
                result.Add(curve.GetKnot(i));
            }
            return result;
        }


        [TestMethod]
        public void ClampEndsTest()
        {
            var curve = GetSmallCurve();
            var locus = EvaluatePoints(curve,curve.Domain.Min, curve.Domain.Max, 5);
            var CVs = ControlPoints(curve);
            var knots = Knots(curve);

            curve.ClampEnd(0);

            var locus2 = EvaluatePoints(curve, curve.Domain.Min, curve.Domain.Max, 5);
            var CVs2 = ControlPoints(curve);
            var knots2 = Knots(curve);

            curve.ClampEnd(1);

            var locus3 = EvaluatePoints(curve, curve.Domain.Min, curve.Domain.Max, 5);
            var CVs3 = ControlPoints(curve);
            var knots3 = Knots(curve);

        }




        [TestMethod]
        public void TestMethod1()
        {
 
            var curve = GetSmallCurve();
         

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
