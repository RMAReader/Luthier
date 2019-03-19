using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Luthier.Geometry.Nurbs
{


    public class Algorithm
    {

        /*Basis function value at x*/
        public static double deboor_value(int i, int p, double[] knot, double x)
        {

            if (i < 0) throw new ArgumentException("deboor error: require i >= 0");
            if (p < 0) throw new ArgumentException("deboor error: require p >= 0");
            if (i + p + 1 >= knot.Length) throw new ArgumentException("deboor error: require i + p + 1 < knot.length");

            if (x == knot[knot.Length - p - 1]) { x -= x * double.Epsilon; }

            if (p == 0)
            {
                if (knot[i] <= x && x < knot[i + 1])
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            double d1 = knot[i + p] - knot[i];
            double d2 = knot[i + p + 1] - knot[i + 1];

            double val1 = 0;
            double val2 = 0;
            if (d1 > 0)
            {
                val1 = (x - knot[i]) / d1 * deboor_value(i, p - 1, knot, x);
            }
            if (d2 > 0)
            {
                val2 = (knot[i + p + 1] - x) / d2 * deboor_value(i + 1, p - 1, knot, x);
            }
            return val1 + val2;
        }


        /*Basis function derivative value at x*/
        public static double deboor_derivative(int i, int p, double[] knot, double x)
        {

            if (i < 0) throw new ArgumentException("deboor error: require i >= 0");
            if (p < 1) throw new ArgumentException("deboor error: require p >= 1");
            if (i + p + 1 >= knot.Length) throw new ArgumentException("deboor error: require i + p + 1 < knot.length");

            double value1 = deboor_value(i, p - 1, knot, x);
            double value2 = deboor_value(i + 1, p - 1, knot, x);

            double d1 = knot[i + p] - knot[i];
            double d2 = knot[i + p + 1] - knot[i + 1];

            double val1 = 0;
            double val2 = 0;

            if (d1 > 0)
            {
                val1 += p / d1 * value1;
            }
            if (d2 > 0)
            {
                val2 += -p / d2 * value2;
            }
            return val1 + val2;
        }


        /*Basis function value and derivative value at x*/
        public static void deboor_values(int i, int p, double[] knot, double x, double[] result)
        {

            double value1 = deboor_value(i, p - 1, knot, x);
            double value2 = deboor_value(i + 1, p - 1, knot, x);

            double d1 = knot[i + p] - knot[i];
            double d2 = knot[i + p + 1] - knot[i + 1];

            for (int j = 0; j < p; j++) result[j] = 0;
            if (d1 > 0)
            {
                result[0] += (x - knot[i]) / d1 * value1;
                result[1] += p / d1 * value1;
            }
            if (d2 > 0)
            {
                result[0] += (knot[i + p + 1] - x) / d2 * value2;
                result[1] += -p / d2 * value2;
            }

        }




        /*
        Evaluates a bspline curve at parameter t using deboor algorithm
        */
        //template<class Type>
	    public static Point2D evaluate_curve(int p, List<double> knot, Point2D[] polygon, double t){
		    if (t == knot[knot.Count - p - 1]) t -= t* 1e14;
            int k = Find_Knot_Span(knot, t);
            if (k < 0 || k >= knot.Count) return null;
		    return evaluate_curve_deboor(p, k, p, knot, polygon, t);
        }


        //template<class Type>
        static Point2D evaluate_curve_deboor(int p, int i, int j, List<double> knot, Point2D[] polygon, double t)
        {
            if (j == 0) return polygon[i];
            double d = (knot[i + p + 1 - j] - knot[i]);
            if (d > 0)
            {
                double a = (t - knot[i]) / d;
                Point2D res1 = evaluate_curve_deboor(p, i - 1, j - 1, knot, polygon, t);
                res1 *= (1 - a);
                Point2D res2 = evaluate_curve_deboor(p, i, j - 1, knot, polygon, t);
                res2 *= a;
                res1 += res2;
                return res1;
            }
            return null;
        }





        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BasisFunction_Evaluate(
            int functionIX,
            int degree,
            double[] knot,
            double t)
        {
            if(degree == 2)
            {
                return BasisFunction_Evaluate_DegreeTwo(functionIX, knot, t);
            }
            else
            {
                return deboor_value(functionIX, degree, knot, t);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BasisFunction_Evaluate_DegreeTwo(
            int functionIX,
            double[] knot,
            double t)
        {
            double n02;

            if (t < knot[functionIX])
            {
                n02 = 0;
            }
            else if (t < knot[functionIX + 1])
            {
                double alpha = t - knot[functionIX];

                n02 = alpha * alpha / (knot[functionIX + 1] - knot[functionIX]) / (knot[functionIX + 2] - knot[functionIX]);

            }
            else if (t < knot[functionIX + 2])
            {
                double alpha = (knot[functionIX + 2] - knot[functionIX + 1]);

                double n11 = (t - knot[functionIX + 1]);

                double n01 = (knot[functionIX + 2] - t);

                n02 = (n01 * (t - knot[functionIX]) / (knot[functionIX + 2] - knot[functionIX])
                    + n11 * (knot[functionIX + 3] - t) / (knot[functionIX + 3] - knot[functionIX + 1])) / alpha;

            }
            else if (t < knot[functionIX + 3])
            {
                double alpha = knot[functionIX + 3] - t;

                n02 = alpha * alpha / (knot[functionIX + 3] - knot[functionIX + 2]) / (knot[functionIX + 3] - knot[functionIX + 1]);

            }
            else
            {
                n02 = 0;
            }

            return n02;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BasisFunction_EvaluateFirstDerivative(
            int functionIX,
            int degree,
            double[] knot,
            double t)
        {
            if(degree == 2)
            {
                return BasisFunction_EvaluateFirstDerivative_DegreeTwo(functionIX, knot, t);
            }
            else
            {
                return deboor_derivative(functionIX, degree, knot, t);
            }
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BasisFunction_EvaluateFirstDerivative_DegreeTwo(
             int functionIX,
             double[] knot,
             double t)
        {
            double n02;

            if (t < knot[functionIX])
            {
                n02 = 0;
            }
            else if (t < knot[functionIX + 1])
            {
                double n01 = (t - knot[functionIX]) / (knot[functionIX + 1] - knot[functionIX]);

                n02 = 2 * n01 / (knot[functionIX + 2] - knot[functionIX]);
            }
            else if (t < knot[functionIX + 2])
            {
                double alpha = (knot[functionIX + 2] - knot[functionIX + 1]);

                double n11 = (t - knot[functionIX + 1]);

                double n01 = (knot[functionIX + 2] - t);

                n02 = 2 * (n01 / (knot[functionIX + 2] - knot[functionIX]) - n11 / (knot[functionIX + 3] - knot[functionIX + 1])) / alpha;

            }
            else if (t < knot[functionIX + 3])
            {
                double n11 = (knot[functionIX + 3] - t) / (knot[functionIX + 3] - knot[functionIX + 2]);

                n02 = -2 * n11 / (knot[functionIX + 3] - knot[functionIX + 1]);
            }
            else
            {
                n02 = 0;
            }

            return n02;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BasisFunction_EvaluateSecondDerivative_DegreeTwo(
             int functionIX,
             double[] knot,
             double t)
        {
            double n02;

            if (t < knot[functionIX])
            {
                n02 = 0;
            }
            else if (t < knot[functionIX + 1])
            {
                n02 = 2 / (knot[functionIX + 1] - knot[functionIX]) / (knot[functionIX + 2] - knot[functionIX]);
            }
            else if (t < knot[functionIX + 2])
            {
                n02 = -2 * (1 / (knot[functionIX + 2] - knot[functionIX]) + 1 / (knot[functionIX + 3] - knot[functionIX + 1])) / (knot[functionIX + 2] - knot[functionIX + 1]);

            }
            else if (t < knot[functionIX + 3])
            {
                n02 = 2 / (knot[functionIX + 3] - knot[functionIX + 2]) / (knot[functionIX + 3] - knot[functionIX + 1]);
            }
            else
            {
                n02 = 0;
            }

            return n02;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BasisFunction_EvaluateAllDerivatives_DegreeTwo(
            int functionIX,
            double[] knot,
            double t,
            ref double[] values)
        {
            if (t < knot[functionIX])
            {
                values[0] = 0;
                values[1] = 0;
                values[2] = 0;
            }
            else if (t < knot[functionIX + 1])
            {
                double alpha = t - knot[functionIX];

                values[2] = 2 / (knot[functionIX + 1] - knot[functionIX]) / (knot[functionIX + 2] - knot[functionIX]);
                values[1] = alpha * values[2];
                values[0] = 0.5 * alpha * values[1];
            }
            else if (t < knot[functionIX + 2])
            {
                double a = knot[functionIX + 2] - knot[functionIX + 1];
                double b = -2 / (knot[functionIX + 2] - knot[functionIX]) / a;
                double c = -2 / (knot[functionIX + 3] - knot[functionIX + 1]) / a;

                values[2] = b + c;

                b *= t - knot[functionIX + 2];
                c *= t - knot[functionIX + 1];

                values[1] = b + c;
                values[0] = 0.5 * ((t - knot[functionIX]) * b - (knot[functionIX + 3] - t) * c);
            }
            else if (t < knot[functionIX + 3])
            {
                double alpha = t - knot[functionIX + 3];

                values[2] = 2 / (knot[functionIX + 3] - knot[functionIX + 2]) / (knot[functionIX + 3] - knot[functionIX + 1]);
                values[1] = alpha * values[2];
                values[0] = 0.5 * alpha * values[1];
            }
            else
            {
                values[0] = 0;
                values[1] = 0;
                values[2] = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BasisFunction_EvaluateAllNonZero_DegreeTwo(
            double[] knot,
            double t,
            ref double[] values,
            ref int[] indices)
        {
            //we know that knot knot[knotIX] <= t < knot[knotIX + 1] (or t is outside of valid domain)
            int knotIX = Find_Knot_Span(2, knot, t);

            indices[2] = knotIX;
            indices[1] = knotIX - 1;
            indices[0] = knotIX - 2;

            double a = 1 / (knot[knotIX + 1] - knot[knotIX]);
            double b = (knot[knotIX + 1] - t);
            double c = a * b / (knot[knotIX + 1] - knot[knotIX - 1]);

            values[0] = b * c;
            values[1] = (t - knot[knotIX - 1]) * c;

            b = (t - knot[knotIX]);
            c = a * b / (knot[knotIX + 2] - knot[knotIX]);

            values[1] += (knot[knotIX + 2] - t) * c;
            values[2] = b * c;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BasisFunction_EvaluateAllNonZero_DegreeOne(
            double[] knot,
            double t,
            ref double[] values,
            ref int[] indices)
        {
            //we know that knot knot[knotIX] <= t < knot[knotIX + 1] (or t is outside of valid domain)
            int knotIX = Find_Knot_Span(1, knot, t);

            indices[0] = knotIX - 1;
            indices[1] = knotIX;

            double a = 1 / (knot[knotIX + 1] - knot[knotIX]);

            values[0] = (knot[knotIX + 1] - t) * a;
            values[1] = (t - knot[knotIX]) * a;
            
        }



        /// <summary>
        /// returns all derivatives for all non-zero basis functions at t as follows:
        /// values = {n[i](t), n[i+1](t), n[i+2](t), n'[i](t), n'[i+1](t), n'[i+2](t), n''[i](t), n''[i+1](t), n''[i+2](t) }
        /// </summary>
        /// <param name="knot"></param>
        /// <param name="t"></param>
        /// <param name="values"></param>
        /// <param name="indices"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(
            double[] knot,
            double t,
            ref double[] values,
            ref int[] indices)
        {
            //we know that knot knot[knotIX] <= t < knot[knotIX + 1] (or t is outside of valid domain)
            int knotIX = Find_Knot_Span(2, knot, t);

            indices[2] = knotIX;
            indices[1] = knotIX - 1;
            indices[0] = knotIX - 2;

            //new version********************************************************************************
            double e = 2 / (knot[knotIX + 1] - knot[knotIX]);
            double f = e / (knot[knotIX + 1] - knot[knotIX - 1]);
            double g = e / (knot[knotIX + 2] - knot[knotIX]);

            double k = t - knot[knotIX + 2];
            double h = t - knot[knotIX + 1];
            double i = t - knot[knotIX];
            double j = t - knot[knotIX - 1];

            //first basis function and derivatives
            values[6] =  f;
            values[3] =  f * h;
            values[0] = 0.5 * values[3] * h;

            //third basis function and derivatives
            values[8] = g;
            values[5] = g * i;
            values[2] = 0.5 * values[5] * i;

            //second basis function and derivatives
            values[7] = -(values[6] + values[8]);
            values[4] = -(values[3] + values[5]);
            values[1] = 0.5 * (-values[3] * j - values[5] * k);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double BasisFunction_EvaluateDerivative_Deboor(
            int derivative,
            int functionIX,
            int degree,
            int knotIX,
            ref double[] knot,
            double t)
        {
            int pIX = functionIX - knotIX + 1;
            if (pIX < derivative || pIX > degree)
            {
                return 0;
            }
            else
            {
                double* p = stackalloc double[degree + 1];
                for (int i = 0; i < degree + 1; i++) p[i] = 0;
                p[pIX] = 1;

                for (int der = 0; der < derivative; der++)
                {
                    int imax = degree - der;
                    for (int i = 0; i < degree - der; i++)
                    {
                        double alpha = imax / (knot[knotIX + imax + i - 1] - knot[knotIX + i - 1]);
                        p[i] = alpha * (p[i + 1] - p[i]);
                    }
                }

                return CompactSpan_Evaluate_Deboor(degree, knotIX, ref knot, p, t);
            }
        }

       
        public static unsafe void Curve_EvaluateGivenBasisFunctions(
            int degree,
            int dimension,
            int startIx, 
            double[] basisFunctions, 
            double[] cv,
            int cvStride,
            int dimensionStride,
            ref double[] result)
        {

                for (int d = 0, k = 0; k < dimension; d += dimensionStride, k++)
                {
                    result[k] = 0;
                    for (int i = 0, j = startIx + d; i <= degree; i++, j += cvStride)
                    {
                        result[k] += basisFunctions[i] * cv[j];
                    }
                }

            
        }



        /// <summary>
        /// Evaluates 1D span of bspline curve.  
        /// Using cvIX and cvStride allows curves/surfaces of any dimension to be evaluated by providing control vector data as one double[] datablock.
        /// Using local working array declared on the stack is faster than using a recursive evaluation function.
        /// </summary>
        /// <param name="degree">Degree of bspline curve. (Order = Degree + 1).</param>
        /// <param name="knotIX">Knot vector index for which knot[knotIX] <= t < knot[knotIX + 1]</param>
        /// <param name="knot">Knot vector array</param>
        /// <param name="cv">Control point array</param>
        /// <param name="cvIX">Index such that control points cv[cvIX], cv[cvIX + cvStride] + ... + cv[cvIX + cvStride * degree] are required for evaluation at t</param>
        /// <param name="cvStride">Stride such that control points cv[cvIX], cv[cvIX + cvStride] + ... + cv[cvIX + cvStride * degree] are required for evaluation at t</param>
        /// <param name="t">Evaluation parameter</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double CurveSpan_Evaluate_Deboor(
            int degree, 
            int knotIX, 
            ref double[] knot, 
            ref double[] cv, 
            int cvIX, 
            int cvStride, 
            double t)
        {
            //create working array on the stack
            double* p = stackalloc double[degree + 1];
            for (int i = 0; i < degree + 1; i++)
            {
                p[i] = cv[cvIX + i * cvStride];
            }

            return CompactSpan_Evaluate_Deboor(degree, knotIX, ref knot, p, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CurveSpan_Evaluate_Deboor2(
           int degree,
           int knotIX,
           ref double[] knot,
           ref double[] cv,
           int cvIX,
           int cvStride,
           double t)
        {
            double c = 0;
            if (degree == 2)
            {
                //int[] indices = new int[3];
                //double[] values = new double[3];
                //BasisFunction_EvaluateAllNonZero_DegreeTwo(knot, t, ref values, ref indices);
                for (int i = 0; i < degree + 1; i++)
                {
                    c += cv[cvIX + i * cvStride] * BasisFunction_Evaluate_DegreeTwo(i + knotIX - degree, knot, t);
                    //c += cv[cvIX + i * cvStride] * values[i];
                }
            }
            else
            {
                for (int i = 0; i < degree + 1; i++)
                {
                    c += cv[cvIX + i * cvStride] * BasisFunction_Evaluate(knotIX - degree, degree, knot, t);
                }
            }
            return c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double CurveSpan_EvaluateFirstDerivative_Deboor(
            int degree,
            int knotIX,
            ref double[] knot,
            double[] cv,
            int cvIX,
            int cvStride,
            double t)
        {
            //create working array on the stack
            double* p = stackalloc double[degree];
            for (int i = 0; i < degree; i++)
            {
                double alpha = degree / (knot[knotIX + degree + i - 1] - knot[knotIX + i - 1]);
                p[i] = alpha * (cv[cvIX + (i + 1) * cvStride] - cv[cvIX + i * cvStride]);
            }
            return CompactSpan_Evaluate_Deboor(degree - 1, knotIX, ref knot, p, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double CurveSpan_EvaluateDerivative_Deboor(
            int derivative,
            int degree,
            int knotIX,
            ref double[] knot,
            double[] cv,
            int cvIX,
            int cvStride,
            double t)
        {
            //create working array on the stack
            double* p = stackalloc double[degree + 1];
            for (int i = 0; i < degree + 1; i++)
            {
                p[i] = cv[cvIX + i * cvStride];
            }

            for (int der = 0; der < derivative; der++)
            {
                int imax = degree - der;
                for (int i = 0; i < degree - der; i++)
                {
                    double alpha = imax / (knot[knotIX + imax + i - 1] - knot[knotIX + i - 1]);
                    p[i] = alpha * (p[i + 1] - p[i]);
                }
            }
            
            return CompactSpan_Evaluate_Deboor(degree - derivative, knotIX, ref knot, p, t);
        }


        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double SurfaceSpan_Evaluate_Deboor(
            int orderU, 
            int orderV, 
            int knotIU, 
            int knotIV, 
            ref double[] knotU, 
            ref double[] knotV, 
            ref double[] cv, 
            int cvIX, 
            int cvStrideU, 
            int cvStrideV, 
            double u, 
            double v)
        {
            //create working arrays on the stack
            double* p = stackalloc double[orderV];
            double* q = stackalloc double[orderU];

            //evaluate V direction, creating new control points for U direction
            for (int k = 0; k < orderU; k++ )
            {
                for (int i = 0; i < orderV; i++)
                {
                    p[i] = cv[cvIX + k * cvStrideU + i * cvStrideV];
                }
                q[k] = CompactSpan_Evaluate_Deboor(orderV - 1, knotIV, ref knotV, p, v);
            }
            return CompactSpan_Evaluate_Deboor(orderU - 1, knotIU, ref knotU, q, u);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ConvertPointsForDerivative(
            int derivative,
            int degree,
            int knotIX,
            ref double[] knot,
            double* p)
        {
            for (int der = 0; der < derivative; der++)
            {
                int imax = degree - der;
                for (int i = 0; i < degree - der; i++)
                {
                    double alpha = imax / (knot[knotIX + imax + i - 1] - knot[knotIX + i - 1]);
                    p[i] = alpha * (p[i + 1] - p[i]);
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double CompactSpan_Evaluate_Deboor(int degree, int knotIX, ref double[] knot, double* cv, double t)
        {
            double alpha, d;
            for (int r = 0; r < degree; r++)
            {
                for (int j = degree; j > r; j--)
                {
                    alpha = (t - knot[knotIX + j - degree]) / (knot[knotIX + j - r] - knot[knotIX + j - degree]);

                    cv[j] = (1.0 - alpha) * cv[j - 1] + alpha * cv[j];
                }
            }
            return cv[degree];
        }


        #region "evaluation algorithms using SIMD vectorization"

        /*
         There was no speed up from using SIMD types on desktop machine - in fact, a small slow down.
             
             */

        ///// <summary>
        ///// Evaluates 1D span of bspline curve.  
        ///// Using cvIX and cvStride allows curves/surfaces of any dimension to be evaluated by providing control vector data as one double[] datablock.
        ///// Using local working array declared on the stack is faster than using a recursive evaluation function.
        ///// </summary>
        ///// <param name="degree">Degree of bspline curve. (Order = Degree + 1).</param>
        ///// <param name="knotIX">Knot vector index for which knot[knotIX] <= t < knot[knotIX + 1]</param>
        ///// <param name="knot">Knot vector array</param>
        ///// <param name="cv">Control point array</param>
        ///// <param name="cvIX">Index such that control points cv[cvIX], cv[cvIX + cvStride] + ... + cv[cvIX + cvStride * degree] are required for evaluation at t</param>
        ///// <param name="cvStride">Stride such that control points cv[cvIX], cv[cvIX + cvStride] + ... + cv[cvIX + cvStride * degree] are required for evaluation at t</param>
        ///// <param name="t">Evaluation parameter</param>
        ///// <returns></returns>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static Vector<double> Evaluate_CurveSpan_DeboorSIMD(
        //    int degree,
        //    int knotIX,
        //    ref double[] knot,
        //    ref double[] cv,
        //    int cvIX,
        //    int cvStride,
        //    double t)
        //{
        //    Vector<double>[] p = new Vector<double>[degree + 1];
        //    for (int i = 0; i < degree + 1; i++)
        //    {
        //        p[i] = new Vector<double> (cv, cvIX + i * cvStride);
        //    }
        //    return Evaluate_Span_Deboor_CompactSIMD(degree, knotIX, ref knot, p, t);
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static System.Numerics.Vector<double> Evaluate_Span_Deboor_CompactSIMD(int degree, int knotIX, ref double[] knot, System.Numerics.Vector<double>[] cv, double t)
        //{
        //    double alpha;
        //    for (int r = 0; r < degree; r++)
        //    {
        //        for (int j = degree; j > r; j--)
        //        {
        //            alpha = (t - knot[knotIX + j - degree]) / (knot[knotIX + j - r] - knot[knotIX + j - degree]);
        //            cv[j] = (1.0 - alpha) * cv[j - 1] + alpha * cv[j];
        //        }
        //    }
        //    return cv[degree];
        //}

        #endregion

        public static int Find_Knot_Span(int degree, double[] knot, double t)
        {
            int imin = degree;
            int imax = knot.Length - degree - 2;
            if (knot[imin] > t) return imin;
            if (knot[imax] <= t) return imax;
            for (int i = imin; i <= imax; i++)
            {
                if (knot[i] <= t && t < knot[i + 1]) return i;
            }
            return -1;
        }

     

        /*
        Oslo algorithm taken from Cohen et al.
        */
        public static int Find_Knot_Span(List<double> TAU, double Tj)
        {
            for (int i = 0; i < TAU.Count - 1; i++)
            {
                if (TAU[i] <= Tj && Tj < TAU[i + 1]) return i;
            }
            if (TAU[TAU.Count - 1] == Tj) return TAU.Count - 2;
            return -1;
        }

        //template<class Type>
        static Point2D olso_subdiv(List<Point2D> originalControlPoints, int Order, List<double> OriginalKnot, List<double> newKnot, int RPI, int i, int j)
        {
            int r = RPI - 1;
            if (r > 0)
            {
                Point2D PP2 = new Point2D(0,0);
                Point2D PP1 = new Point2D(0,0);
                double P1 = newKnot[j + Order - r] - OriginalKnot[i];
                double P2 = OriginalKnot[i + Order - r] - newKnot[j + Order - r];
                if (P1 != 0) PP1 = olso_subdiv(originalControlPoints, Order, OriginalKnot, newKnot, r, i, j);
                if (P2 != 0) PP2 = olso_subdiv(originalControlPoints, Order, OriginalKnot, newKnot, r, i - 1, j);
                if (Math.Abs(P1 + P2) > double.Epsilon)
                {
                    return (P1 * PP1 + P2 * PP2) / (P1 + P2);
                }
                else
                {
                    return new Point2D(0,0);
                }
            }
            else
            {
                return originalControlPoints[i];
            }
        }

        ///*
        //N = where original polygon has N+1 vertices
        //P = original polygon of Type
        //K = order of bspline curve (i.e. p+1)
        //TAU = original knot vector of length N + K
        //T = refinement knot vector of length Q > N + K
        //D = subdivided polygon of Type
        //*/
        //template<class Type>
        public static List<Point2D> olso_insertion(List<Point2D> OriginalControlPoints, int Order, List<double> OriginalKnot, List<double> newKnot)
        {
            int MU;
            List<Point2D> newControlPoints = new List<Point2D>(newKnot.Count - Order - 1);
            for (int j = 0; j < newKnot.Count - Order - 1; j++)
            {
                MU = Find_Knot_Span(OriginalKnot, newKnot[j]);
                newControlPoints.Add(olso_subdiv(OriginalControlPoints, Order + 1, OriginalKnot, newKnot, Order + 1, MU, j));
            }
            return newControlPoints;
        }





        ///*
        //N = where original polygon has N+1 vertices
        //P = original polygon of Type
        //K = order of bspline curve (i.e. p+1)
        //TAU = original knot vector of length N + K
        //T = refinement knot vector of length Q > N + K
        //D = subdivided polygon of Type
        //*/
        public static double[] KnotInsertionOslo(double[] OriginalControlPoints, int degree, double[] OriginalKnot, double[] newKnot)
        {
            int MU;
            double[] newControlPoints = new double[newKnot.Length - degree - 1];
            for (int j = 0; j < newControlPoints.Length; j++)
            {
                MU = Find_Knot_Span(degree - 1, OriginalKnot, newKnot[j]);
                newControlPoints[j] = olso_subdiv_recursive(OriginalControlPoints, degree + 1, OriginalKnot, newKnot, degree + 1, MU, j);
                //newControlPoints[j] = olso_subdiv(OriginalControlPoints, degree + 1, OriginalKnot, newKnot, MU, j);
            }
            return newControlPoints;
        }

        static double olso_subdiv_recursive(double[] originalControlPoints, int Order, double[] OriginalKnot, double[] newKnot, int RPI, int i, int j)
        {
            int r = RPI - 1;
            if (r > 0)
            {
                double PP2 = 0;
                double PP1 = 0;
                double P1 = newKnot[j + Order - r] - OriginalKnot[i];
                double P2 = OriginalKnot[i + Order - r] - newKnot[j + Order - r];
                if (P1 != 0) PP1 = olso_subdiv_recursive(originalControlPoints, Order, OriginalKnot, newKnot, r, i, j);
                if (P2 != 0) PP2 = olso_subdiv_recursive(originalControlPoints, Order, OriginalKnot, newKnot, r, i - 1, j);
                if (Math.Abs(P1 + P2) > double.Epsilon)
                {
                    return (P1 * PP1 + P2 * PP2) / (P1 + P2);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return originalControlPoints[i];
            }
        }

        static double olso_subdiv(double[] P, int K, double[] TAU, double[] T, int MU, int J)
        {
            double[] TEMP = new double[MU + 1];
            for (int i = MU - K + 1; i <= MU; i++)
            {
                TEMP[i] = P[i];
            }
            for (int r = 1; r < K; r++)
            {
                for (int i = MU - K + 1 + r; i <= MU; i++)
                {
                    double T1 = T[J + K - r] - TAU[i];
                    double T2 = TAU[i + K - r] - T[J + K - r];
                    double d = T1 + T2;
                    TEMP[i] = (Math.Abs(d) > double.Epsilon) ? (T1 * TEMP[i] + T2 * TEMP[i - 1])/d : 0;

                }
            }
            return TEMP[MU];
        }




    }
}
