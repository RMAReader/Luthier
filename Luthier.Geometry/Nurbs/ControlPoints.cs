using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{
    public class ControlPoints
    {
        /// <summary>
        /// Array storing control point values in some arrangement
        /// </summary>
        public readonly double[] Data;

        /// <summary>
        /// Dimension of control points
        /// </summary>
        public readonly int Dimension;

        /// <summary>
        /// Number of control points in each direction
        /// </summary>
        public readonly int[] CvCount;

        /// <summary>
        /// Stride between consecutive dimensions of same control point. 
        /// If C[i,j]_x = Data[s] then:
        /// C[i, j]_y = Data[s + DimensionStride] 
        /// </summary>
        public readonly int DimensionStride;

        /// <summary>
        /// Stride between control points in given direction. 
        /// If C[i,j]_x = Data[s] then: 
        /// C[i + 1, j]_x = Data[s + CvStride[0]] 
        /// C[i, j + 1]_x = Data[s + CvStride[1]]
        /// </summary>
        public readonly int[] CvStride;


        public ControlPoints(int dimension, params int[] cvCount)
        {
            Dimension = dimension;
            CvCount = new int[cvCount.Length];
            Array.Copy(cvCount, CvCount, cvCount.Length);

            int dataLength = Dimension;
            for (int i = 0; i < CvCount.Length; i++)
                dataLength *= CvCount[i];

            Data = new double[dataLength];

            CvStride = new int[CvCount.Length];
            SetStrides(new int[] { 0, 1 }, 2, ref CvStride, ref DimensionStride);
        }

        public void GetCV(double[] cv, params int[] cvIx)
        {
            int k = 0;
            for (int i = 0; i < CvStride.Length; i++)
            {
                k += cvIx[i] * CvStride[i];
            }
            for (int d = 0; d < Dimension; d++, k += DimensionStride)
            {
                cv[d] = Data[k];
            }
        }

        public void SetCV(double[] cv, params int[] cvIx)
        {
            int k = 0;
            for (int i = 0; i < CvStride.Length; i++)
            {
                k += cvIx[i] * CvStride[i];
            }
            for (int d = 0; d < Dimension; d++, k += DimensionStride)
            {
                Data[k] = cv[d];
            }
        }

        public int GetDataIndex(int dimension, params int[] cvIx)
        {
            int k = dimension * DimensionStride;
            for (int i = 0; i < CvStride.Length; i++)
            {
                k += cvIx[i] * CvStride[i];
            }
            return k;
        }

        private void SetStrides(int[] cvOrder, int dimensionOrder, ref int[] cvStride, ref int dimensionStride)
        {
            cvStride[0] = 1;
            for (int i = 1; i < CvCount.Length; i++)
            {
                cvStride[i] = CvStride[i - 1] * CvCount[i - 1];  
            }
            
            dimensionStride = cvStride[CvCount.Length - 1] * CvCount[CvCount.Length - 1];

        }
    }
}
