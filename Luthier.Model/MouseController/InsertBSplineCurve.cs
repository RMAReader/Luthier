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
    public class InsertBSplineCurve : PanZoomMouseWheel
    {
        private readonly BSplineFactory factory;

        private GraphicPoint2D point;
        private GraphicBSplineCurve curve;

        public InsertBSplineCurve(BSplineFactory factory) { this.factory = factory; }
        public bool curveInProgress { get => (curve != null); }

        public override void Close()
        {
            if (curveInProgress)
            {
                //factory.DeletePoint(curve, point);
                //if (curve.pointsKeys.Count < 2) factory.DeleteLine(curve);
            }
            curve = null;
            point = null;
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (curveInProgress)
            {
                point.Set(p.X, p.Y);
                point = factory.AppendPoint(curve, p.X, p.Y);
            }
            else
            {
                curve = factory.New(2);
                factory.AppendPoint(curve, p.X, p.Y);
                point = factory.AppendPoint(curve, p.X, p.Y);
            }
        }

        public override void OnMouseMove(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (curveInProgress) point.Set(p.X, p.Y);
        }

        public override void MouseRightButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (curveInProgress) point.Set(p.X, p.Y);
            curve = null;
            point = null;
        }

    }
}
