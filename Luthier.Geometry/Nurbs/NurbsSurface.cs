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
        [XmlElement]
        public int CvCount0;
        [XmlElement]
        public int CvCount1;
        [XmlArray]
        public double[] cvArray;
        [XmlArray]
        public double[] knotArray0;
        [XmlArray]
        public double[] knotArray1;

        public NurbsSurface() { }
        public NurbsSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            Dimension = dimension;
            IsRational = bIsRational;
            Order0 = order0;
            Order1 = order1;
            CvCount0 = cv_count0;
            CvCount1 = cv_count1;

            InitialiseArrays();
        }

        private void InitialiseArrays()
        {
            knotArray0 = new double[CvCount0 + Order0];
            knotArray1 = new double[CvCount1 + Order1];
            cvArray = new double[CvSize * CvCount0 * CvCount1];
        }

        public int CvSize => (IsRational) ? Dimension + 1 : Dimension;

        public Interval Domain0()
        {
            double min = knotArray0[Order0 - 2];
            double max = knotArray0[knotArray0.Length - Order0 + 1];
            return new Interval(min, max);
        }
        public Interval Domain1()
        {
            double min = knotArray1[Order1 - 2];
            double max = knotArray1[knotArray1.Length - Order1 + 1];
            return new Interval(min, max);
        }

        public double[] GetCV(int i, int j)
        {
            double[] cv = new double[CvSize];
            int startIndex = (CvCount1 * i + j) * CvSize;
            Array.Copy(cvArray, startIndex, cv, 0, CvSize);
            return cv;
        }

        public void SetCV(int i, int j, double[] cv)
        {
            int startIndex = (CvCount1 * i + j) * CvSize;
            Array.Copy(cv, 0, cvArray, startIndex, CvSize);
        }

        public double[] Evaluate(double u, double v)
        {
            var result = new double[Dimension];

            int knotIU = Nurbs.Algorithm.Find_Knot_Span(Order0 - 1, knotArray0, u);
            int knotIV = Nurbs.Algorithm.Find_Knot_Span(Order1 - 1, knotArray1, v);
            int cvIX = ((knotIU - Order0 + 1) * CvCount1 + knotIV - Order1 + 1) * CvSize;
            int cvStrideU = CvCount1 * CvSize;
            int cvStrideV = CvSize;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = Nurbs.Algorithm.SurfaceSpan_Evaluate_Deboor(Order0, Order1, knotIU, knotIV, ref knotArray0, ref knotArray1, ref cvArray, cvIX, cvStrideU, cvStrideV, u, v);
                cvIX++;
            }
            return result;
        }

        public double[] EvaluateNormal(double u, double v)
        {
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


        public NurbsSurface Clone()
        {
            var result = new NurbsSurface(Dimension, IsRational, Order0, Order1, CvCount0, CvCount1);
            Array.Copy(cvArray, result.cvArray, cvArray.Length);
            Array.Copy(knotArray0, result.knotArray0, knotArray0.Length);
            Array.Copy(knotArray1, result.knotArray1, knotArray1.Length);
            return result;
        }
    }




}
