using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class NurbsSurfaceNearestPoint
    {
        
        public static void Find(NurbsSurface surface, double minU, double maxU, double minV, double maxV, double[] point, ref double u, ref double v)
        {

            u = 0.5 * (minU + maxU);
            v = 0.5 * (minV + maxV);
           

            //offsets
            int du = 3;
            int dv = 6;
            int duu = 9;
            int duv = 12;
            int dvv = 15;

            double error = 0;
            double detJ = 0;
            double[] f = new double[2];
            double[,] J = new double[2, 2];
            double[,] JInverse = new double[2, 2];
            
            double[] values = surface.EvaluateAllDerivatives(u, v);

            UpdateFunctionAndJacobian(surface, point, du, dv, duu, duv, dvv, values, ref f, ref J, ref error);

            int iterations = 0;
            while (error > 1E-10 && iterations < 1000)
            {

                detJ = J[0, 0] * J[1, 1] - J[0, 1] * J[1, 0];

                JInverse[0, 0] = J[1, 1] / detJ;
                JInverse[0, 1] = -J[0, 1] / detJ;
                JInverse[1, 0] = -J[1, 0] / detJ;
                JInverse[1, 1] = J[0, 0] / detJ;

                u = u - JInverse[0, 0] * f[0] - JInverse[0, 1] * f[1];
                v = v - JInverse[1, 0] * f[0] - JInverse[1, 1] * f[1];

                values = surface.EvaluateAllDerivatives(u, v);

                UpdateFunctionAndJacobian(surface, point, du, dv, duu, duv, dvv, values, ref f, ref J, ref error);

                iterations++;
            }
        }


        private static void UpdateFunctionAndJacobian(
            NurbsSurface surface, 
            double[] point, 
            int du,
            int dv,
            int duu,
            int duv,
            int dvv,
            double[] values, 
            ref double[] f, 
            ref double[,] J, 
            ref double error)
        {
            f[0] = 0;
            f[1] = 0;

            J[0, 0] = 0;
            J[0, 1] = 0;

            J[1, 0] = 0;
            J[1, 1] = 0;

            for (int i = 0; i < surface.Dimension; i++)
            {
                double ray = point[i] - values[i];

                f[0] += ray * values[du + i];
                f[1] += ray * values[dv + i];

                J[0, 0] += ray * values[duu + i] - values[du + i] * values[du + i];
                J[0, 1] += ray * values[duv + i] - values[du + i] * values[dv + i];

                J[1, 0] += ray * values[duv + i] - values[du + i] * values[dv + i];
                J[1, 1] += ray * values[dvv + i] - values[dv + i] * values[dv + i];
            }

            error = f[0] * f[0] + f[1] * f[1];
        }

    }
}
