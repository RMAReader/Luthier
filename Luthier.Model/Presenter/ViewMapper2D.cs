using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Presenter
{
    public class ViewMapper2D
    {
        public PointF Origin { get; set; }
        public double Scale { get; set; }
        private RectangleF ClipBounds { get; set; }

        public ViewMapper2D()
        {
            Origin = new PointF(0, 0);
            Scale = 1;
        }

        public ViewMapper2D Copy() => new ViewMapper2D
        {
            Origin = new PointF(Origin.X, Origin.Y),
            Scale = Scale,
            ClipBounds = ClipBounds,
        };

        public ViewMapper2D ZoomFixedPoint(PointF viewPoint, double factor)
        {
            return new ViewMapper2D
            {
                Scale = Scale * factor,
                Origin = new PointF((float)(Origin.X + viewPoint.X / Scale - viewPoint.X / (Scale * factor)),
                                           (float)(Origin.Y + viewPoint.Y / Scale - viewPoint.Y / (Scale * factor)))
            };
        }
        //public void ZoomFixedPoint(PointF viewPoint, double factor)
        //{
        //    Scale = Scale * factor;
        //    Origin = new PointF((float)(Origin.X + viewPoint.X / Scale - viewPoint.X / (Scale * factor)),
        //                               (float)(Origin.Y + viewPoint.Y / Scale - viewPoint.Y / (Scale * factor)));

        //}


        public ViewMapper2D PanView(PointF fromViewPoint, PointF toViewPoint)
        {
            var from = TransformViewToModelCoordinates(fromViewPoint);
            var to = TransformViewToModelCoordinates(toViewPoint);

            return new ViewMapper2D
            {
                Scale = Scale,
                Origin = new PointF(Origin.X + from.X - to.X, Origin.Y + from.Y - to.Y)
            };
        }

        public void CopyPropertyValues(ViewMapper2D source)
        {
            Scale = source.Scale;
            Origin = source.Origin;
            ClipBounds = source.ClipBounds;
        }

        public PointF TransformModelToViewCoordinates(PointF p) => new PointF((float)((p.X - Origin.X) * Scale), (float)((p.Y - Origin.Y) * Scale));
        public PointF[] TransformModelToViewCoordinates(PointF[] p) => p.Select(x => TransformModelToViewCoordinates(x)).ToArray();

        public PointF TransformViewToModelCoordinates(PointF p) => new PointF((float)(p.X / Scale + Origin.X), (float)(p.Y / Scale + Origin.Y));
        public PointF[] TransformViewToModelCoordinates(PointF[] p) => p.Select(x => TransformViewToModelCoordinates(x)).ToArray();


    }
}
