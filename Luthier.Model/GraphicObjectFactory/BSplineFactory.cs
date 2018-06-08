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

        public GraphicNurbSurface CreateSurface(int cv_count0, int cv_count1, double minx, double miny, double maxx, double maxy)
        {
            data.Model.HasChanged = true;

            var points = new List<double>();
            for (int i = 0; i < cv_count0; i++)
            {
                double x = (1 - (double)i / (cv_count0 - 1)) * minx + ((double)i / (cv_count0 - 1)) * maxx;
                for (int j = 0; j < cv_count1; j++)
                {
                    double y = (1 - (double)j / (cv_count1 - 1)) * miny + ((double)j / (cv_count1 - 1)) * maxy;
                    double z = (x * x + y * y) / 1000;

                    points.AddRange(new double[] { x, y, z });
                }
            }
            var s = new GraphicNurbSurface(3, false, 3, 3, cv_count0, cv_count1);
            s.cvArray = points.ToArray();

            var knot0 = new List<double>();
            for (int i = 0; i < cv_count0 + 1; i++) knot0.Add(i);
            var knot1 = new List<double>();
            for (int i = 0; i < cv_count1 + 1; i++) knot1.Add(i);

            s.knotArray0 = knot0.ToArray();
            s.knotArray1 = knot1.ToArray();

            data.objects.Add(s);
            return s;
        }
    }
}
