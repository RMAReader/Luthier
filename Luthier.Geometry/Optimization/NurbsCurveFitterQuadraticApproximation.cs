using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Luthier.Geometry.Optimization
{
    /// <summary>
    /// Class which implements the algorithm described here:
    /// https://www.microsoft.com/en-us/research/wp-content/uploads/2016/12/Fitting-B-spline-Curves-to-Point-Clouds-by-Curvature-Based-Squared-Distance-Minimization.pdf
    /// </summary>
    public class NurbsCurveFitterQuadraticApproximation : NurbsCurveFitterBase
    {
        private int footPointCount = 1024 * 16;


        public override NurbsCurve Fit(NurbsCurve initialGuess, PointCloud cloud)
        {
            NurbsCurve currentCurve = initialGuess.DeepCopy();

            var nearestPoints = currentCurve.NearestSquaredDistance(cloud, footPointCount);

            while (!IsConverged(nearestPoints))
            {
                BuildLinearSystem(out Matrix<double> M, out Vector<double> v);

                var d = M.Solve(v);

                currentCurve = UpdateCurve(currentCurve, d);

                nearestPoints = currentCurve.NearestSquaredDistance(cloud, footPointCount);
            }

            return null;
        }


        private void BuildLinearSystem(out Matrix<double> M, out Vector<double> v)
        {
            M = null;
            v = null;
        }

        private NurbsCurve UpdateCurve(NurbsCurve currentCurve, Vector<double> v)
        {
            return null;
        }

        private bool IsConverged(NurbsCurveNearestPointResult nearestPoints)
        {
            return false;
        }
    }
}
