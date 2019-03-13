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
        private PointCloud _cloud;
        private NurbsCurve _initialGuess;
        private NurbsCurve _curve;

        private int footPointCount = 32;


        public override NurbsCurve Fit(NurbsCurve initialGuess, PointCloud cloud)
        {
            _initialGuess = initialGuess;
            _cloud = cloud;
            _curve = initialGuess.DeepCopy();

            var squaredDistanceFunction = new NurbsCurveSquaredDistance(_initialGuess, _cloud, EndConstraint.VariablePositionVariableTangent);

            var values = new List<double>();


            for (int i=0; i< 10; i++)
            {
                values.Add(squaredDistanceFunction.Value(_curve.ControlPoints.Data));

                var H = squaredDistanceFunction.Hessian(_curve.ControlPoints.Data);
                var D = squaredDistanceFunction.Gradient(_curve.ControlPoints.Data);

                var M = Matrix<double>.Build.Dense(_curve.ControlPoints.Data.Length, _curve.ControlPoints.Data.Length, H);
                var V = Vector<double>.Build.Dense(D);

                var d = M.Solve(-V);

                for(int j=0; j<d.Count;j++ )
                {
                    _curve.ControlPoints.Data[i] -= d[i];
                }
            }

            
            return null;
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
