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
        public void Knot_GetParameterGivenControlPolygonIntersect_SimpleKnot()
        {
            double[] knot = new double[] { 0, 0, 0, 0.3, 0.7, 1, 1, 1 };
            int order = 3;

            var inputs = new List<(int, double)>
            {
                (0, 1.00),
                (0, 0.75),
                (0, 0.50),
                (0, 0.25),
                (0, 0.00),
                (1, 1.00),
                (1, 0.75),
                (1, 0.50),
                (1, 0.25),
                (1, 0.00),
                (2, 1.00),
                (2, 0.75),
                (2, 0.50),
                (2, 0.25),
                (2, 0.00),
                (3, 1.00),
                (3, 0.75),
                (3, 0.50),
                (3, 0.25),
                (3, 0.00),
            };

            var parameters = inputs.Select(x => (x.Item1, x.Item2, Knot.GetParameterGivenControlPolygonIntersect(knot, x.Item1, x.Item2, order)));

          
        }

     
    }
}
