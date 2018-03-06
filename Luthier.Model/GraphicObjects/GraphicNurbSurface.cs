using Luthier.Geometry.BSpline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicNurbSurface : GraphicObjectBase
    {
        [XmlElement]
        public int dimension;
        [XmlElement]
        public bool bIsRational;
        [XmlElement]
        public int order0;
        [XmlElement]
        public int order1;
        [XmlElement]
        public int cv_count0;
        [XmlElement]
        public int cv_count1;
        [XmlArray]
        public double[] cvArray;
        [XmlArray]
        public double[] knotArray0;
        [XmlArray]
        public double[] knotArray1;

        public GraphicNurbSurface()  { }
        public GraphicNurbSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            this.dimension = dimension;
            this.bIsRational = bIsRational;
            this.order0 = order0;
            this.order1 = order1;
            this.cv_count0 = cv_count0;
            this.cv_count1 = cv_count1;
        }


        public double[] GetCV(int i, int j)
        {
            double[] cv = new double[dimension];
            int startIndex = (cv_count1 * i + j) * dimension;
            Array.Copy(cvArray, startIndex, cv, 0, dimension);
            return cv;
        }

        public void SetCV(int i, int j, double[] cv)
        {
            int startIndex = (cv_count1 * i + j) * dimension;
            Array.Copy(cv, 0, cvArray, startIndex, dimension);
        }

        public NurbsSurface ToPrimitive(IApplicationDocumentModel model)
        {
            var surface = new NurbsSurface(dimension,bIsRational,order0,order1,cv_count0,cv_count1);

            for (int i = 0; i < surface.KnotCount(0); i++) surface.SetKnot(0, i, knotArray0[i]);
            for (int i = 0; i < surface.KnotCount(1); i++) surface.SetKnot(1, i, knotArray1[i]);
            for (int i = 0; i < cv_count0; i++)
            {
                for (int j = 0; j < cv_count1; j++)
                {
                    surface.SetCV(i, j, GetCV(i,j));
                }
            }
            return surface;
        }

        public IEnumerable<IDraggable> GetDraggableObjects()
        {
            for (int i = 0; i < cv_count0; i++)
            {
                for (int j = 0; j < cv_count1; j++)
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

        public double GetDistance(double x, double y)
        {
            double cvx = surface.GetCV(i, j)[0];
            double cvy = surface.GetCV(i, j)[1];
            return Math.Sqrt((cvx - x) * (cvx - x) + (cvy - y) * (cvy - y)); ;
        }

        public void Set(double x, double y)
        {
            double[] cv = new double[surface.dimension];
            cv[0] = x;
            cv[1] = y;
            surface.SetCV(i, j, cv);
        }
    }
}
