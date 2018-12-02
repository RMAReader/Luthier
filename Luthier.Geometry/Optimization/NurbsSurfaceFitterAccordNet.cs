using Accord.Math.Optimization;
using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{
    public class NurbsSurfaceFitterAccordNet : NurbsSurfaceFitterBase
    {
        private PointCloud _cloud;
        private NurbsSurface _initialGuess;
        private NurbsSurface _surface;


        public override NurbsSurface Fit(NurbsSurface initialGuess, PointCloud cloud)
        {
            _initialGuess = initialGuess;
            _cloud = cloud;
            _surface = initialGuess;

            TryFit();

            return _surface;
        }

        public NurbsSurface Solution => _surface;


        public void TryFit()
        {
            var squaredDistanceFunction = new NurbsSurfaceSquaredDistance(_initialGuess, _cloud);

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
            var lbfgs = new BroydenFletcherGoldfarbShanno(numberOfVariables: _surface.controlPoints.Data.Length, function: f, gradient: g);

            lbfgs.Progress += Lbfgs_Progress;

            // And then minimize the function:
            sw.Restart();

            bool success = lbfgs.Minimize(_initialGuess.controlPoints.Data);

            long t3 = sw.ElapsedMilliseconds;

            double minValue = lbfgs.Value;
            double[] solution = lbfgs.Solution;

        }

        private void Lbfgs_Progress(object sender, OptimizationProgressEventArgs e)
        {
            var eventArg = new IterationCompleteEventArgs
            {
                Error = e.SolutionNorm,
                NumberOfIterations = e.Iteration,
                Parameters = e.Solution
            };
            OnIterationComplete(eventArg);
        }
    }
}
