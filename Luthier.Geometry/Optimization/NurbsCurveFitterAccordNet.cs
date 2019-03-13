using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Optimization;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using MathNet.Numerics.LinearAlgebra;
using Accord.Math;

namespace Luthier.Geometry.Optimization
{
    public class NurbsCurveFitterAccordNet : NurbsCurveFitterBase
    {

        private PointCloud _cloud;
        private NurbsCurve _initialGuess;
        private NurbsCurve _curve;


        public override NurbsCurve Fit(NurbsCurve initialGuess, PointCloud cloud)
        {
            _initialGuess = initialGuess;
            _cloud = cloud;
            _curve = initialGuess.DeepCopy();

            TryFit3();

            return _curve;
        }


        public void TryFit3()
        {
            var squaredDistanceFunction = new NurbsCurveSquaredDistance(_initialGuess, _cloud, EndConstraint.VariablePositionVariableTangent);
           

            List<double> values = new List<double>();

            long t1 = 0;
            long t2 = 0;

            var sw = new System.Diagnostics.Stopwatch();

            Func<double[], double> f = (x) =>
            {
                long t = sw.ElapsedMilliseconds;

                double value = squaredDistanceFunction.Value(x);
                values.Add(value);

                t1 += sw.ElapsedMilliseconds - t;
                return value;
            };


            Func<double[], double[]> g = (x) => 
            {
                long t = sw.ElapsedMilliseconds;

                var grad = squaredDistanceFunction.Gradient(x);

                t2 += sw.ElapsedMilliseconds - t;

                return grad;
            };

            // Finally, we can create the L-BFGS solver, passing the functions as arguments
            var lbfgs = new BroydenFletcherGoldfarbShanno(numberOfVariables: _curve.ControlPoints.Data.Length, function: f, gradient: g);

            // And then minimize the function:
            sw.Restart();

            bool success = lbfgs.Minimize(_initialGuess.ControlPoints.Data);

            long t3 = sw.ElapsedMilliseconds;

            double minValue = lbfgs.Value;
            double[] solution = lbfgs.Solution;

        }

 

    }
}
