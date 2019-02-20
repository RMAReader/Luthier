using Luthier.Geometry.Nurbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test.Nurbs
{
    [TestCategory("CorrectnessTests")]
    [TestClass]
    public class KnotTests
    {

        [TestMethod]
        public void Knot_GetParameterGivenControlPolygonIntersect()
        {
            double[] knot = new double[] { 0, 0, 0, 1, 1, 1 };
            int order = 3;

            Assert.AreEqual(0.0, Knot.GetParameterGivenControlPolygonIntersect(knot, 0, 1.0, order), 0.0000001);
            Assert.AreEqual(0.166666666666667, Knot.GetParameterGivenControlPolygonIntersect(knot, 0, 0.5, order), 0.0000001);
            Assert.AreEqual(0.5, Knot.GetParameterGivenControlPolygonIntersect(knot, 0, 0.0, order), 0.0000001);

            Assert.AreEqual(0.5, Knot.GetParameterGivenControlPolygonIntersect(knot, 1, 1.0, order), 0.0000001);
            Assert.AreEqual(0.833333333333333, Knot.GetParameterGivenControlPolygonIntersect(knot, 0, 0.5, order), 0.0000001);
            Assert.AreEqual(1.0, Knot.GetParameterGivenControlPolygonIntersect(knot, 0, 0.0, order), 0.0000001);

        }
    }
}
