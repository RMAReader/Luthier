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
        public UniqueKey[] cvArray;
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


        public UniqueKey GetCV(int i, int j)
        {
            return cvArray[cv_count1 * i + j];
        }

        public NurbsSurface ToPrimitive(IApplicationDocumentModel model)
        {
            var points = cvArray.Select(x => ((GraphicPoint2D)model.Objects()[x]).ToPrimitive()).ToList();
            var surface = new NurbsSurface(dimension,bIsRational,order0,order1,cv_count0,cv_count1);

            for (int i = 0; i < surface.KnotCount(0); i++) surface.SetKnot(0, i, knotArray0[i]);
            for (int i = 0; i < surface.KnotCount(1); i++) surface.SetKnot(1, i, knotArray1[i]);
            for (int i = 0; i < cv_count0; i++)
            {
                for (int j = 0; j < cv_count1; j++)
                {
                    var point = new double[2] { points[i * cv_count0 + j].x , points[i * cv_count0 + j].y };
                    surface.SetCV(i, j, 3, point);
                }
            }
            return surface;
        }


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }
    }
}
