using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Geometry.Nurbs
{
    [Serializable]
    public class NurbsSurface
    {
        [XmlElement]
        public int Dimension;
        [XmlElement]
        public bool IsRational;
        [XmlElement]
        public int Order0;
        [XmlElement]
        public int Order1;
        [XmlArray]
        public double[] knotArray0;
        [XmlArray]
        public double[] knotArray1;


        public ControlPoints controlPoints;

        public NurbsSurface() { }
        public NurbsSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            Dimension = dimension;
            IsRational = bIsRational;
            Order0 = order0;
            Order1 = order1;
            //CvCount0 = cv_count0;
            //CvCount1 = cv_count1;

            controlPoints = new ControlPoints(CvSize, cv_count0, cv_count1 );

            InitialiseArrays();
        }

        private void InitialiseArrays()
        {
            knotArray0 = new double[controlPoints.CvCount[0] + Order0];
            knotArray1 = new double[controlPoints.CvCount[1] + Order1];
            //cvArray = new double[CvSize * CvCount0 * CvCount1];
        }

        public int CvSize => (IsRational) ? Dimension + 1 : Dimension;
        public int CvCount0 => controlPoints.CvCount[0];
        public int CvCount1 => controlPoints.CvCount[1];

        public Interval Domain0()
        {
            double min = knotArray0[Order0 - 1];
            double max = knotArray0[knotArray0.Length - Order0];
            return new Interval(min, max);
        }
        public Interval Domain1()
        {
            double min = knotArray1[Order1 - 1];
            double max = knotArray1[knotArray1.Length - Order1];
            return new Interval(min, max);
        }

        public double[] GetCV(int i, int j)
        {
            double[] cv = new double[CvSize];
            controlPoints.GetCV(cv, i, j);
            return cv;
        }

        public void SetCV(int i, int j, double[] cv)
        {
            //int startIndex = (CvCount1 * i + j) * CvSize;
            //Array.Copy(cv, 0, cvArray, startIndex, CvSize);

            controlPoints.SetCV(cv, i, j);
        }

        public double[] Evaluate(double u, double v)
        {
            var result = new double[Dimension];
            Evaluate(u, v, result);
            return result;
        }
        public void Evaluate(double u, double v, double[] point )
        {
            if (Order0 == 3 && Order1 == 3)
            {
                int[] indicesU = new int[3];
                double[] valuesU = new double[3];
                Algorithm.BasisFunction_EvaluateAllNonZero_DegreeTwo(knotArray0, u, ref valuesU, ref indicesU);

                int[] indicesV = new int[3];
                double[] valuesV = new double[3];
                Algorithm.BasisFunction_EvaluateAllNonZero_DegreeTwo(knotArray1, v, ref valuesV, ref indicesV);

                for (int d = 0; d < Dimension; d++)
                {
                    point[d] = 0;
                    for (int i = 0; i < Order0; i++)
                    {
                        for (int j = 0; j < Order1; j++)
                        {
                            int index = controlPoints.GetDataIndex(d, indicesU[i], indicesV[j]);
                            point[d] += controlPoints.Data[index] * valuesU[i] * valuesV[j];
                        }
                    }
                }
            }
            else throw new NotImplementedException();
        }



        /// <summary>
        /// returns all derivatives in values array, as {S, dS/du, dS/dv, d2S/D2u, d2S/dudv, d2S/d2v }
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="values"></param>
        public double[] EvaluateAllDerivatives(double u, double v)
        {
            double[] result = new double[18];
            EvaluateAllDerivatives(u, v, result);
            return result;
        }
        /// <summary>
        /// returns all derivatives in values array, as {S, dS/du, dS/dv, d2S/D2u, d2S/dudv, d2S/d2v }
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="values"></param>
        public void EvaluateAllDerivatives(double u, double v, double[] values)
        {
            if (Order0 == 3 && Order1 == 3)
            {
                int[] indicesU = new int[3];
                double[] valuesU = new double[9];
                Algorithm.BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(knotArray0, u, ref valuesU, ref indicesU);

                int[] indicesV = new int[3];
                double[] valuesV = new double[9];
                Algorithm.BasisFunction_EvaluateAllNonZero_AllDerivatives_DegreeTwo(knotArray1, v, ref valuesV, ref indicesV);

                EvaluateAllDerivativesGivenBasisFunctions(indicesU, valuesU, indicesV, valuesV, ref values);
            }
            else throw new NotImplementedException();
        }
        /// <summary>
        /// returns all derivatives in values array, as {S, dS/du, dS/dv, d2S/D2u, d2S/dudv, d2S/d2v }
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="values"></param>
        public void EvaluateAllDerivativesGivenBasisFunctions(int[] indicesU, double[] basisFuncsU, int[] indicesV, double[] basisFuncsV, ref double[] values)
        {
            if (Order0 == 3 && Order1 == 3)
            {
                for (int d = 0; d < Dimension; d++)
                {
                    values[d] = 0;
                    values[d + 3] = 0;
                    values[d + 6] = 0;
                    values[d + 9] = 0;
                    values[d + 12] = 0;
                    values[d + 15] = 0;
                    for (int i = 0; i < Order0; i++)
                    {
                        for (int j = 0; j < Order1; j++)
                        {
                            int index = controlPoints.GetDataIndex(d, indicesU[i], indicesV[j]);

                            //S(u, v)
                            values[d] += controlPoints.Data[index] * basisFuncsU[i] * basisFuncsV[j];

                            //dS/du
                            values[d + 3] += controlPoints.Data[index] * basisFuncsU[i + 3] * basisFuncsV[j];

                            //dS/dv
                            values[d + 6] += controlPoints.Data[index] * basisFuncsU[i] * basisFuncsV[j + 3];

                            //d2S/d2u
                            values[d + 9] += controlPoints.Data[index] * basisFuncsU[i + 6] * basisFuncsV[j];

                            //d2S/dudv
                            values[d + 12] += controlPoints.Data[index] * basisFuncsU[i + 3] * basisFuncsV[j + 3];

                            //d2S/d2v
                            values[d + 15] += controlPoints.Data[index] * basisFuncsU[i] * basisFuncsV[j + 6];
                        }
                    }
                }
            }
            else throw new NotImplementedException();
        }


        public double[] EvaluateNormal(double u, double v)
        {
            if (Order0 == 3 && Order1 == 3)
            {
                double[] values = new double[18];

                EvaluateAllDerivatives(u, v, values);

                double[] normal = new double[Dimension];

                normal[0] = -values[4] * values[8] + values[5] * values[7];
                normal[1] = -values[5] * values[6] + values[3] * values[8];
                normal[2] = -values[3] * values[7] + values[4] * values[6];

                return normal;
            }

            var result = new double[Dimension];

            double delta = 0.0001;
            var p0 = Evaluate(u, v);
            var pU = Evaluate(u + delta, v);
            var pV = Evaluate(u, v + delta);

            var u1 = pU[0] - p0[0];
            var u2 = pU[1] - p0[1];
            var u3 = pU[2] - p0[2];

            var v1 = pV[0] - p0[0];
            var v2 = pV[1] - p0[1];
            var v3 = pV[2] - p0[2];

            result[0] = -u2 * v3 + u3 * v2;
            result[1] = -u3 * v1 + u1 * v3;
            result[2] = -u1 * v2 + u2 * v1;

            return result;
        }



        public NurbsSurface InsertKnot(int direction, double t)
        {
            if (direction == 0) return InsertKnot(t, ref knotArray0, Order0);
            else return InsertKnot(t, ref knotArray1, Order1);
        }

        private NurbsSurface InsertKnot(double t, ref double[] knotArray, int order)
        {
            double[] newKnot = new double[knotArray.Length + 1];
            for(int i=0, j=0; i < knotArray.Length; i++)
            {

            }
        }


        public NurbsSurface Clone()
        {
            //var result = new NurbsSurface(Dimension, IsRational, Order0, Order1, CvCount0, CvCount1);
            //Array.Copy(cvArray, result.cvArray, cvArray.Length);
            var result = new NurbsSurface(Dimension, IsRational, Order0, Order1, controlPoints.CvCount[0], controlPoints.CvCount[1]);
            Array.Copy(knotArray0, result.knotArray0, knotArray0.Length);
            Array.Copy(knotArray1, result.knotArray1, knotArray1.Length);
            return result;
        }
    }




}
