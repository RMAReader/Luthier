using Luthier.Geometry.Nurbs;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{

    public enum EndConstraint
    {
        VariablePositionVariableTangent,
        FixedPositionVariableTangent,
        FixedPositionFixedTangent
    }

    public class NurbsCurveSquaredDistance
    {
        private NurbsCurve _curve;
        private PointCloud _cloud;
        private EndConstraint _constraint;

        private bool _requireGradient;
        private bool _requireHessian;

        public double CurrentValue { get; private set; }
        public double[] CurrentGradient { get; private set; }
        public double[] CurrentHessian { get; private set; }
        public double[] Parameters { get; private set; }

        public int EvaluationCount { get; private set; }

        public NurbsCurveSquaredDistance(NurbsCurve curve, PointCloud cloud, EndConstraint constraint)
        {
            _curve = curve;
            _cloud = cloud;
            _constraint = constraint;

            _requireGradient = false;
            _requireHessian = false;

            EvaluationCount = 0;
            CurrentValue = double.MaxValue;
            CurrentGradient = new double[_curve.cvDataBlock.Length];
            CurrentHessian = new double[_curve.cvDataBlock.Length * _curve.cvDataBlock.Length];
            Parameters = new double[_curve.cvDataBlock.Length];
        }

        public double Value(double[] point)
        {
            if (HasChanged(Parameters, point))
            {
                if (_constraint == EndConstraint.FixedPositionFixedTangent)
                {
                    UpdateCurveFixedPositionFixedTangent(point);
                }
                else if (_constraint == EndConstraint.FixedPositionVariableTangent)
                {
                    UpdateCurveFixedPositionVariableTangent(point);
                }
                else if (_constraint == EndConstraint.VariablePositionVariableTangent)
                {
                    UpdateCurveVariablePositionVariableTangent(point);
                }

                EvaluateAtParameters(point);
            }
            return CurrentValue;

        }

        public double[] Gradient(double[] point)
        {
            if(HasChanged(Parameters, point) || _requireGradient == false)
            {
                _requireGradient = true;
                EvaluateAtParameters(point);
            }
            return CurrentGradient;
        }

        public double[] Hessian(double[] point)
        {
            if (HasChanged(Parameters, point) || _requireHessian == false)
            {
                _requireHessian = true;
                EvaluateAtParameters(point);
            }
            return CurrentHessian;
        }


        private void EvaluateAtParameters(double[] parameters)
        {
            EvaluationCount += 1;

            Array.Copy(parameters, Parameters, parameters.Length);
            Array.Copy(parameters, _curve.cvDataBlock, parameters.Length);

            var nearestPoints = _curve.NearestSquaredDistance(_cloud, 32);

            CurrentValue = nearestPoints.Distances.Sum();

            for (int i = 0; i < CurrentGradient.Length; i++)
                CurrentGradient[i] = 0;

            double[] cp = new double[2];

            for (int p = 0; p < _cloud.PointCount; p++)
            {
                _cloud.GetPointFast(p, cp);

                double t = nearestPoints.Parameters[p];

                //need basis functions for M and V, so evaluate them explicitly
                int[] indices = new int[3];
                double[] basis = new double[9];
                Nurbs.Algorithm.BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(_curve.knot, t, ref basis, ref indices);

                double[] d0 = new double[_curve._dimension];
                double[] d1 = new double[_curve._dimension];
                double[] d2 = new double[_curve._dimension];

                _curve.EvaluateAllDerivatives(basis, indices, ref d0, ref d1, ref d2);

                double[] tangent = new double[_curve._dimension];
                double[] normal = new double[_curve._dimension];
                double curvature = 0;

                Curvature.Curvature_TwoDimensions(d1, d2, ref tangent, ref normal, ref curvature);

                //alpha = [C(t) - X].T(t)
                double cxt = (d0[0] - cp[0]) * tangent[0];
                double cyt = (d0[1] - cp[1]) * tangent[1];
                double alpha = cxt + cyt;

                //beta = [C(t) - X].N(t)
                double cxn = (d0[0] - cp[0]) * normal[0];
                double cyn = (d0[1] - cp[1]) * normal[1];
                double beta = cxn + cyn;

                //update Gradient
                for (int i = 0; i < _curve._order; i++)
                {
                    //x-coordinates
                    int vectorIx = indices[i];
                    CurrentGradient[vectorIx] += 2 * beta * normal[0] * basis[i];

                    //y-coordinates
                    vectorIx = indices[i] + _curve._cvCount;
                    CurrentGradient[vectorIx] += 2 * beta * normal[1] * basis[i];
                }


                if (_requireHessian)
                {
                    //update Hessian
                    double tangentCoeff = 0;
                    if (beta > 0)
                    {
                        double d = Math.Sqrt(nearestPoints.Distances[p]);
                        tangentCoeff = d / (d - curvature);
                    }

                    for (int i = 0; i < _curve._order; i++)
                    {
                        for (int j = 0; j < _curve._order; j++)
                        {
                            double basisFunctions = 2 * basis[i] * basis[j];

                            //x-coordinates to x-coordinates
                            int vectorIx = indices[i];
                            int vectorIy = indices[j];
                            int hessianPos = vectorIx + _curve.cvDataBlock.Length * vectorIy;

                            CurrentHessian[hessianPos] += basisFunctions * (tangentCoeff * tangent[0] * tangent[0] + normal[0] * normal[0]);


                            //x-coordinates to y-coordinates
                            vectorIx = indices[i];
                            vectorIy = indices[j] + _curve._cvCount;
                            hessianPos = vectorIx + _curve.cvDataBlock.Length * vectorIy;

                            CurrentHessian[hessianPos] += basisFunctions * (tangentCoeff * tangent[0] * tangent[1] + normal[0] * normal[1]);


                            //y-coordinates to x-coordinates
                            vectorIx = indices[i] + _curve._cvCount;
                            vectorIy = indices[j];
                            hessianPos = vectorIx + _curve.cvDataBlock.Length * vectorIy;

                            CurrentHessian[hessianPos] += basisFunctions * (tangentCoeff * tangent[1] * tangent[0] + normal[1] * normal[0]);


                            //y-coordinates to y-coordinates
                            vectorIx = indices[i] + _curve._cvCount;
                            vectorIy = indices[j] + _curve._cvCount;
                            hessianPos = vectorIx + _curve.cvDataBlock.Length * vectorIy;

                            CurrentHessian[hessianPos] += basisFunctions * (tangentCoeff * tangent[1] * tangent[1] + normal[1] * normal[1]);
                        }
                    }
                }
            }

        }


        private bool HasChanged(double[] currentArray, double[] newArray)
        {
            if (currentArray == null) return true;

            for (int i = 0; i< currentArray.Length; i++)
            {
                if (currentArray[i] != newArray[i])
                {
                    return true;
                }
            }
            return false;
        }









        private void UpdateCurveVariablePositionVariableTangent(double[] point)
        {
            _curve.cvDataBlock = point;
        }


        private void UpdateCurveFixedPositionVariableTangent(double[] point)
        {
            //_curve.cvDataBlock = _initialCvDataBlock;

            double[] start = _curve.Evaluate(_curve.Domain.Min);
            double[] end = _curve.Evaluate(_curve.Domain.Max);

            _curve.cvDataBlock = point;

            var cv1 = _curve.GetCV(1);
            double[] cv0 = new double[] {2 * start[0] - cv1[0], 2 * start[1] - cv1[1] };
            _curve.SetCV(0, cv0);

            var cvn_1 = _curve.GetCV(_curve.NumberOfPoints - 2);
            double[] cvn = new double[] { 2 * end[0] - cvn_1[0], 2 * end[1] - cvn_1[1] };
            _curve.SetCV(_curve.NumberOfPoints - 1, cvn);
        }

        private void UpdateCurveFixedPositionFixedTangent(double[] point)
        {
            //_curve.cvDataBlock = _initialCvDataBlock;

            double[] start = _curve.Evaluate(_curve.Domain.Min);
            double[] end = _curve.Evaluate(_curve.Domain.Max);

            double[] startTangent = new double[] 
            {
                _curve.GetCV(1)[0] - _curve.GetCV(0)[0],
                _curve.GetCV(1)[1] - _curve.GetCV(0)[1]
            };
            double startLength = Math.Sqrt(startTangent[0] * startTangent[0] + startTangent[1] * startTangent[1]);
            startTangent[0] /= startLength;
            startTangent[1] /= startLength;


            double[] endTangent = new double[]
            {
                _curve.GetCV(_curve.NumberOfPoints - 2)[0] - _curve.GetCV(_curve.NumberOfPoints - 1)[0],
                _curve.GetCV(_curve.NumberOfPoints - 2)[1] - _curve.GetCV(_curve.NumberOfPoints - 1)[1]
            };
            double endLength = Math.Sqrt(endTangent[0] * endTangent[0] + endTangent[1] * endTangent[1]);
            endTangent[0] /= endLength;
            endTangent[1] /= endLength;


            _curve.cvDataBlock = point;

            var cv1 = _curve.GetCV(1);
            double proj1 = ((cv1[0] - start[0]) * startTangent[0] + (cv1[1] - start[1]) * startTangent[1]);
            cv1[0] = start[0] + startTangent[0] * proj1;
            cv1[1] = start[1] + startTangent[1] * proj1;

            double[] cv0 = new double[] { 2 * start[0] - cv1[0], 2 * start[1] - cv1[1] };
            _curve.SetCV(0, cv0);
            _curve.SetCV(1, cv1);

            var cvn_1 = _curve.GetCV(_curve.NumberOfPoints - 2);
            double projn = ((cvn_1[0] - end[0]) * endTangent[0] + (cvn_1[1] - end[1]) * endTangent[1]);
            cvn_1[0] = end[0] + endTangent[0] * projn;
            cvn_1[1] = end[1] + endTangent[1] * projn;

            double[] cvn = new double[] { 2 * end[0] - cvn_1[0], 2 * end[1] - cvn_1[1] };
            _curve.SetCV(_curve.NumberOfPoints - 1, cvn);
            _curve.SetCV(_curve.NumberOfPoints - 2, cvn_1);
        }

    }
}
