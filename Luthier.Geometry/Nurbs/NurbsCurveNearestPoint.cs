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
        public double NearestSquaredDistanceNewtonRaphson(double parameterMinimum, double parameterMaximum, ref double[] c0, ref double[] c1, ref double[] c2)
        {
            return FindRoot(parameterMinimum, parameterMaximum, ref c0, ref c1, ref c2);
            //return NewtonRaphson.FindRoot(f, df, parameterMinimum, parameterMaximum, accuracy: 1e-8, maxIterations: 10000);
            //return RobustNewtonRaphson.FindRoot(f, df, parameterMinimum, parameterMaximum, accuracy: 1e-10, maxIterations: 100, subdivision: 20);
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
                df += (_point[i] - c0[i]) * c2[i] - c1[i] * c1[i];
            }

            return df;
        }

        private double FindRoot(double parameterMinimum, double parameterMaximum, ref double[] c0, ref double[] c1, ref double[] c2)
        {
            double t = 0.5 * (parameterMinimum + parameterMaximum);

            double f = 0;
            double df = 0;

            c0 = _curve.Evaluate(t);
            c1 = _curve.EvaluateDerivative(1, t);
            c2 = _curve.EvaluateDerivative(2, t);

            for (int i = 0; i < c0.Length; i++)
            {
                f += (_point[i] - c0[i]) * c1[i];
                df += (_point[i] - c0[i]) * c2[i] - c1[i] * c1[i];
            }

            int iterations = 0;

            while (Math.Abs(f) > 1e-10 && iterations < 1000)
            {
                t = t - f / df;

                c0 = _curve.Evaluate(t);
                c1 = _curve.EvaluateDerivative(1, t);
                c2 = _curve.EvaluateDerivative(2, t);

                f = 0;
                df = 0;
                for (int i = 0; i < c0.Length; i++)
                {
                    f += (_point[i] - c0[i]) * c1[i];
                    df += (_point[i] - c0[i]) * c2[i] - c1[i] * c1[i];
                }
               
                iterations++;
            }
            return t;

        }


        public double FindRoot(double parameterMinimum, double parameterMaximum, ref double[] values)
        {
            double t = 0.5 * (parameterMinimum + parameterMaximum);

            double f = 0;
            double df = 0;

            _curve.EvaluateAllDerivatives(t, values);

            int firstOffset = _curve._dimension;
            int secondOffset = _curve._dimension;

            for (int i = 0; i < _curve._dimension; i++)
            {
                f += (_point[i] - values[i]) * values[i + firstOffset];
                df += (_point[i] - values[i]) * values[i + secondOffset] - values[i + firstOffset] * values[i + firstOffset];
            }

            int iterations = 0;

            while (Math.Abs(f) > 1e-10 && iterations < 1000)
            {
                t = t - f / df;

                _curve.EvaluateAllDerivatives(t, values);

                f = 0;
                df = 0;
                for (int i = 0; i < _curve._dimension; i++)
                {
                    f += (_point[i] - values[i]) * values[i + firstOffset];
                    df += (_point[i] - values[i]) * values[i + secondOffset] - values[i + firstOffset] * values[i + firstOffset];
                }

                iterations++;
            }
            return t;

        }

    }
}
