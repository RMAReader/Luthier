using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Luthier.Geometry.Test
{
    [TestCategory("CorrectnessTests")]
    [TestClass]
    public class NurbsAlgorithmTests
    {
        public delegate void BasisFunctionEvaluation(double expected, int i, int degree, double[] knot, double t);
        public delegate void CurveEvaluation(double expected, int degree, int knotIX, double[] knot, double[] cvDataBlock, double t);

     
        [TestMethod]
        public void Find_Knot_Span()
        {
            int degree = 2;
            double[] knot = new double[] { 0, 1, 3, 6, 10, 15 };

            double[] t = { 0, 1, 1.5, 3, 3.2, 6, 6.5, 10, 11 };
            int[] expected = new int[] { 2, 2, 2, 2, 2, 2, 2, 2 };

            for (int i = 0; i < expected.Length; i++)
            {
                int knotIX = Nurbs.Algorithm.Find_Knot_Span(degree, knot, t[i]);
                Assert.AreEqual(expected[i], knotIX);
            }

        }

        [TestMethod]
        public void BasisFunction_Evaluate()
        {
            BasisFunctionEvaluation handler2 = (expected, i, degree, knot, t) => 
            {
                Assert.AreEqual(expected, Nurbs.Algorithm.BasisFunction_Evaluate(i, degree, knot, t), 1E-12);
            };

            BasisFunction_Evaluate_ComplexKnot_DegreeZero(handler2);
            BasisFunction_Evaluate_ComplexKnot_DegreeOne(handler2);
            BasisFunction_Evaluate_ComplexKnot_DegreeTwo(handler2);

        }

        [TestMethod]
        public void BasisFunction_Evaluate_DegreeTwo()
        {
            BasisFunctionEvaluation handler2 = (expected, i, degree, knot, t) =>
            {
                double v1 = Nurbs.Algorithm.BasisFunction_Evaluate_DegreeTwo(i, knot, t);
                Assert.AreEqual(expected, v1, 1E-12);
            };

            BasisFunction_Evaluate_ComplexKnot_DegreeTwo(handler2);

        }

        [TestMethod]
        public void BasisFunction_EvaluateFirstDerivative()
        {
            BasisFunctionEvaluation handler2 = (expected, i, degree, knot, t) =>
            {
                double v1 = Nurbs.Algorithm.BasisFunction_EvaluateFirstDerivative(i, degree, knot, t);
                Assert.AreEqual(expected, v1, 1E-12);
            };

            BasisFunction_EvaluateFirstDerivative_ComplexKnot_DegreeTwo(handler2);

        }

        [TestMethod]
        public void BasisFunction_EvaluateFirstDerivative_DegreeTwo()
        {
            BasisFunctionEvaluation handler2 = (expected, i, degree, knot, t) =>
            {
                double v1 = Nurbs.Algorithm.BasisFunction_EvaluateFirstDerivative_DegreeTwo(i, knot, t);
                Assert.AreEqual(expected, v1, 1E-12);
            };

            BasisFunction_EvaluateFirstDerivative_ComplexKnot_DegreeTwo(handler2);

        }

        [TestMethod]
        public void BasisFunction_EvaluateSecondDerivative_DegreeTwo()
        {
            BasisFunctionEvaluation handler2 = (expected, i, degree, knot, t) =>
            {
                double v1 = Nurbs.Algorithm.BasisFunction_EvaluateSecondDerivative_DegreeTwo(i, knot, t);
                Assert.AreEqual(expected, v1, 1E-12);
            };

            BasisFunction_EvaluateSecondDerivative_ComplexKnot_DegreeTwo(handler2);

        }

        [TestMethod]
        public void BasisFunction_EvaluateAllDerivatives_DegreeTwo()
        {
            BasisFunctionEvaluation handler0 = (expected, i, degree, knot, t) =>
            {
                double[] values = new double[3];
                Nurbs.Algorithm.BasisFunction_EvaluateAllDerivatives_DegreeTwo(i, knot, t, ref values);
                Assert.AreEqual(expected, values[0], 1E-12);
            };

            BasisFunction_Evaluate_ComplexKnot_DegreeTwo(handler0);

            BasisFunctionEvaluation handler1 = (expected, i, degree, knot, t) =>
            {
                double[] values = new double[3];
                Nurbs.Algorithm.BasisFunction_EvaluateAllDerivatives_DegreeTwo(i, knot, t, ref values);
                Assert.AreEqual(expected, values[1], 1E-12);
            };

            BasisFunction_EvaluateFirstDerivative_ComplexKnot_DegreeTwo(handler1);

            BasisFunctionEvaluation handler2 = (expected, i, degree, knot, t) =>
            {
                double[] values = new double[3];
                Nurbs.Algorithm.BasisFunction_EvaluateAllDerivatives_DegreeTwo(i, knot, t, ref values);
                Assert.AreEqual(expected, values[2], 1E-12);
            };

            BasisFunction_EvaluateSecondDerivative_ComplexKnot_DegreeTwo(handler2);

        }

        [TestMethod]
        public void BasisFunction_EvaluateAllNonZero_DegreeTwo()
        {
            BasisFunctionEvaluation handler = (expected, i, degree, knot, t) =>
            {
                double[] values = new double[3];
                int[] indices = new int[3];
                Nurbs.Algorithm.BasisFunction_EvaluateAllNonZero_DegreeTwo(knot, t, ref values, ref indices);

                if(indices.Contains(i) && t < 1.0)
                {
                    int j = Array.IndexOf(indices, i);
                    Assert.AreEqual(expected, values[j], 1E-12);
                }
            };

            BasisFunction_Evaluate_ComplexKnot_DegreeTwo(handler);

        }


        [TestMethod]
        public void CurveSpan_Evaluate_Deboor2()
        {
            CurveEvaluation handler = (expected, degree, knotIX, knot, cvDataBlock, t) =>
            {
                Assert.AreEqual(expected, Nurbs.Algorithm.CurveSpan_Evaluate_Deboor2(degree, knotIX, ref knot, ref cvDataBlock, knotIX - degree, 1, t), 1E-12);
            };

            CurveSpan_Evaluate_ComplexKnot_DegreeTwo(handler);
        }



        public void BasisFunction_Evaluate_ComplexKnot_DegreeZero(BasisFunctionEvaluation testFunction)
        {
            int degree = 0;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N0,0(t)==0 for all t since [k0, k1) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 0, degree, knot, t);
            }

            //N1,0(t)==0 for all t since [k1, k2) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 1, degree, knot, t);
            }

            //N2,0(t)==1 for t in [0, 0.3), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 1 : 0;
                testFunction(expected, 2, degree, knot, t);
            }

            //N3,0(t)==1 for t in [0.3, 0.5), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.3 <= t && t < 0.5) ? 1 : 0;
                testFunction(expected, 3, degree, knot, t);
            }

            //N4,0(t)==0 for all t since [k4, k5) = [0.5, 0.5) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 4, degree, knot, t);
            }

            //N5,0(t)==1 for  t in [k5, k6) = [0.5, 0.6), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 1 : 0;
                testFunction(expected, 5, degree, knot, t);
            }

            //N6,0(t)==1 for  t in [k6, k7) = [0.6, 1.0), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.6 <= t && t < 1.0) ? 1 : 0;
                testFunction(expected, 6, degree, knot, t);
            }

            //N7,0(t)==0  for all t since [k7, k8) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 7, degree, knot, t);
            }

            //N8,0(t)==0  for all t since [k8, k9) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 8, degree, knot, t);
            }

        }

        public void BasisFunction_Evaluate_ComplexKnot_DegreeOne(BasisFunctionEvaluation testFunction)
        {
            int degree = 1;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N0,1(t)==0 for all t since [k0, k2) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 0, degree, knot, t);
            }

            //N1,1(t) == 1 - t/0.3 for t in [k2, k3) = [0, 0.3), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 1 - t / 0.3 : 0;
                testFunction(expected, 1, degree, knot, t);
            }

            //N2,1(t) == t/0.3 for t in [k2, k3) = [0, 0.3), N2(t) == 2.5 - 5t for t in [k3, k4) = [0.3, 0.5), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? t / 0.3
                    : (0.3 <= t && t < 0.5) ? 2.5 - 5 * t
                    : 0;
                testFunction(expected, 2, degree, knot, t);
            }

            //N3,1(t) == 5t - 1.5 for t in [k3, k4) = [0.3, 0.5), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.3 <= t && t < 0.5) ? 5 * t - 1.5 : 0;
                testFunction(expected, 3, degree, knot, t);
            }

            //N4,1(t) == 6 - 10 * t for t in [k5, k6) = [0.5, 0.6), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 6 - 10 * t : 0;
                testFunction(expected, 4, degree, knot, t);
            }

            //N5,1(t) == 10 * t - 5 for t in [k5, k6) = [0.5, 0.6), N5(t) == 2.5 * (1 - t) for t in [k6, k7) = [0.6, 1.0),zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 10 * t - 5
                    : (0.6 <= t && t < 1.0) ? 2.5 * (1 - t)
                    : 0;
                testFunction(expected, 5, degree, knot, t);
            }

            //N6,1(t) == 2.5 * t - 1.5 for t in [k6, k7) = [0.6, 1.0),zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0.6 <= t && t < 1.0) ? 2.5 * t - 1.5 : 0;
                testFunction(expected, 6, degree, knot, t);
            }

            //N7,1(t)==0 for all t since [k7, k9) = [0, 0) so doesn't exist
            foreach (double t in parameters)
            {
                double expected = 0;
                testFunction(expected, 7, degree, knot, t);
            }

        }

        public void BasisFunction_Evaluate_ComplexKnot_DegreeTwo(BasisFunctionEvaluation testFunction)
        {
            int degree = 2;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N0,2(t) == (1 - t/0.3)^2 in [0, 0.3)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? (1 - t / 0.3) * (1 - t / 0.3) : 0;
                testFunction(expected, 0, degree, knot, t);
            }

            //N1,2(t) == 1 - t/0.3 for t in [k2, k3) = [0, 0.3), zero otherwise
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? (60 * t - 160 * t * t) / 9
                    : (0.3 <= t && t < 0.5) ? 2.5 * (1 - 2 * t) * (1 - 2 * t)
                    : 0;
                testFunction(expected, 1, degree, knot, t);
            }

            //N2,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 20 * t * t / 3
                    : (0.3 <= t && t < 0.5) ? -3.75 + 25 * t - 35 * t * t
                    : 0;
                testFunction(expected, 2, degree, knot, t);
            }

            //N3,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.3 <= t && t < 0.5) ? (5 * t - 1.5) * (5 * t - 1.5)
                    : (0.5 <= t && t < 0.6) ? (6 - 10 * t) * (6 - 10 * t)
                    : 0;
                testFunction(expected, 3, degree, knot, t);
            }

            //N4,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 20 * (-2 + 7 * t - 6 * t * t)
                    : (0.6 <= t && t < 1.0) ? 5 - 10 * t + 5 * t * t
                    : 0;
                testFunction(expected, 4, degree, knot, t);
            }

            //N5,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 20 * t * t - 20 * t + 5
                    : (0.6 <= t && t < 1.0) ? -11.25 * t * t + 17.5 * t - 6.25
                    : 0;
                testFunction(expected, 5, degree, knot, t);
            }

            //N6,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.6 <= t && t < 1.0) ? 6.25 * t * t - 7.5 * t + 2.25
                    : 0;
                testFunction(expected, 6, degree, knot, t);
            }

        }

        public void BasisFunction_EvaluateFirstDerivative_ComplexKnot_DegreeTwo(BasisFunctionEvaluation testFunction)
        {
            int degree = 2;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N'0,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? -2.0 * (1 - t / 0.3) * (10.0 / 3.0) : 0;
                testFunction(expected, 0, degree, knot, t);
            }

            //N'1,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? (60.0  - 320.0 * t) / 9
                    : (0.3 <= t && t < 0.5) ? -10.0 * (1 - 2 * t)
                    : 0;
                testFunction(expected, 1, degree, knot, t);
            }

            //N'2,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 40.0 * t / 3
                    : (0.3 <= t && t < 0.5) ? 25.0 - 70.0 * t
                    : 0;
                testFunction(expected, 2, degree, knot, t);
            }

            //N'3,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.3 <= t && t < 0.5) ? 10.0 * (5 * t - 1.5)
                    : (0.5 <= t && t < 0.6) ? -20.0 * (6 - 10 * t)
                    : 0;
                testFunction(expected, 3, degree, knot, t);
            }

            //N'4,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 140.0 - 240.0 * t
                    : (0.6 <= t && t < 1.0) ? -10.0 + 10.0 * t
                    : 0;
                testFunction(expected, 4, degree, knot, t);
            }

            //N'5,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 40 * t - 20
                    : (0.6 <= t && t < 1.0) ? -22.5 * t + 17.5
                    : 0;
                testFunction(expected, 5, degree, knot, t);
            }

            //N'6,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.6 <= t && t < 1.0) ? 12.5 * t - 7.5
                    : 0;
                testFunction(expected, 6, degree, knot, t);
            }

        }

        public void BasisFunction_EvaluateSecondDerivative_ComplexKnot_DegreeTwo(BasisFunctionEvaluation testFunction)
        {
            int degree = 2;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            //N''0,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 200.0 / 9.0 : 0;
                testFunction(expected, 0, degree, knot, t);
            }

            //N''1,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? -320.0 / 9
                    : (0.3 <= t && t < 0.5) ? 20.0 
                    : 0;
                testFunction(expected, 1, degree, knot, t);
            }

            //N''2,2(t)
            foreach (double t in parameters)
            {
                double expected = (0 <= t && t < 0.3) ? 40.0 / 3
                    : (0.3 <= t && t < 0.5) ? -70.0
                    : 0;
                testFunction(expected, 2, degree, knot, t);
            }

            //N''3,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.3 <= t && t < 0.5) ? 50.0 
                    : (0.5 <= t && t < 0.6) ? 200.0
                    : 0;
                testFunction(expected, 3, degree, knot, t);
            }

            //N''4,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? -240.0
                    : (0.6 <= t && t < 1.0) ? 10.0
                    : 0;
                testFunction(expected, 4, degree, knot, t);
            }

            //N''5,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.5 <= t && t < 0.6) ? 40.0
                    : (0.6 <= t && t < 1.0) ? -22.5
                    : 0;
                testFunction(expected, 5, degree, knot, t);
            }

            //N''6,2(t)
            foreach (double t in parameters)
            {
                double expected = (0.6 <= t && t < 1.0) ? 12.5
                    : 0;
                testFunction(expected, 6, degree, knot, t);
            }

        }

        public void CurveSpan_Evaluate_ComplexKnot_DegreeTwo(CurveEvaluation testFunction)
        {
            int degree = 2;
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.5, 0.5, 0.6, 1, 1, 1 };

            double[] cvDataBlock = new double[] { 0.5, 1, 3, 6, 10, 15, 21 };

            double[] parameters = new double[] { 0, 0.1, 0.3, 0.4, 0.5, 0.55, 0.6, 0.7, 1.0 };

            foreach (double t in parameters)
            {
                double expected = 0;
                if (0 <= t && t < 0.3)
                {
                    double N0 = Nurbs.Algorithm.deboor_value(0, degree, knot, t);
                    double N1 = Nurbs.Algorithm.deboor_value(1, degree, knot, t);
                    double N2 = Nurbs.Algorithm.deboor_value(2, degree, knot, t);

                    expected = N0 * cvDataBlock[0] + N1 * cvDataBlock[1] + N2 * cvDataBlock[2];

                    testFunction(expected, degree, 2, knot, cvDataBlock, t);
                }
                else if (t < 0.5)
                {
                    double N1 = Nurbs.Algorithm.deboor_value(1, degree, knot, t);
                    double N2 = Nurbs.Algorithm.deboor_value(2, degree, knot, t);
                    double N3 = Nurbs.Algorithm.deboor_value(3, degree, knot, t);

                    expected = N1 * cvDataBlock[1] + N2 * cvDataBlock[2] + N3 * cvDataBlock[3];

                    testFunction(expected, degree, 3, knot, cvDataBlock, t);
                }
                else if (t < 0.6)
                {
                    double N3 = Nurbs.Algorithm.deboor_value(3, degree, knot, t);
                    double N4 = Nurbs.Algorithm.deboor_value(4, degree, knot, t);
                    double N5 = Nurbs.Algorithm.deboor_value(5, degree, knot, t);

                    expected = N3 * cvDataBlock[3] + N4 * cvDataBlock[4] + N5 * cvDataBlock[5];

                    testFunction(expected, degree, 5, knot, cvDataBlock, t);
                }
                else if (t <= 1.0)
                {
                    double N4 = Nurbs.Algorithm.deboor_value(4, degree, knot, t);
                    double N5 = Nurbs.Algorithm.deboor_value(5, degree, knot, t);
                    double N6 = Nurbs.Algorithm.deboor_value(6, degree, knot, t);

                    expected = N4 * cvDataBlock[4] + N5 * cvDataBlock[5] + N6 * cvDataBlock[6];

                    testFunction(expected, degree, 6, knot, cvDataBlock, t);
                }

            }

        }

    }
}
