using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class NurbsCurveBulkEvaluator
    {
        private readonly NurbsCurve _curve;

        //private Matrix<double> _basisFunctionMatrix;

        private double[] _evaluationPoints;
        public double[] EvaluationPoints
        {
            get => _evaluationPoints;
            set
            {
                _evaluationPoints = value;
                CalculateBasisFunctions();
            }
        }

        private double[] _basisFunctions;
        private int[] _knotIX;

        public NurbsCurveBulkEvaluator(NurbsCurve curve)
        {
            _curve = curve;
        }

        public NurbsCurveBulkEvaluator(NurbsCurve curve, double[] evaluationPoints)
        {
            _curve = curve;
            EvaluationPoints = evaluationPoints;
        }


        private void CalculateBasisFunctions()
        {
            _basisFunctions = new double[_evaluationPoints.Length * _curve._order];
            _knotIX = new int[_evaluationPoints.Length];

            int basisFunctionIX = 0;
            for (int t = 0; t < _evaluationPoints.Length; t++)
            {
                _knotIX[t] = Algorithm.Find_Knot_Span(_curve._order - 1, _curve.knot, _evaluationPoints[t]);
                for (int i = _knotIX[t] - _curve._order + 1; i <= _knotIX[t]; i++)
                {
                    _basisFunctions[basisFunctionIX] = Algorithm.BasisFunction_Evaluate(i, _curve._order - 1, _curve.knot, _evaluationPoints[t]);
                    basisFunctionIX++;
                }
            }

            //var values = new List<Tuple<int, int, double>>(_evaluationPoints.Length * _curve._order);
            //for (int t = 0; t < _evaluationPoints.Length; t++)
            //{
            //    int knotIx = Algorithm.Find_Knot_Span(_curve._order - 1, _curve.knot, _evaluationPoints[t]);
            //    for (int i = knotIx - _curve._order + 1; i <= knotIx; i++)
            //    {
            //        double basisFunction = Algorithm.BasisFunction_Evaluate(i, _curve._order - 1, _curve.knot, _evaluationPoints[t]);

            //        values.Add(new Tuple<int, int, double>(t, i, basisFunction));
            //    }
            //}

            //_basisFunctionMatrix = Matrix<double>.Build.SparseOfIndexed(rows: _curve._order, columns: _evaluationPoints.Length, enumerable: values);
        }



        public double[] Evaluate()
        {
            double[] result = new double[_evaluationPoints.Length * _curve._dimension];

            for (int t = 0; t < _evaluationPoints.Length; t++)
            {
                for(int d = 0; d < _curve._dimension; d++)
                {
                    for (int i = 0; i < _curve._order; i++)
                    {
                        int basisFuncIx = t * _curve._order + i;
                        int cvIx = _curve.ControlPoints.GetDataIndex(d, _knotIX[t] - _curve._order + 1 + i);
                        int resultIx = t * _curve._dimension + d;

                        result[resultIx] += _basisFunctions[basisFuncIx] * _curve.ControlPoints.Data[cvIx];
                    }
                }
            }
            return result;
        }

        public double[] EvaluateFirstDerivative()
        {
            return null;
        }

    }
}
