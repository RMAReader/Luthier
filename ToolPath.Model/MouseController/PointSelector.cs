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
        private List<GraphicPoint2D> selectedPoints;

        public PointSelector(IApplicationDocumentModel model, double selectionRadius)
        {
            this.model = model;
            this.selectionRadius = selectionRadius;
            selectedPoints = new List<GraphicPoint2D>();
        }



        public override void MouseLeftButtonDown(int x, int y)
        {
            double distance = float.MaxValue;
            foreach (GraphicPoint2D point in model.Objects().Values.Where(p => p is GraphicPoint2D))
            {
                PointF p = ViewMapper.TransformModelToViewCoordinates(new PointF((float)point.X, (float)point.Y));
                double d = (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
                if (d < selectionRadius * selectionRadius && d < distance)
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
            }
        }

        public override void MouseLeftButtonUp(int x, int y)
        {
            selectedPoints.Clear();
        }

    }
}
