using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class NurbsCurveNearestPoint
    {
        private readonly NurbsCurve _curve;
        private readonly double[] _point;
        private double _delta = 1E-8;
        private int _curveEvaluationCount;

        public int CurveEvaluationCount => _curveEvaluationCount;

        public NurbsCurveNearestPoint(NurbsCurve curve, double[] point)
        {
            _curve = curve;
            _point = point;
            _curveEvaluationCount = 0;
        }

        //minimum is a t where (point - curve(t)).Dot(curve'(t)) == 0
        public double NearestSquaredDistanceNewtonRaphson(double parameterMinimum, double parameterMaximum)
        {
            //return NewtonRaphson.FindRoot(f, df, parameterMinimum, parameterMaximum, accuracy: 1e-8, maxIterations: 10000);
            return RobustNewtonRaphson.FindRoot(f, df, parameterMinimum, parameterMaximum, accuracy: 1e-10, maxIterations: 100, subdivision: 20);
        }

        //minimum is a t where (point - curve(t)).Dot(curve'(t)) == 0
        private double f(double t)
        {
            _curveEvaluationCount += 2;

            var c0 = _curve.Evaluate(t);
            var c1 = _curve.EvaluateDerivative(1, t);

            double f = 0;
            for (int i = 0; i < c0.Length; i++)
            {
                f += (_point[i] - c0[i]) * c1[i];
            }

            return f;
        }

        private double df(double t)
        {
            _curveEvaluationCount += 3;

            var c0 = _curve.Evaluate(t);
            var c1 = _curve.EvaluateDerivative(1, t);
            var c2 = _curve.EvaluateDerivative(2, t);

            double df = 0;
            for (int i = 0; i < c0.Length; i++)
            {
                df += (_point[i] - c1[i]) * c1[i] + (_point[i] - c0[i]) * c2[i];
            }

            return df;
        }

    }
}
