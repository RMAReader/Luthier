using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test.Benchmark
{
    [TestClass]
    public class NurbsAlgorithmBenchmarkTests
    {

        [TestMethod]
        public void Curve_EvaluateGivenBasisFunctions()
        {
            
            int dimension = 3;
            int degree = 2;
            int cvCount = 50;
            int cvStride = 1;
            int dimensionStride = 50;
            double[] cvblock = new double[cvCount * dimension];

            int startIx = 0;
            double[] basisFunctions = new double[degree+1];

            double[] result = new double[dimension];


            int iterations = 1000000;
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {

                Luthier.Geometry.Nurbs.Algorithm.Curve_EvaluateGivenBasisFunctions(degree, dimension, startIx, basisFunctions, cvblock, cvStride, dimensionStride, ref result);
            }
            long t1 = sw.ElapsedMilliseconds;
        }

    }
}
