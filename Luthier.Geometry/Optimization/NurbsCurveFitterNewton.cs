using Luthier.Geometry.Nurbs;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{
    public class NurbsCurveFitterNewton : NurbsCurveFitterBase
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
            long t3 = 0;

            var sw = new System.Diagnostics.Stopwatch();

            Func<Vector<double>, double> f = (x) =>
            {
                long t = sw.ElapsedMilliseconds;

                double value = squaredDistanceFunction.Value(x.AsArray());
                values.Add(value);

                t1 += sw.ElapsedMilliseconds - t;
                return value;
            };


            Func<Vector<double>, Vector<double>> g = (x) =>
            {
                long t = sw.ElapsedMilliseconds;

                var grad = squaredDistanceFunction.Gradient(x.AsArray());

                t2 += sw.ElapsedMilliseconds - t;

                return Vector<double>.Build.Dense(grad);
            };

            Func<Vector<double>, Matrix<double>> h = (x) =>
            {
                long t = sw.ElapsedMilliseconds;

                var hessian = squaredDistanceFunction.Hessian(x.AsArray());

                t3 += sw.ElapsedMilliseconds - t;

                return Matrix<double>.Build.Dense(x.Count, x.Count, hessian);
            };

            // Finally, we can create the L-BFGS solver, passing the functions as arguments
            var minimizer = new NewtonMinimizer(gradientTolerance: 1E-6, maximumIterations: 10000, useLineSearch: false);

            var lbfgs = new BfgsMinimizer(
                gradientTolerance: 1E-6, 
                parameterTolerance: 1E-6, 
                functionProgressTolerance: 1E-6);

            var objvalue = ObjectiveFunction.GradientHessian(f, g, h);

            
                
            // And then minimize the function:
            sw.Restart();

            //var result1 = lbfgs.FindMinimum(objvalue, Vector<double>.Build.Dense(_initialGuess.cvDataBlock));
            var result2 = minimizer.FindMinimum(objvalue, Vector<double>.Build.Dense(_initialGuess.ControlPoints.Data));

            long t4 = sw.ElapsedMilliseconds;


        }



    }
}