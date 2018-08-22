using Luthier.Geometry;
using Luthier.Geometry.BSpline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Algorithm = Luthier.Geometry.BSpline.Algorithm;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicNurbSurface : GraphicObjectBase
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

        public GraphicNurbSurface()  { }
        public GraphicNurbSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            Dimension = dimension;
            IsRational = bIsRational;
            Order0 = order0;
            Order1 = order1;
            CvCount0 = cv_count0;
            CvCount1 = cv_count1;

            InitialiseArrays();
        }

        private void  InitialiseArrays()
        {
            knotArray0 = new double[CvCount0 + Order0 - 2];
            knotArray1 = new double[CvCount1 + Order1 - 2];
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

            int knotIU = Algorithm.Find_Knot_Span(Order0 - 1, knotArray0, u);
            int knotIV = Algorithm.Find_Knot_Span(Order1 - 1, knotArray1, v);
            int cvIX = ((knotIU - Order0 + 2) * CvCount1 + knotIV - Order1 + 2) * CvSize;
            int cvStrideU = CvCount1 * CvSize;
            int cvStrideV = CvSize;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = Algorithm.Evaluate_SurfaceSpan_Deboor(Order0, Order1, knotIU, knotIV, ref knotArray0, ref knotArray1, ref cvArray, cvIX, cvStrideU, cvStrideV, u, v);
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


        //public NurbsSurface ToPrimitive(IApplicationDocumentModel model)
        //{
        //    var surface = new NurbsSurface(Dimension,IsRational,Order0,Order1,CvCount0,CvCount1);

        //    for (int i = 0; i < surface.KnotCount(0); i++) surface.SetKnot(0, i, knotArray0[i]);
        //    for (int i = 0; i < surface.KnotCount(1); i++) surface.SetKnot(1, i, knotArray1[i]);
        //    for (int i = 0; i < CvCount0; i++)
        //    {
        //        for (int j = 0; j < CvCount1; j++)
        //        {
        //            surface.SetCV(i, j, GetCV(i,j));
        //        }
        //    }
        //    return surface;
        //}

        public IEnumerable<IDraggable2d> GetDraggableObjects2d()
        {
            for (int i = 0; i < CvCount0; i++)
            {
                for (int j = 0; j < CvCount1; j++)
                {
                    yield return new DraggableSurfaceCV2d(this, i, j);
                }
            }
        }

        public IEnumerable<IDraggable> GetDraggableObjects()
        {
            for (int i = 0; i < CvCount0; i++)
            {
                for (int j = 0; j < CvCount1; j++)
                {
                    yield return new DraggableSurfaceCV(this, i, j);
                }
            }
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }
    }


    public class DraggableSurfaceCV2d : IDraggable2d
    {
        private GraphicNurbSurface surface;
        private int i;
        private int j;

        public DraggableSurfaceCV2d(GraphicNurbSurface surface, int i, int j)
        {
            this.surface = surface;
            this.i = i;
            this.j = j;
        }

        public double GetDistance(double x, double y)
        {
            double cvx = surface.GetCV(i, j)[0];
            double cvy = surface.GetCV(i, j)[1];
            return Math.Sqrt((cvx - x) * (cvx - x) + (cvy - y) * (cvy - y)); ;
        }

        public void Set(double x, double y)
        {
            double[] cv = new double[surface.Dimension];
            cv[0] = x;
            cv[1] = y;
            surface.SetCV(i, j, cv);
        }
    }

    public class DraggableSurfaceCV : IDraggable
    {
        private GraphicNurbSurface surface;
        private int i;
        private int j;

        public DraggableSurfaceCV(GraphicNurbSurface surface, int i, int j)
        {
            this.surface = surface;
            this.i = i;
            this.j = j;
        }

        public double[] Values
        {
            get => surface.GetCV(i, j);
            set => surface.SetCV(i, j, value);
        }
    }
}
