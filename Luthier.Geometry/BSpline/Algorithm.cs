﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.BSpline
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
        public static unsafe double Evaluate_CurveSpan_Deboor(
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
            return Evaluate_Span_Deboor_Compact(degree, knotIX, ref knot, p, t);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double Evaluate_SurfaceSpan_Deboor(
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
            for (int k = 0; k < orderV; k++ )
            {
                for (int i = 0; i < orderV; i++)
                {
                    p[i] = cv[cvIX + k * cvStrideU + i * cvStrideV];
                }
                q[k] = Evaluate_Span_Deboor_Compact(orderV - 1, knotIV, ref knotV, p, v);
            }
            return Evaluate_Span_Deboor_Compact(orderU - 1, knotIU, ref knotU, q, u);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double Evaluate_Span_Deboor_Compact(int degree, int knotIX, ref double[] knot, double* cv, double t)
        {
            double alpha;
            for (int r = 1; r <= degree + 1; r++)
            {
                for (int j = degree; j > r - 1; j--)
                {
                    alpha = (t - knot[j + knotIX - degree]) / (knot[j + 1 + knotIX - r] - knot[j + knotIX - degree]);
                    cv[j] = (1.0 - alpha) * cv[j - 1] + alpha * cv[j];
                }
            }
            return cv[degree];
        }


        public static int Find_Knot_Span(int degree, double[] knot, double t)
        {
            int imin = degree - 1;
            int imax = knot.Length - degree - 1;
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





    }
}
