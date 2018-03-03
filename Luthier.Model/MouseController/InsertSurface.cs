using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController
{
    public class InsertSurface : PanZoomMouseWheel
    {
        private BSplineFactory factory;
        public bool inProgress;
        private GraphicNurbSurface surface;
        public PointF firstPoint;
        public PointF secondPoint;

        public InsertSurface(BSplineFactory factory)
        {
            this.factory = factory;
            inProgress=false;
        }

            public override void Close()
            {
                //if (state != InsertImageState.Point1Pending)
                //{
                //    factory.Delete(image);
                //}
            }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            switch (inProgress)
            {
                case false:
                    inProgress = true;
                    firstPoint = p;
                    break;

                case true:
                    factory.CreateSurface(5, 5, Math.Min(firstPoint.X, p.X), Math.Min(firstPoint.Y, p.Y), Math.Max(firstPoint.X, p.X), Math.Max(firstPoint.Y, p.Y));
                    inProgress = false;
                    break;
            }
        }

        public override void OnMouseMove(int x, int y)
        {
            secondPoint = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
        }

    }
}
