using Luthier.Geometry.Nurbs;
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
            data.Model.Add(obj);
            return obj;
        }

        public GraphicPoint2D AppendPoint(GraphicBSplineCurve curve, double x, double y)
        {
            var point = data.Point2DFactory().New(x, y);
            curve.AddPoint(point);
            return point;
        }

        public GraphicNurbsSurface CreateSurface(int cv_count0, int cv_count1, double minx, double miny, double maxx, double maxy)
        {

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
            var s = new GraphicNurbsSurface(3, false, 3, 3, cv_count0, cv_count1);
            s.cvArray = points.ToArray();

            var knot0 = new List<double>();
            for (int i = 0; i < cv_count0 + 1; i++) knot0.Add(i);
            var knot1 = new List<double>();
            for (int i = 0; i < cv_count1 + 1; i++) knot1.Add(i);

            s.knotArray0 = knot0.ToArray();
            s.knotArray1 = knot1.ToArray();

            data.Model.Add(s);
            return s;
        }


        public static GraphicNurbsSurface CreateSurface(int cv_count0, int cv_count1, double[] corner1, double[] corner2, double[] corner3)
        {
            var s = new GraphicNurbsSurface(3, false, 3, 3, cv_count0, cv_count1);

            for (int u = 0; u < cv_count0; u++)
            {
                double du = ((double)u) / (cv_count0 - 1);
                for (int v = 0; v < cv_count1; v++)
                {
                    double dv = ((double)v) / (cv_count1 - 1);

                    var cv = new double[3];
                    for(int i = 0; i < cv.Length; i++)
                    {
                        cv[i] = (1 - du - dv) * corner1[i] + du * corner2[i] + dv * corner3[i];
                    };

                    s.SetCV(u, v, cv);
                }
            }

            for (int i = 0; i < s.knotArray0.Length; i++) s.knotArray0[i] = i;
            for (int i = 0; i < s.knotArray1.Length; i++) s.knotArray1[i] = i;

            return s;
           
        }
    }
}
