using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Luthier.Core;
using Luthier.Geometry;
using Luthier.Geometry.BSpline;
using static Luthier.Geometry.Intersection;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicCompositePolygon : GraphicObjectBase, IPolygon2D
    {
        [XmlArray]
        public List<UniqueKey> Junctions;

        public GraphicCompositePolygon() => Junctions = new List<UniqueKey>();

        UniqueKey IPolygon2D.Key() => base.Key;

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return ToPolygon2D(model).Distance(new Point2D(x,y));
        }

        public Polygon2D ToPolygon2D(IApplicationDocumentModel model)
        {
            var points = new List<Point2D>();

            foreach (var pointKeyPair in Junctions.EnumeratePairsClosed())
            {
                    GraphicIntersection left = (GraphicIntersection)model.Model[pointKeyPair.Item1];
                    Point2D left_centre = ((GraphicPoint2D)model.Model[left.Centre]).ToPrimitive();
                    var left_curve1 = (left.Object1 == null) ? null : ((GraphicBSplineCurve)model.Model[left.Object1]).ToPrimitive(model);
                    var left_curve2 = (left.Object2 == null) ? null : ((GraphicBSplineCurve)model.Model[left.Object2]).ToPrimitive(model);
                    var left_intersect = Intersection.GetIntersection(left_curve1, left_curve2, left_centre, 0.001);

                    GraphicIntersection right = (GraphicIntersection)model.Model[pointKeyPair.Item2];
                    Point2D right_centre = ((GraphicPoint2D)model.Model[right.Centre]).ToPrimitive();
                    var right_curve1 = (right.Object1 == null) ? null : ((GraphicBSplineCurve)model.Model[right.Object1]).ToPrimitive(model);
                    var right_curve2 = (right.Object2 == null) ? null : ((GraphicBSplineCurve)model.Model[right.Object2]).ToPrimitive(model);
                    var right_intersect = Intersection.GetIntersection(right_curve1, right_curve2, right_centre, 0.001);

                    if (left_intersect != null && right_intersect != null)
                    {
                        var s = new CurveSegment(left_intersect, left.Object1, left.Object2, right_intersect, right.Object1, right.Object2);

                        if (s.curve != null)
                        {
                            points.AddRange(s.curve.ToLines(1000, s.from, s.to));
                        }
                        else
                        {
                            points.Add(left_centre);
                            points.Add(right_centre);
                        }
                    }
                    else if (left_intersect != null && right_intersect == null)
                    {
                        points.Add(right_centre);
                    }
                    else if (left_intersect == null && right_intersect != null)
                    {
                        points.Add(left_centre);
                    }
                    else
                    {
                        points.Add(left_centre);
                        points.Add(right_centre);
                    }
            }

            return new Polygon2D(points);
        }
    }


    class CurveSegment
    {
        public Geometry.BSpline.NurbsCurve curve;
        public double from;
        public double to;

        public CurveSegment(CurveIntersection left, UniqueKey left1, UniqueKey left2, CurveIntersection right, UniqueKey right1, UniqueKey right2)
        {
            if (left1 == right1)
            {
                curve = left.curve1;
                from = left.Parameter1;
                to = right.Parameter1;
            }
            else if (left1 == right2)
            {
                curve = left.curve1;
                from = left.Parameter1;
                to = right.Parameter2;
            }
            else if (left2 == right1)
            {
                curve = left.curve2;
                from = left.Parameter2;
                to = right.Parameter1;
            }
            else if (left2 == right2)
            {
                curve = left.curve2;
                from = left.Parameter2;
                to = right.Parameter2;
            }
        }
    }
}
