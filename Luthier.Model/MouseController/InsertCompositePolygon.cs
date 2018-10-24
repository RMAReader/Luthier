using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.Presenter;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using System.Drawing;

namespace Luthier.Model.MouseController
{
    public class InsertCompositePolygon : PanZoomMouseWheel
    {
        private CompositePolygonFactory factory;
        private Boolean InProgress = false;
        private double selectionRadius;
        private GraphicCompositePolygon polygon;

        public InsertCompositePolygon(CompositePolygonFactory factory, double selectionRadius)
        {
            this.factory = factory;
            this.selectionRadius = selectionRadius;
        }


        public override void MouseLeftButtonDown(int x, int y)
        {
            if(InProgress)
            {
                var intersection = GetNearestIntersection(x, y);
                if (intersection != null) factory.AddIntersection(polygon, intersection);
            }
            else
            {
                var intersection = GetNearestIntersection(x, y);
                if (intersection != null)
                {
                    polygon = factory.New();
                    factory.AddIntersection(polygon, intersection);
                    InProgress = true;
                }
            }
        }


        public override void MouseRightButtonDown(int x, int y)
        {
            if (InProgress)
            {
                if (polygon.Junctions.Count < 3) { }
            }
            else
            {
                InProgress = false;
            }
        }


        private GraphicIntersection GetNearestIntersection(double x, double y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF((float)x, (float)y));
            var range = (double)Math.Sqrt(selectionRadius * selectionRadius * ViewMapper.Scale * ViewMapper.Scale);
            var nearestObj = factory.GetModel().Model.VisibleObjects()
                .Where(o => o is GraphicIntersection)
                .Select(o => new { Distance = o.GetDistance(factory.GetModel(), p.X, p.Y), Object = o })
                .Where(o => o.Distance < range)
                .OrderBy(o => o.Distance)
                .FirstOrDefault();

            return (nearestObj == null) ? null : (GraphicIntersection)nearestObj.Object;
        }
    }
}
