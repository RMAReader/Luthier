using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test
{
    public static class AssertExtensions
    {
        public static void AreEqual(double[] expected, double[]actual, double tolerance)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for(int i=0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], tolerance);
            }
        }
    }
}
