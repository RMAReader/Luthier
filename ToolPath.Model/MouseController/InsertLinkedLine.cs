using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using System.Drawing;

namespace Luthier.Model.MouseController
{
    public class InsertLinkedLine : PanZoomMouseWheel, IMouseController
    {
        private readonly ILinkedLine2DFactory factory;

        private GraphicPoint2D point;
        private GraphicLinkedLine2D line;

        public InsertLinkedLine(ILinkedLine2DFactory factory) { this.factory = factory; ViewMapper = new ViewMapper2D(); }

        public bool lineInProgress{ get => (line != null); }

        public override void Close()
        {
            if (lineInProgress)
            {
                factory.DeletePoint(line, point);
                if (line.pointsKeys.Count < 2) factory.DeleteLine(line);
            }
            line = null;
            point = null;
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (lineInProgress)
            {
                point.Set(p.X, p.Y);
                point = factory.AppendPoint(line, p.X, p.Y);
            }
            else
            {
                line = factory.New();
                factory.AppendPoint(line, p.X, p.Y);
                point = factory.AppendPoint(line, p.X, p.Y);
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
            line = null;
            point = null;
        }


    }
}
