using Luthier.Geometry.Nurbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test.Nurbs
{
    [TestClass]
    public class NurbsSurfaceTests
    {

        [TestCategory("CorrectnessTests")]
        [TestMethod]
        public void NurbsSurface_NearestPoint()
        {
            //define a flat square surface
            var surface = new NurbsSurface(dimension: 3, bIsRational: false, order0: 3, order1: 3, cv_count0: 3, cv_count1: 3);
            surface.knotArray0 = new double[] { 0, 1, 2, 3, 4, 5 };
            surface.knotArray1 = new double[] { 0, 1, 2, 3, 4, 5 };
            surface.SetCV(0, 0, new double[] { 0, 0, -20 });
            surface.SetCV(0, 1, new double[] { 0, 10, -10 });
            surface.SetCV(0, 2, new double[] { 0, 20, -20 });
            surface.SetCV(1, 0, new double[] { 10, 0, -10 });
            surface.SetCV(1, 1, new double[] { 10, 10, 0 });
            surface.SetCV(1, 2, new double[] { 10, 20, -10 });
            surface.SetCV(2, 0, new double[] { 20, 0, -20 });
            surface.SetCV(2, 1, new double[] { 20, 10, -10 });
            surface.SetCV(2, 2, new double[] { 20, 20, -20 });

            double[] point1 = new double[] { 11, 10, 5 };
            double u1 = 0;
            double v1 = 0;
            NurbsSurfaceNearestPoint.Find(surface, 2, 3, 2, 3, point1, ref u1, ref v1);

            double[] point2 = new double[] { 15, 15, 5 };
            double u2 = 0;
            double v2 = 0;
            NurbsSurfaceNearestPoint.Find(surface, 2, 3, 2, 3, point2, ref u2, ref v2);

            Assert.AreEqual(2.5333086967233309, u1);
            Assert.AreEqual(2.5, v1);

            Assert.AreEqual(2.6610926775114225, u2);
            Assert.AreEqual(2.6610926775114225, v2);


        }


    }
}
