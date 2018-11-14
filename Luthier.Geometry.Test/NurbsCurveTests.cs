using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry.Nurbs;

namespace Luthier.Geometry.Test
{
    [TestCategory("CorrectnessTests")]
    [TestClass]
    public class NurbsCurveTests
    {

        [TestMethod]
        public void NurbsCurve_Evaluate()
        {
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 3);
            curve.SetCV(0, new double[] { 0, 0 });
            curve.SetCV(1, new double[] { 0, 1 });
            curve.SetCV(2, new double[] { 1, 1 });

            double[] evaluationPoints = new double[] { 2, 2.5, 3 };

            double[][] expected = new double[][]
                {
                    new double[]{ 0.0, 0.5 },
                    new double[]{ 0.125, 0.875 },
                    new double[]{ 0.5, 1.0 }
                };


            for(int i=0; i < evaluationPoints.Length; i++)
            {
                double t = evaluationPoints[i];
                double[] e = expected[i];
                double[] a = curve.Evaluate(t);

                Assert.AreEqual(e[0], a[0], 1E-9);
                Assert.AreEqual(e[1], a[1], 1E-9);
            }

        }

        [TestMethod]
        public void NurbsCurve_EvaluateMany()
        {
            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 3);
            curve.SetCV(0, new double[] { 0, 0 });
            curve.SetCV(1, new double[] { 0, 1 });
            curve.SetCV(2, new double[] { 1, 1 });

            double[] evaluationPoints = new double[] { 2, 2.5, 3 };
            double[] expected = new double[] { 0.0, 0.5, 0.125, 0.875, 0.5, 1.0 };

            curve.SetEvaluationPoints(evaluationPoints);

            double[] actual = curve.RecalculateAtEvaluationPoints();

            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], 1E-9);
            }
        }

    }
}
