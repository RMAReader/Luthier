using Luthier.Geometry.BSpline;
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
    public class NurbsCurveNearstPointObjFunc
    {
        private NurbsCurve _curve;
        private PointCloud _cloud;

        public NurbsCurveNearstPointObjFunc(NurbsCurve curve, PointCloud cloud)
        {
            _curve = curve;
            _cloud = cloud;
        }


        public double Value(Vector<double> point)
        {
            _curve.cvDataBlock = point.Storage.AsArray();

            var nearestPointCalculator = new NurbsCurveNearestPoint();
            var info = nearestPointCalculator.Calculate(_curve, _cloud);

            return info.Distances.Sum();
        }

        public Vector<double> Gradient(Vector<double> point)
        {
            var gradient = Vector<double>.Build.Dense(point.Count, 0);

            double f = Value(point);
            double delta = 0.1;

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

    }
}
