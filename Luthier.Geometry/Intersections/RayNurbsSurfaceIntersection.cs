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
        public bool IsHit;
        public double RayParameter;
        public double[] SurfaceParameters;
        public double[] Coordinates;
    }

    public class IntersectionCalculatorRayNurbsSurface
    {
        public static double Tolerance = 1E-3;


        public static RayNurbsSurfaceIntersection GetIntersect(
            double[] rayFrom,
            double[] rayTo,
            NurbsSurface surface)
        {
          
            //TODO: need stopping criteria of distance from current point of intersection on surface to line

            var workingSurface = surface.DeepCopy();

            var newKnotsU = new List<double>();
            var newKnotsV = new List<double>();

            bool solutionFound = false;

            for (int itr = 0; itr < 10 && !solutionFound; itr++)
            {
                newKnotsU.Clear();
                newKnotsV.Clear();

                for (int i = 0; i < workingSurface.CvCount0 - 1; i++)
                {
                    for (int j = 0; j < workingSurface.CvCount1 - 1; j++)
                    {
                        var p1 = workingSurface.GetCV(i, j);
                        var p2 = workingSurface.GetCV(i + 1, j);
                        var p3 = workingSurface.GetCV(i + 1, j + 1);
                        var p4 = workingSurface.GetCV(i, j + 1);

                        var intersect1 = IntersectionCalculatorRayTriangle.GetIntersect(rayFrom, rayTo, p1, p2, p4);
                        var intersect2 = IntersectionCalculatorRayTriangle.GetIntersect(rayFrom, rayTo, p3, p4, p2);

                        if (intersect1.IsHit)
                        {
                            var u = Knot.GetParameterGivenControlPolygonIntersect(workingSurface.knotArray0, i, 1 - intersect1.BarycentricCoords[0], workingSurface.Order0);
                            var v = Knot.GetParameterGivenControlPolygonIntersect(workingSurface.knotArray1, j, 1 - intersect1.BarycentricCoords[1], workingSurface.Order1);

                            var surfaceP1 = workingSurface.Evaluate(u, v);

                            var error = Algorithm.DistancePointToLine(surfaceP1, rayFrom, rayTo);
                            if(error < Tolerance)
                            {
                                solutionFound = true;
                                return new RayNurbsSurfaceIntersection
                                {
                                    IsHit = true,
                                    RayParameter = intersect1.RayParameter,
                                    SurfaceParameters = new double[] { u, v },
                                    Coordinates = surfaceP1
                                };
                            }
                            else
                            {
                                newKnotsU.Add(u);
                                newKnotsV.Add(v);
                            }
                            //use Greville abiscae to find approx surface parameters of intersection
                        }

                        if (intersect2.IsHit)
                        {
                            //use Greville abiscae to find approx surface parameters of intersection
                            var u = Knot.GetParameterGivenControlPolygonIntersect(workingSurface.knotArray0, i, intersect2.BarycentricCoords[0], workingSurface.Order0);
                            var v = Knot.GetParameterGivenControlPolygonIntersect(workingSurface.knotArray1, j, intersect2.BarycentricCoords[1], workingSurface.Order1);

                            var surfaceP1 = workingSurface.Evaluate(u, v);

                            var error = Algorithm.DistancePointToLine(surfaceP1, rayFrom, rayTo);
                            if (error < Tolerance)
                            {
                                solutionFound = true;
                                return new RayNurbsSurfaceIntersection
                                {
                                    IsHit = true,
                                    RayParameter = intersect1.RayParameter,
                                    SurfaceParameters = new double[] { u, v },
                                    Coordinates = surfaceP1
                                };
                            }
                            else
                            {
                                newKnotsU.Add(u);
                                newKnotsV.Add(v);
                            }
                        }
                    }
                }

                if (newKnotsU.Count > 0 && newKnotsV.Count > 0)
                {
                    workingSurface = workingSurface.InsertKnot(0, newKnotsU.Distinct().ToArray());
                    workingSurface = workingSurface.InsertKnot(1, newKnotsV.Distinct().ToArray());
                }
                else
                {
                    return new RayNurbsSurfaceIntersection { IsHit = false, };
                }
            }

            return new RayNurbsSurfaceIntersection { IsHit = false, };

        }

    }
}
