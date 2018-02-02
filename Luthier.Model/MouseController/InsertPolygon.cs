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
    public class InsertPolygon : PanZoomMouseWheel
    {
        private readonly IPolygon2DFactory factory;

        private GraphicPoint2D point;
        private GraphicPolygon2D polygon;

        public InsertPolygon(IPolygon2DFactory factory) { this.factory = factory; }

        public bool lineInProgress { get => (polygon != null); }

        public override void Close()
        {
            if (lineInProgress)
            {
                factory.DeletePoint(polygon, point);
                if (polygon.pointsKeys.Count < 2) factory.Delete(polygon);
            }
            polygon = null;
            point = null;
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (lineInProgress)
            {
                point.Set(p.X, p.Y);
                point = factory.AppendPoint(polygon, p.X, p.Y);
            }
            else
            {
                polygon = factory.New();
                factory.AppendPoint(polygon, p.X, p.Y);
                point = factory.AppendPoint(polygon, p.X, p.Y);
            }
        }

        public override void OnMouseMove(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (lineInProgress) point.Set(p.X, p.Y);
        }


        public override void MouseRightButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (lineInProgress) point.Set(p.X, p.Y);
            polygon = null;
            point = null;
        }

    }
}
