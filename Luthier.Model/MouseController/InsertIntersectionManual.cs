using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController
{
 
    public enum InsertIntersectinState
    {
        CentrePending,
        RadiusPending,
        Object1Pending,
        Object2Pending
    }

    public class InsertIntersectionManual : PanZoomMouseWheel
    {
        private readonly IntersectionFactory factory;
        private GraphicIntersection intersection;
        public InsertIntersectinState State = InsertIntersectinState.CentrePending;
        public double selectionRadius;

        public InsertIntersectionManual(IntersectionFactory factory, double selectionRadius)
        {
            this.factory = factory;
            this.selectionRadius = selectionRadius;
        }


        public override void Close()
        {
            if (State != InsertIntersectinState.CentrePending)
            {
                factory.Delete(intersection);
            }
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            PointF r = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y + 20));
            switch (State)
            {
                case InsertIntersectinState.CentrePending:
                    intersection = factory.New(p.X, p.Y);
                    factory.SetRadius(intersection, r.X, r.Y);
                    //State = InsertIntersectinState.RadiusPending;
                    State = InsertIntersectinState.Object1Pending;
                    break;

                //case InsertIntersectinState.RadiusPending:
                //    factory.SetRadius(intersection, p.X, p.Y);
                //    State = InsertIntersectinState.Object1Pending;
                //    break;

                case InsertIntersectinState.Object1Pending:
                    intersection.Object1 = GetNearestObject(p.X, p.Y)?.Key;
                    State = InsertIntersectinState.Object2Pending;
                    break;

                case InsertIntersectinState.Object2Pending:
                    intersection.Object2 = GetNearestObject(p.X, p.Y)?.Key;
                    State = InsertIntersectinState.CentrePending;
                    break;
            }
          
        }


        public override void OnMouseMove(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            switch (State)
            {
                case InsertIntersectinState.CentrePending:
                    break;

                case InsertIntersectinState.RadiusPending:
                    factory.SetRadius(intersection, p.X, p.Y);
                    break;

                case InsertIntersectinState.Object1Pending:
                    break;

                case InsertIntersectinState.Object2Pending:
                    break;
            }
        }

       
        private GraphicObjectBase GetNearestObject(double x, double y)
        {
            //PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF((float)x, (float)y));
            var range = (double)Math.Sqrt(selectionRadius * selectionRadius * ViewMapper.Scale * ViewMapper.Scale);
            var nearestObj = factory.GetModel().objects
                .Select(o => new { Distance = o.GetDistance(factory.GetModel(), x, y), Object = o })
                .Where(o => o.Distance < range && (o.Object is GraphicPoint2D)==false)
                .OrderBy(o => o.Distance)
                .FirstOrDefault();

            return (nearestObj==null) ? null : nearestObj.Object;
        }
    }
}
