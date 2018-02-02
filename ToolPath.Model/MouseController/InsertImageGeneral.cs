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
 
    public class InsertImageGeneral : PanZoomMouseWheel
    {
        private GraphicImageFactory factory;
        private InsertImageState state;
        private GraphicImage image;


        public InsertImageGeneral(GraphicImageFactory factory)
        {
            this.factory = factory;
            state = InsertImageState.Point1Pending;
        }

        public override void Close()
        {
            if (state != InsertImageState.Point1Pending)
            {
                factory.Delete(image);
            }
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            switch (state)
            {
                case InsertImageState.Point1Pending:
                    image = factory.New(p.X, p.Y);
                    factory.SetPoint(image, 0, p.X, p.Y);
                    state = InsertImageState.Point2Pending;
                    break;

                case InsertImageState.Point2Pending:
                    factory.SetPoint(image, 1, p.X, p.Y);
                    state = InsertImageState.Point3Pending;
                    break;

                case InsertImageState.Point3Pending:
                    factory.SetPoint(image, 2, p.X, p.Y);
                    state = InsertImageState.Point1Pending;
                    break;

            }
        }


        public override void OnMouseMove(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            switch (state)
            {
                case InsertImageState.Point1Pending:
                    break;

                case InsertImageState.Point2Pending:
                    factory.SetPoint(image, 1, p.X, p.Y);
                    break;

                case InsertImageState.Point3Pending:
                    factory.SetPoint(image, 2, p.X, p.Y);
                    break;
            }
        }

    }
}
