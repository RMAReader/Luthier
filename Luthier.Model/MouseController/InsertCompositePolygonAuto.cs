using Luthier.Geometry;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController
{
    public class InsertCompositePolygonAuto : PanZoomMouseWheel
    {
        private readonly CompositePolygonFactory cfactory;
        private readonly IntersectionFactory ifactory;

        private double selectionRadius;
        private GraphicCompositePolygon polygon;
        private bool inProgress = false;

        public InsertCompositePolygonAuto(CompositePolygonFactory cfactory, IntersectionFactory ifactory, double selectionRadius)
        {
            this.cfactory = cfactory;
            this.ifactory = ifactory;
            this.selectionRadius = selectionRadius;

            ViewMapper = new ViewMapper2D();
        }


        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            PointF r = ViewMapper.TransformViewToModelCoordinates(new PointF(x, (float)(y + selectionRadius)));

            var intersection = CreateIntersection(p, r);
            if (intersection != null)
            {
                if (inProgress)
                {
                    cfactory.AddIntersection(polygon, intersection);
                }
                else
                {
                    polygon = cfactory.New();
                    cfactory.AddIntersection(polygon, intersection);
                    inProgress = true;
                }
            }
        }


        public override void MouseRightButtonDown(int x, int y)
        {
            inProgress = false;
        }

 
        private GraphicIntersection CreateIntersection(PointF p, PointF r)
        {
            var curves = GetNearestIntesectingObjects(p.X, p.Y);
            if (curves != null)
            {
                var intersection = ifactory.New(p.X, p.Y);
                ifactory.SetRadius(intersection, r.X, r.Y);
                intersection.Object1 = curves.Item1;
                intersection.Object2 = curves.Item2;
                return intersection;
            }
            return null;
        }


        private Tuple<UniqueKey, UniqueKey> GetNearestIntesectingObjects(double x, double y)
        {
            var range = (double)Math.Sqrt(selectionRadius * selectionRadius * ViewMapper.Scale * ViewMapper.Scale);
            var curves = ifactory.GetModel().Model.VisibleObjects()
                .Where(o => o is GraphicBSplineCurve)
                .Select(o => new { o.Key, Curve = ((GraphicBSplineCurve)o).ToPrimitive(ifactory.GetModel()) }).ToList();

            double minDistance = double.MaxValue;
            var centre = new Point2D(x, y);
            Tuple<UniqueKey, UniqueKey> result = null;
            for (int i = 0; i < curves.Count; i++)
            {
                for (int j = i + 1; j < curves.Count; j++)
                {
                    var intersect = Intersection.GetIntersection(curves[i].Curve, curves[j].Curve, centre, 0.01);
                    if (intersect != null && (centre - intersect.Point).L2Norm() < minDistance)
                    {
                        minDistance = (centre - intersect.Point).L2Norm();
                        result = new Tuple<UniqueKey, UniqueKey>(curves[i].Key, curves[j].Key);
                    }
                }
            }
            return result;
        }
    }
}
