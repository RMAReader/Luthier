using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Providers.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class MathNetTests
    {

        [TestMethod]
        public void Optimisation()
        {
            double gradientTol = 0.00001;
            double parameterTol = 0.00001;
            double functionProgressTol = 0.00001;

            var algorithm = new BfgsBMinimizer(gradientTol, parameterTol, functionProgressTol);

            var function = new MyObjectiveFunction();
            var upperbound = Vector<double>.Build.DenseOfArray(new double[] { 10, 10 });
            var lowerbound = Vector<double>.Build.DenseOfArray(new double[] { 0, 0 });
            var initialguess = Vector<double>.Build.DenseOfArray(new double[] { 4, 4 });

            algorithm.FindMinimum(function, lowerbound, upperbound, initialguess);
        }


        [TestMethod]
        public void FindMinimum_Rosenbrock_Easy()
        {
            var obj = ObjectiveFunction.Gradient(RosenbrockFunction.Value, RosenbrockFunction.Gradient);
            var solver = new BfgsBMinimizer(1e-10, 1e-10, 1e-10, maximumIterations: 100000);
            var lowerBound = new DenseVector(new[] { -5.0, -5.0 });
            var upperBound = new DenseVector(new[] { 5.0, 5.0 });
            var initialGuess = new DenseVector(new[] { 1.2, 1.2 });

            var result = solver.FindMinimum(obj, lowerBound, upperBound, initialGuess);

        }



        [TestMethod]
        public void LinearSystemSolver()
        {
            //test NMath for solving Ax=b

            LinearAlgebraControl.UseBest();
            var provider = LinearAlgebraControl.Provider;

            int dim = 20;
            
            var A = Matrix.Build.Dense(dim, dim);
            var b = Vector.Build.Dense(dim);

            var rnd = new Random();

            for (int i = 0; i < dim; i++)
            {
                b[i] = i;
                for (int j = i; j < Math.Min(i + 10, dim); j++)
                    A[i, j] = rnd.NextDouble();
            }

            //create random positive semi-definite matrix
            A.TransposeAndMultiply(A, A);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            var x = A.Solve(b);

            long t = sw.ElapsedMilliseconds;
            
            //check
            var c = A.TransposeThisAndMultiply(x);
        }

    }



    public class MyObjectiveFunction : IObjectiveFunction
    {

        private Vector<double> _point;
        private double _value;

        public Vector<double> Point => _point;

        public double Value => _value;

        public bool IsGradientSupported => true;

        public Vector<double> Gradient => Vector<double>.Build.DenseOfArray(new double[] 
        {
            2 * (_point[0] - 3),
            2 * (_point[1] - 5)
        });

        public bool IsHessianSupported => false;

        public Matrix<double> Hessian => null;

        public IObjectiveFunction CreateNew()
        {
            return new MyObjectiveFunction();
        }

        public void EvaluateAt(Vector<double> point)
        {
            _point = point.Clone();
            _value = (point[0] - 3) * (point[0] - 3) + (point[1] - 5) * (point[1] - 5);
        }

        public IObjectiveFunction Fork()
        {
            return new MyObjectiveFunction();
        }
    }


    public static class RosenbrockFunction
    {
        public static double Value(Vector<double> input)
        {
            return Math.Pow((1 - input[0]), 2) + 100 * Math.Pow((input[1] - input[0] * input[0]), 2);
        }

        public static Vector<double> Gradient(Vector<double> input)
        {
            Vector<double> output = new DenseVector(2);
            output[0] = -2 * (1 - input[0]) + 200 * (input[1] - input[0] * input[0]) * (-2 * input[0]);
            output[1] = 2 * 100 * (input[1] - input[0] * input[0]);
            return output;
        }

        public static Matrix<double> Hessian(Vector<double> input)
        {
            Matrix<double> output = new DenseMatrix(2, 2);
            output[0, 0] = 2 - 400 * input[1] + 1200 * input[0] * input[0];
            output[1, 1] = 200;
            output[0, 1] = -400 * input[0];
            output[1, 0] = output[0, 1];
            return output;
        }

        public static Vector<double> Minimum
        {
            get
            {
                return new DenseVector(new double[] { 1, 1 });
            }
        }
    }

    public static class BigRosenbrockFunction
    {
        public static double Value(Vector<double> input)
        {
            return 1000.0 + 100.0 * RosenbrockFunction.Value(input / 100.0);
        }

        public static Vector<double> Gradient(Vector<double> input)
        {
            return 100.0 * RosenbrockFunction.Gradient(input / 100.0);
        }

        public static Matrix<double> Hessian(Vector<double> input)
        {
            return 100.0 * RosenbrockFunction.Hessian(input / 100.0);
        }

        public static Vector<double> Minimum
        {
            get
            {
                return new DenseVector(new double[] { 100, 100 });
            }
        }
    }
}
