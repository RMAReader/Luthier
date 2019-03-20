using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Extensions
{
    //public static class ArrayExtensions
    //{
    //    public static double DotProduct(this double[] lhs, double[] rhs)
    //    {
    //        if (lhs.Length != rhs.Length) throw new Exception();

    //        switch (lhs.Length)
    //        {
    //            case 1:
    //                return lhs[0] * rhs[0];

    //            case 2:
    //                return lhs[0] * rhs[0] + lhs[1] * rhs[1];

    //            case 3:
    //                return lhs[0] * rhs[0] + lhs[1] * rhs[1] + lhs[2] * rhs[2];

    //            default:
    //                double result = 0.0;
    //                for (int i = 0; i < lhs.Length; i++)
    //                {
    //                    result += lhs[i] * rhs[i];
    //                }
    //                return result;
    //        }
    //    }

    //    public static double[] VectorProduct(this double[] lhs, double[] rhs)
    //    {
    //        if (lhs.Length != 3 || rhs.Length != 3) throw new Exception();

    //        return new double[]
    //        {
    //            lhs[1] * rhs[2] - lhs[2] * rhs[1],
    //            lhs[2] * rhs[0] - lhs[0] * rhs[2],
    //            lhs[0] * rhs[1] - lhs[1] * rhs[0],
    //        };
    //    }

    //    public static double[] Add(this double[] lhs, double[] rhs)
    //    {
    //        if (lhs.Length != rhs.Length) throw new Exception();

    //        switch (lhs.Length)
    //        {
    //            case 1:
    //                return new double[] { lhs[0] + rhs[0] };

    //            case 2:
    //                return new double[] { lhs[0] + rhs[0], lhs[1] + rhs[1] };

    //            case 3:
    //                return new double[] { lhs[0] + rhs[0], lhs[1] + rhs[1], lhs[2] + rhs[2] };

    //            default:
    //                var result = new double[lhs.Length];
    //                for (int i = 0; i < lhs.Length; i++)
    //                {
    //                    result[i] = lhs[i] + rhs[i];
    //                }
    //                return result;
    //        }
    //    }

    //    public static double[] Subtract(this double[] lhs, double[] rhs)
    //    {
    //        if (lhs.Length != rhs.Length) throw new Exception();

    //        switch (lhs.Length)
    //        {
    //            case 1:
    //                return new double[] { lhs[0] - rhs[0] };

    //            case 2:
    //                return new double[] { lhs[0] - rhs[0], lhs[1] - rhs[1] };

    //            case 3:
    //                return new double[] { lhs[0] - rhs[0], lhs[1] - rhs[1], lhs[2] - rhs[2] };

    //            default:
    //                var result = new double[lhs.Length];
    //                for (int i = 0; i < lhs.Length; i++)
    //                {
    //                    result[i] = lhs[i] - rhs[i];
    //                }
    //                return result;
    //        }
    //    }


    //    public static double L2Norm(this double[] array)
    //    {
    //        double l2norm = 0.0;
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            l2norm += array[i] * array[i];
    //        }
    //        return Math.Sqrt(l2norm);
    //    }

    //    public static void Normalise(this double[] array)
    //    {
    //        var l2norm = array.L2Norm();
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            array[i] /= l2norm;
    //        }
    //    }

    //    public static double[] Multiply(this double[] array, double a)
    //    {
    //        var result = new double[array.Length];
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            result[i] = array[i] * a;
    //        }
    //        return result;
    //    }


        
    //}

    
   
}
