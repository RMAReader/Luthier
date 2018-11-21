using Luthier.Geometry.Nurbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test.Benchmark
{
    [TestClass]
    public class NurbsCurveBenchmarkTests
    {

        [TestMethod]
        public void NurbsCurve_Evaluate()
        {
            var curve = new NurbsCurve(dimension: 3, isRational: false, order: 3, cvCount: 100);

            int iterations = 1000000;
            double t = 0.5 * (curve.Domain.Min + curve.Domain.Max);
            double[] point = new double[curve._dimension];

            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                curve.Evaluate(t, point);
            }
            long t1 = sw.ElapsedMilliseconds;
        }
    }
}
