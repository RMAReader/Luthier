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
    public class GraphicBSplineCurve : GraphicObjectBase
    {
        [XmlArray]
        public List<UniqueKey> pointsKeys;
        [XmlElement]
        public Geometry.BSpline.Knot knot;

        public GraphicBSplineCurve() { }

        public GraphicBSplineCurve(int p)
        {
            pointsKeys = new List<UniqueKey>();
            knot = Knot.CreateUniformOpen(p, p + 1);
        }
        public void AddPoint(GraphicPoint2D p)
        {
            pointsKeys.Add(p.Key);
            knot = Knot.CreateUniformOpen(knot.p, pointsKeys.Count + knot.p + 1);
        }

        public Knot GetKnot() => knot;

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return model.objects
                .Where(o => pointsKeys.Contains(o.Key))
                .Select(o => o.GetDistance(model,x,y))
                .OrderBy(o => o)
                .FirstOrDefault();
        }

        public Geometry.BSpline.NurbsCurve ToPrimitive(IApplicationDocumentModel model)
        {
            var points = pointsKeys.Select(x => ((GraphicPoint2D)model.Objects()[x]).ToPrimitive()).ToList();
            return new Geometry.BSpline.NurbsCurve(points, knot);
        }
    }
}
