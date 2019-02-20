using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Intersections
{
    public class RayNurbsSurfaceIntersection
    {



    }

    public class IntersectionCalculatorRayNurbsSurface
    {

        public static RayNurbsSurfaceIntersection GetIntersect(
            double[] rayFrom,
            double[] rayTo,
            NurbsSurface surface)
        {

            var workingSurface = surface.DeepCopy();

            var newKnotsU = new List<double>();
            var newKnotsV = new List<double>();

            for (int i = 0; i < workingSurface.CvCount0 - 1; i++)
            {
                for (int j = 0; j < workingSurface.CvCount1 - 1; j++)
                {
                    var p1 = surface.GetCV(i, j);
                    var p2 = surface.GetCV(i + 1, j);
                    var p3 = surface.GetCV(i + 1, j + 1);
                    var p4 = surface.GetCV(i, j + 1);

                    var intersect1 = IntersectionCalculatorRayTriangle.GetIntersect(rayFrom, rayTo, p1, p2, p3);
                    var intersect2 = IntersectionCalculatorRayTriangle.GetIntersect(rayFrom, rayTo, p2, p3, p4);

                    if (intersect1.IsHit)
                    {
                        var u1 = Knot.GetParameterGivenControlPolygonIntersect(surface.knotArray0, i, intersect1.BarycentricCoords[0], workingSurface.Order0);
                        var v1 = Knot.GetParameterGivenControlPolygonIntersect(surface.knotArray1, j, intersect1.BarycentricCoords[1], workingSurface.Order1);

                        newKnotsU.Add(u1);
                        newKnotsV.Add(v1);
                        //use Greville abiscae to find approx surface parameters of intersection
                    }

                    if (intersect2.IsHit)
                    {
                        //use Greville abiscae to find approx surface parameters of intersection
                        var u2 = Knot.GetParameterGivenControlPolygonIntersect(surface.knotArray0, i, intersect1.BarycentricCoords[0], workingSurface.Order0);
                        var v2 = Knot.GetParameterGivenControlPolygonIntersect(surface.knotArray1, j, intersect1.BarycentricCoords[1], workingSurface.Order1);

                        newKnotsU.Add(u2);
                        newKnotsV.Add(v2);
                    }
                }
            }

            workingSurface = workingSurface.InsertKnot(0, newKnotsU.ToArray());
            workingSurface = workingSurface.InsertKnot(1, newKnotsV.ToArray());

            return null;
        }

    }
}
