using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opennurbs_CLI.Tests
{
    [TestClass]
    public class NurbsSurfaceTests
    {
        [TestMethod]
        public void testMethod1()
        {
            var surface = new opennurbs_CLI.NurbsSurface(2, 0, 3, 3, 3, 3);

            surface.SetCV(0, 0, 3, new double[] { 0, 0 });
            surface.SetCV(1, 0, 3, new double[] { 1, 0 });
            surface.SetCV(2, 0, 3, new double[] { 2, 0 });
            surface.SetCV(0, 1, 3, new double[] { 0, 10 });
            surface.SetCV(1, 1, 3, new double[] { 1, 10 });
            surface.SetCV(2, 1, 3, new double[] { 2, 10 });
            surface.SetCV(0, 2, 3, new double[] { 0, 20 });
            surface.SetCV(1, 2, 3, new double[] { 1, 20 });
            surface.SetCV(2, 2, 3, new double[] { 2, 20 });

            for(int i=0; i< surface.KnotCount(0); i++)
            {
                surface.SetKnot(0, i, i);
            }
            for (int i = 0; i < surface.KnotCount(1); i++)
            {
                surface.SetKnot(1, i, i);
            }

            var domainU = surface.Domain(0);
            var domainV = surface.Domain(1);

            var p11 = surface.Evaluate(1.0, 1.0, 1);
            var p12 = surface.Evaluate(1.2, 1.2, 1);
            var p13 = surface.Evaluate(1.5, 1.5, 1);
            var p14 = surface.Evaluate(1.9, 1.9, 1);
            var p15 = surface.Evaluate(2.0, 2.0, 1);
            var degreeU = surface.Degree(0);
            var degreeV = surface.Degree(1);
            var nCV1 = surface.CVCount;

            surface.InsertKnot(0, 1.5, 1);
            surface.InsertKnot(1, 1.5, 1);

            var p21 = surface.Evaluate(1.0,1.0, 1);
            var p22 = surface.Evaluate(1.2,1.2, 1);
            var p23 = surface.Evaluate(1.5,1.5, 1);
            var p24 = surface.Evaluate(1.9,1.9, 1);
            var p25 = surface.Evaluate(2.0,2.0, 1);
            var degreeU2 = surface.Degree(0);
            var degreeV2 = surface.Degree(1);
            var nCV2 = surface.CVCount;

            var res = surface.IncreaseDegree(0, 10);
            var res2 = surface.IncreaseDegree(1, 10);

            var p31 = surface.Evaluate(1.0, 1.0, 1);
            var p32 = surface.Evaluate(1.2, 1.2, 1);
            var p33 = surface.Evaluate(1.5, 1.5, 1);
            var p34 = surface.Evaluate(1.9, 1.9, 1);
            var p35 = surface.Evaluate(2.0, 2.0, 1);
            var degreeU3 = surface.Degree(0);
            var degreeV3 = surface.Degree(1);
            var nCV3 = surface.CVCount;

            Assert.AreEqual(p31[0], p11[0], 0.0000001);
            Assert.AreEqual(p31[1], p11[1], 0.0000001);
        }


    }
}
