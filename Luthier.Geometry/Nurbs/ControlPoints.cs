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
        public double[] Data { get; set; }

        /// <summary>
        /// Dimension of control points
        /// </summary>
        public int Dimension { get; set; }

        /// <summary>
        /// Number of control points in each direction
        /// </summary>
        public int[] CvCount { get; set; }

        /// <summary>
        /// Stride between consecutive dimensions of same control point. 
        /// If C[i,j]_x = Data[s] then:
        /// C[i, j]_y = Data[s + DimensionStride] 
        /// </summary>
        public int DimensionStride { get; set; }

        /// <summary>
        /// Stride between control points in given direction. 
        /// If C[i,j]_x = Data[s] then: 
        /// C[i + 1, j]_x = Data[s + CvStride[0]] 
        /// C[i, j + 1]_x = Data[s + CvStride[1]]
        /// </summary>
        public int[] CvStride { get; set; }

        public ControlPoints() { }

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
            SetStrides(new int[] { 0, 1 }, 2);
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


        public double[] GetCVSliceThroughPoint(int dimension, int direction, params int[] cvIx)
        {
            double[] result = new double[CvCount[direction]];
            for(int i = 0; i < result.Length; i++)
            {
                cvIx[direction] = i;
                int k = GetDataIndex(dimension, cvIx);
                result[i] = Data[k];
            }

            return result;
        }

        public void SetCVSliceThroughPoint(double[] cvSlice, int dimension, int direction, params int[] cvIx)
        {
            for (int i = 0; i < cvSlice.Length; i++)
            {
                cvIx[direction] = i;
                int k = GetDataIndex(dimension, cvIx);
                Data[k] = cvSlice[i];
            }
        }

        private void SetStrides(int[] cvOrder, int dimensionOrder)
        {
            CvStride[0] = 1;
            for (int i = 1; i < CvCount.Length; i++)
            {
                CvStride[i] = CvStride[i - 1] * CvCount[i - 1];  
            }
            
            DimensionStride = CvStride[CvCount.Length - 1] * CvCount[CvCount.Length - 1];

        }

     
        public ControlPoints Clone()
        {
            var result = new ControlPoints
            {
                Data = new double[Data.Length],
                Dimension = Dimension,
                DimensionStride = DimensionStride,
                CvCount = new int[CvCount.Length],
                CvStride = new int[CvCount.Length],
            };

            Array.Copy(Data, result.Data, Data.Length);
            Array.Copy(CvCount, result.CvCount, CvCount.Length);
            Array.Copy(CvStride, result.CvStride, CvStride.Length);

            return result;
        }


        public ControlPoints Scale(double scaleFactor)
        {
            var result = Clone();

            for (int i = 0; i < result.Data.Length; i++)
                result.Data[i] *= scaleFactor;

            return result;
        }
    }
}
