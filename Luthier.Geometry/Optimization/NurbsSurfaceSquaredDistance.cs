using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{
    internal class NurbsSurfaceSquaredDistance
    {
        private NurbsSurface _surface;
        private PointCloud _cloud;

        private bool _requireGradient;
        private bool _requireHessian;

        public double CurrentValue { get; private set; }
        public double[] CurrentGradient { get; private set; }
        public double[] Parameters { get; private set; }

        public int EvaluationCount { get; private set; }

        public NurbsSurfaceSquaredDistance(NurbsSurface surface, PointCloud cloud)
        {
            _surface = surface;
            _cloud = cloud;

            _requireGradient = false;
            _requireHessian = false;

            EvaluationCount = 0;
            CurrentValue = double.MaxValue;
            CurrentGradient = new double[_surface.controlPoints.Data.Length];
            Parameters = new double[_surface.controlPoints.Data.Length];
        }

        public double Value(double[] point)
        {
            if (HasChanged(Parameters, point))
            {
                EvaluateAtParameters(point);
            }
            return CurrentValue;

        }

        public double[] Gradient(double[] point)
        {
            if (HasChanged(Parameters, point) || _requireGradient == false)
            {
                _requireGradient = true;
                EvaluateAtParameters(point);
            }
            return CurrentGradient;
        }

   
        private void EvaluateAtParameters(double[] parameters)
        {
            EvaluationCount += 1;

            Array.Copy(parameters, Parameters, parameters.Length);
            Array.Copy(parameters, _surface.controlPoints.Data, parameters.Length);

            for (int i = 0; i < CurrentGradient.Length; i++)
                CurrentGradient[i] = 0;

            CurrentValue = 0;

            double[] cp = new double[_cloud.Dimension];

            for (int p = 0; p < _cloud.PointCount; p++)
            {
                _cloud.GetPointFast(p, cp);

                //need to find u,v of nearest point
                double u = 0;
                double v = 0;

                NurbsSurfaceNearestPoint.Find(_surface, _surface.Domain0().Min, _surface.Domain0().Max, _surface.Domain1().Min, _surface.Domain1().Max, cp, ref u, ref v);

                //need basis functions for M and V, so evaluate them explicitly
                int[] indicesU = new int[3];
                double[] basisFuncsU = new double[9];
                Nurbs.Algorithm.BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(_surface.knotArray0, u, ref basisFuncsU, ref indicesU);

                int[] indicesV = new int[3];
                double[] basisFuncsV = new double[9];
                Nurbs.Algorithm.BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(_surface.knotArray1, v, ref basisFuncsV, ref indicesV);

                double[] values = new double[18];

                //returns all derivatives in values array, as {S, dS/du, dS/dv, d2S/D2u, d2S/dudv, d2S/d2v }
                _surface.EvaluateAllDerivativesGivenBasisFunctions(indicesU, basisFuncsU, indicesV, basisFuncsV, ref values);

                CurrentValue += (values[0] - cp[0]) * (values[0] - cp[0]);
                CurrentValue += (values[1] - cp[1]) * (values[1] - cp[1]);
                CurrentValue += (values[2] - cp[2]) * (values[2] - cp[2]);

                int du = 3;
                int dv = 6;

                double[] normal = new double[]
                {
                    values[du + 1] * values[dv + 2] - values[du + 2] * values[dv + 1],
                    values[du + 2] * values[dv + 0] - values[du + 0] * values[dv + 2],
                    values[du + 0] * values[dv + 1] - values[du + 1] * values[dv + 0],
                };
                double normalL2 = normal.L2Norm();

                normal[0] /= normalL2;
                normal[1] /= normalL2;
                normal[2] /= normalL2;
                
                //beta = [C(t) - X].N(t)
                double cxn = (values[0] - cp[0]) * normal[0];
                double cyn = (values[1] - cp[1]) * normal[1];
                double czn = (values[2] - cp[2]) * normal[2];

                double beta = cxn + cyn + czn;

                //update Gradient
                for (int i = 0; i < indicesU.Length; i++)
                {
                    double funcU = 2 * beta * basisFuncsU[i];
                    for (int j = 0; j < indicesV.Length; j++)
                    {
                        double funcUfuncV = funcU * basisFuncsV[j];
                        for(int d = 0; d < _surface.Dimension; d++)
                        {
                            int vectorIx = _surface.controlPoints.GetDataIndex(d, indicesU[i], indicesV[j]);
                            CurrentGradient[vectorIx] += normal[d] * funcUfuncV;
                        }
                    }
                }
                
            }

        }


        private bool HasChanged(double[] currentArray, double[] newArray)
        {
            if (currentArray == null) return true;

            for (int i = 0; i < currentArray.Length; i++)
            {
                if (currentArray[i] != newArray[i])
                {
                    return true;
                }
            }
            return false;
        }




    }
}
