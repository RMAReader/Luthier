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
    public class PointSelector : PanZoomMouseWheel
    {
        private readonly IApplicationDocumentModel model;
        private double selectionRadius;
        private List<IDraggable2d> selectedPoints;

        public PointSelector(IApplicationDocumentModel model, double selectionRadius)
        {
            this.model = model;
            this.selectionRadius = selectionRadius;
            selectedPoints = new List<IDraggable2d>();
        }



        public override void MouseLeftButtonDown(int x, int y)
        {
            double distance = float.MaxValue;
            foreach (IDraggable2d point in model.Model.GetDraggableObjects2d())
            {
                PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
                double d = point.GetDistance(p.X, p.Y) * ViewMapper.Scale;
                if (d < selectionRadius && d < distance)
                {
                    selectedPoints.Clear();
                    selectedPoints.Add(point);
                    distance = d;
                }
            }
        }



        public override void OnMouseMove(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            foreach (var point in selectedPoints)
            {
                point.Set(p.X, p.Y);
                model.Model.HasChanged = true;
            }
        }

        public override void MouseLeftButtonUp(int x, int y)
        {
            selectedPoints.Clear();
            model.Model.HasChanged = true;
        }

    }
}
