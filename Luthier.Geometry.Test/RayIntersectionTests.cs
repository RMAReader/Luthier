using Luthier.Geometry.Intersections;
using Luthier.Geometry.Nurbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class RayIntersectionTests
    {


        [TestMethod]
        public void RayTriangleIntersection()
        {
            double[] p1 = new double[] { 0, 0, 0 };
            double[] p2 = new double[] { 2, 0, 0 };
            double[] p3 = new double[] { 0, 2, 0 };

            //interior
            var intersection1 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 1, 0.5, 1 }, new double[] { 1, 0.5, -1 }, p1, p2, p3);
            
            //exterior
            var intersection8 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 2, 2, 1 }, new double[] { 2, 2, -1 }, p1, p2, p3);

            //corners
            var intersection2 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 0, 0, 1 }, new double[] { 0, 0, -1 }, p1, p2, p3);
            var intersection3 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 2, 0, 1 }, new double[] { 2, 0, -1 }, p1, p2, p3);
            var intersection4 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 0, 2, 1 }, new double[] { 0, 2, -1 }, p1, p2, p3);

            //edges
            var intersection5 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 1, 1, 1 }, new double[] { 1, 1, -1 }, p1, p2, p3);
            var intersection6 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 0, 1, 1 }, new double[] { 0, 1, -1 }, p1, p2, p3);
            var intersection7 = IntersectionCalculatorRayTriangle.GetIntersect(new double[] { 1, 0, 1 }, new double[] { 1, 0, -1 }, p1, p2, p3);
        }



        [TestMethod]
        public void RayNurbsSurfaceIntersection()
        {
            var surface = new NurbsSurface(dimension: 3, bIsRational: false, order0: 3, order1: 3, cv_count0: 3, cv_count1: 3)
            {
                knotArray0 = Knot.CreateUniformClosed(2, 6).data.ToArray(),
                knotArray1 = Knot.CreateUniformClosed(2, 6).data.ToArray(),
            };
            for(int i=0; i<3; i++)
            {
                for(int j=0; j<3; j++)
                {
                    surface.SetCV(i, j, new double[] { i, j, i*i+j*j });
                }
            }
            
            //interior
            var intersection1 = IntersectionCalculatorRayNurbsSurface.GetIntersect(new double[] { 1, 0.6, 1 }, new double[] { 1, 0.4, -1 }, surface);
                       
        }
    }
}
