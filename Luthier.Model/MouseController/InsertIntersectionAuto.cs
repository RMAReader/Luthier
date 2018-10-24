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
    public class InsertIntersectionAuto : PanZoomMouseWheel
    {
        private readonly IntersectionFactory factory;
        private GraphicIntersection intersection;
        public double selectionRadius;

        public InsertIntersectionAuto(IntersectionFactory factory, double selectionRadius)
        {
            this.factory = factory;
            this.selectionRadius = selectionRadius;
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            PointF r = ViewMapper.TransformViewToModelCoordinates(new PointF(x, (float)(y + selectionRadius)));

            var curves = GetNearestIntesectingObjects(p.X, p.Y);
            if (curves != null)
            {
                intersection = factory.New(p.X, p.Y);
                factory.SetRadius(intersection, r.X, r.Y);
                intersection.Object1 = curves.Item1;
                intersection.Object2 = curves.Item2;
            }
        }
    
     
        private Tuple<UniqueKey, UniqueKey> GetNearestIntesectingObjects(double x, double y)
        {
            var range = (double)Math.Sqrt(selectionRadius * selectionRadius * ViewMapper.Scale * ViewMapper.Scale);
            var curves = factory.GetModel().Model.VisibleObjects()
                .Where(o => o is GraphicBSplineCurve)
                .Select(o => new { o.Key, Curve = ((GraphicBSplineCurve)o).ToPrimitive(factory.GetModel())}).ToList();

            double minDistance = double.MaxValue;
            var centre = new Point2D(x, y);
            Tuple<UniqueKey, UniqueKey> result = null;
            for (int i = 0; i < curves.Count; i++)
            {
                for(int j = i + 1; j < curves.Count; j++)
                {
                    var intersect = Intersection.GetIntersection(curves[i].Curve, curves[j].Curve, centre , 0.01);
                    if(intersect != null && (centre - intersect.Point).L2Norm() < minDistance)
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
