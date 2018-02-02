using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjectFactory
{
    public class BSplineFactory
    {
        private ApplicationDocumentModel data;

        public BSplineFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public GraphicBSplineCurve New(int p)
        {
            var obj = new GraphicBSplineCurve(p);
            data.objects.Add(obj);
            return obj;
        }

        public GraphicPoint2D AppendPoint(GraphicBSplineCurve curve, double x, double y)
        {
            var point = data.Point2DFactory().New(x, y);
            curve.AddPoint(point);
            return point;
        }
    }
}
