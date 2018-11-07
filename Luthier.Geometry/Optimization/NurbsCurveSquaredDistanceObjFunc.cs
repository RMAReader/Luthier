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

    public class NurbsCurveSquaredDistanceObjFunc
    {
        private readonly double[] _initialCvDataBlock;
        private NurbsCurve _curve;
        private PointCloud _cloud;
        private readonly EndConstraint _constraint;

        public int ValueCount = 0;

        public NurbsCurveSquaredDistanceObjFunc(NurbsCurve curve, PointCloud cloud, EndConstraint constraint)
        {
            _curve = curve;
            _cloud = cloud;
            _constraint = constraint;

            _initialCvDataBlock = new double[_curve.cvDataBlock.Length];
            Array.Copy(_curve.cvDataBlock, _initialCvDataBlock, _curve.cvDataBlock.Length);
        }


        public double Value(Vector<double> point)
        {
            ValueCount++;

            if (_constraint == EndConstraint.FixedPositionFixedTangent)
            {
                UpdateCurveFixedPositionFixedTangent(point);
            }
            else if (_constraint == EndConstraint.FixedPositionVariableTangent)
            {
                UpdateCurveFixedPositionVariableTangent(point);
            }
            else if(_constraint == EndConstraint.VariablePositionVariableTangent)
            {
                UpdateCurveVariablePositionVariableTangent(point);
            }

            var info = _curve.NearestSquaredDistance(_cloud, 16000);

            double value = 0;
            for(int i=0; i < info.Distances.Length; i++)
            {
                value += info.Distances[i];
            }

            return value;
        }

        public Vector<double> Gradient(Vector<double> point)
        {
            var gradient = Vector<double>.Build.Dense(point.Count, 0);

            double f = Value(point);
            double delta = 0.0001;

            for(int i = 0; i < point.Count; i++)
            {
                double pi = point[i];
                point[i] += delta;
                double df = Value(point) - f;

                gradient[i] = df / delta;
                point[i] = pi;
            }
            return gradient;
        }


        private void UpdateCurveVariablePositionVariableTangent(Vector<double> point)
        {
            _curve.cvDataBlock = point.Storage.AsArray();
        }


        private void UpdateCurveFixedPositionVariableTangent(Vector<double> point)
        {
            _curve.cvDataBlock = _initialCvDataBlock;

            double[] start = _curve.Evaluate(_curve.Domain.Min);
            double[] end = _curve.Evaluate(_curve.Domain.Max);

            _curve.cvDataBlock = point.Storage.AsArray();

            var cv1 = _curve.GetCV(1);
            double[] cv0 = new double[] {2 * start[0] - cv1[0], 2 * start[1] - cv1[1] };
            _curve.SetCV(0, cv0);

            var cvn_1 = _curve.GetCV(_curve.NumberOfPoints - 2);
            double[] cvn = new double[] { 2 * end[0] - cvn_1[0], 2 * end[1] - cvn_1[1] };
            _curve.SetCV(_curve.NumberOfPoints - 1, cvn);
        }

        private void UpdateCurveFixedPositionFixedTangent(Vector<double> point)
        {
            _curve.cvDataBlock = _initialCvDataBlock;

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


            _curve.cvDataBlock = point.Storage.AsArray();

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
