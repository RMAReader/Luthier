using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Intersections
{

    public class RayTriangleIntersection
    {
        public double[] IntersectWorldCoords;
        public double[] BarycentricCoords;
        public double RayParameter;
        public bool IsHit;
    }

    public class IntersectionCalculatorRayTriangle 
    {

        public static RayTriangleIntersection GetIntersect(
            double[] rayFrom, 
            double[] rayTo, 
            double[] triangleP1,
            double[] triangleP2, 
            double[] triangleP3)
        { 
            Matrix4x4 m = new Matrix4x4((float)(triangleP2[0]- triangleP1[0]), (float)(triangleP2[1]- triangleP1[1]), (float)(triangleP2[2] - triangleP1[2]), 0,
                                        (float)(triangleP3[0] - triangleP1[0]), (float)(triangleP3[1] - triangleP1[1]), (float)(triangleP3[2] - triangleP1[2]), 0,
                                        (float)(rayFrom[0] - rayTo[0]), (float)(rayFrom[1] - rayTo[1]), (float)(rayFrom[2] - rayTo[2]), 0,
                                        0, 0, 0, 1f);

            if (Matrix4x4.Invert(m, out Matrix4x4 mInverse))
            {
                Vector3 f = new Vector3((float)rayFrom[0], (float)rayFrom[1], (float)rayFrom[2]);
                Vector3 o = new Vector3((float)triangleP1[0], (float)triangleP1[1], (float)triangleP1[2]);

                var coeffs = Vector3.Transform(f - o, mInverse);
                var intersectWorld = new double[]
                {
                    (1 - coeffs.Z) * rayFrom[0] + coeffs.Z * rayTo[0],
                    (1 - coeffs.Z) * rayFrom[1] + coeffs.Z * rayTo[1],
                    (1 - coeffs.Z) * rayFrom[2] + coeffs.Z * rayTo[2]
                };

                return new RayTriangleIntersection
                {
                    IntersectWorldCoords = intersectWorld,
                    RayParameter = coeffs.Z,
                    BarycentricCoords = new double[] { coeffs.X, coeffs.Y },
                    IsHit = 0 <= coeffs.X && 0 <= coeffs.Y && coeffs.X + coeffs.Y <= 1
                };
            }
            return new RayTriangleIntersection
            {
                IsHit = false
            }; 
        }
    }
}
