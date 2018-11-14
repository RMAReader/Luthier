using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry.Nurbs;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;

namespace Luthier.Geometry.Optimization
{

    public class NurbsCurveFitterConjugateGradient : NurbsCurveFitterBase
    {
        public override NurbsCurve Fit(NurbsCurve initialGuess, PointCloud cloud)
        {
            var lowerBound = DenseVector.Build.Dense(initialGuess.cvDataBlock.Length, -5000);
            var upperBound = DenseVector.Build.Dense(initialGuess.cvDataBlock.Length, 5000);

            var objFunc = new NurbsCurveSquaredDistanceObjFunc(initialGuess, cloud, EndConstraint.VariablePositionVariableTangent);
            var objvalue = ObjectiveFunction.Value(objFunc.Value);
            var fdgof = new ForwardDifferenceGradientObjectiveFunction(objvalue, lowerBound, upperBound);

            int iteration = 0;

            while (iteration < 20)
            {
                try
                {
                    var currentPoint = new DenseVector(initialGuess.cvDataBlock);
                    var cgresult = ConjugateGradientMinimizer.Minimum(fdgof, currentPoint, gradientTolerance: 1e-2, maxIterations: 1);
                }
                catch
                {
                    var e = new IterationCompleteEventArgs();
                    e.NumberOfIterations = iteration;
                    OnIterationComplete(e);
                }
                iteration++;
            }

            return initialGuess;
        }
      

    }


}
