using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class NurbsSurfaceNearestPoint
    {
        
        public static void Find(NurbsSurface surface, int minU, int maxU, int minV, int maxV, double[] point)
        {
            double[] uv = new double[] 
            {
                0.5 * (minU + maxU),
                0.5 * (minV + maxV)
            };
            
            double[] c0 = surface.Evaluate(uv[0], uv[1]);
            double[] c1u = null;
            double[] c1v = null;
            double[] c2uu = null;
            double[] c2vv = null;
            double[] c2uv = null;

            double[] f = new double[2];
            double[,] J = new double[2,2];

            for (int itr = 0; itr < 10; itr++)
            {

                for (int i = 0; i < surface.Dimension; i++)
                {
                    double ray = point[i] - c0[i];

                    f[0] += ray * c1u[i];
                    f[1] += ray * c1v[i];

                    J[0, 0] += ray * c2uu[i] - c1u[i] * c1u[i];
                    J[0, 1] += ray * c2uv[i] - c1u[i] * c1v[i];

                    J[1, 0] += ray * c2uv[i] - c1u[i] * c1v[i];
                    J[1, 1] += ray * c2vv[i] - c1v[i] * c1v[i];
                }
                double[,] JInverse = new double[2, 2];

                double detJ = J[0, 0] * J[1, 1] - J[0, 1] * J[1, 0];
                JInverse[0, 0] = J[1, 1] / detJ;
                JInverse[0, 1] = -J[0, 1] / detJ;
                JInverse[1, 0] = -J[1, 0] / detJ;
                JInverse[1, 1] = J[0, 0] / detJ;

                uv[0] = uv[0] - JInverse[0, 0] * f[0] - JInverse[0, 1] * f[1];
                uv[1] = uv[1] - JInverse[1, 0] * f[0] - JInverse[1, 1] * f[1];
            }
        }

    }
}
