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
    public class CurvatureTests
    {

        [TestMethod]
        public void CentreOfCurvature_TwoDimensions()
        {
            double[] firstDerivative = new double[] { 0.5, 0.5 };
            double[] secondDerivative = new double[] { 1, -1 };

            double[] expectedCentreOfCurvature = new double[]
            {
                1 / Math.Sqrt(2) / 2.8284271247461894,
                -1 / Math.Sqrt(2) / 2.8284271247461894,
            };
            double[] actualCentreOfCurvature = new double[2];

            Curvature.CentreOfCurvature_TwoDimensions(firstDerivative, secondDerivative, ref actualCentreOfCurvature);

            AssertExtensions.AreEqual(expectedCentreOfCurvature, actualCentreOfCurvature, 1E-9);

        }

        [TestMethod]
        public void CentreOfCurvature_ThreeDimensions()
        {
            double[] firstDerivative = new double[] { 0.5, 0.35355339059327373, 0.35355339059327373 };
            double[] secondDerivative = new double[] { 1, -0.70710678118654746, -0.70710678118654746 };

            double[] expectedCentreOfCurvature = new double[] 
            {
                0.70710678118654746 / 2.8284271247461894,
                -0.5 / 2.8284271247461894,
                -0.5 / 2.8284271247461894
            };
            double[] actualCentreOfCurvature = new double[3];
            
            Curvature.CentreOfCurvature_ThreeDimensions(firstDerivative, secondDerivative, ref actualCentreOfCurvature);

            AssertExtensions.AreEqual(expectedCentreOfCurvature, actualCentreOfCurvature, 1E-9);
            
        }


        [TestMethod]
        public void Curvature_ThreeDimensions_InXYPlane()
        {
            double[] firstDerivative = new double[] { 0.5, 0.5, 0 };
            double[] secondDerivative = new double[] { 1, -1, 0 };

            double[] expectedTangent = new double[] { 1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0 };
            double[] expectedNormal = new double[] { 1 / Math.Sqrt(2), -1 / Math.Sqrt(2), 0 };
            double[] expectedBinormal = new double[] { 0, 0, -1 };
            double expectedCurvature = 2.8284271247461894;

            double[] actualTangent = new double[3];
            double[] actualNormal = new double[3];
            double[] actualBinormal = new double[3];
            double actualCurvature = 0;

            Curvature.Curvature_ThreeDimensions(firstDerivative, secondDerivative, ref actualTangent, ref actualNormal, ref actualBinormal, ref actualCurvature);

            Assert.AreEqual(expectedCurvature, actualCurvature, 1E-9);
            AssertExtensions.AreEqual(expectedTangent, actualTangent, 1E-9);
            AssertExtensions.AreEqual(expectedNormal, actualNormal, 1E-9);
            AssertExtensions.AreEqual(expectedBinormal, actualBinormal, 1E-9);
        }


        [TestMethod]
        public void Curvature_ThreeDimensions()
        {
            double[] firstDerivative = new double[] { 0.5, 0.35355339059327373, 0.35355339059327373 };
            double[] secondDerivative = new double[] { 1, -0.70710678118654746, -0.70710678118654746 };

            double[] expectedTangent = new double[] { 0.70710678118654746, 0.5, 0.5 };
            double[] expectedNormal = new double[] { 0.70710678118654746, -0.5, -0.5 };
            double[] expectedBinormal = new double[] { 0, 0.70710678118654757, -0.70710678118654757 };
            double expectedCurvature = 2.8284271247461894;

            double[] actualTangent = new double[] { 1, 1, 1 };
            double[] actualNormal = new double[] { 1, 1, 1 };
            double[] actualBinormal = new double[] { 1, 1, 1 };
            double actualCurvature = 0;

            Curvature.Curvature_ThreeDimensions(firstDerivative, secondDerivative, ref actualTangent, ref actualNormal, ref actualBinormal, ref actualCurvature);

            Assert.AreEqual(expectedCurvature, actualCurvature, 1E-9);
            AssertExtensions.AreEqual(expectedTangent, actualTangent, 1E-9);
            AssertExtensions.AreEqual(expectedNormal, actualNormal, 1E-9);
            AssertExtensions.AreEqual(expectedBinormal, actualBinormal, 1E-9);
        }

    }
}
