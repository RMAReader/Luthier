using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController
{
    public class MouseControllerFactory : IMouseControllerFactory
    {
        private ApplicationDocumentModel model;
        public MouseControllerFactory(ApplicationDocumentModel model)
        {
            this.model = model;
        }

        public InsertLinkedLine InserLinekedLine() => new InsertLinkedLine(model.LinkedLine2DFactory());

        public InsertBSplineCurve InsertBSplineCurv() => new InsertBSplineCurve(model.BSplineFactory());

        public InsertPoint InsertPoint() => new InsertPoint(model.Point2DFactory());
        
        public InsertPolygon InsertPolygon() => new InsertPolygon(model.Polygon2DFactory());

        public InsertImage InsertImage() => new InsertImage(model.ImageFactory());

        public InsertIntersectionAuto InsertIntersectionAuto(int range) => new InsertIntersectionAuto(model.IntersectionFactory(), range);

        public InsertIntersectionManual InsertIntersectionManual(int range) => new InsertIntersectionManual(model.IntersectionFactory(), range);

        public PointSelector PointSelector(int range) => new PointSelector(model, range);
        
        public PolygonSelector PolygonSelector(int range) => new PolygonSelector(model, range);

        //public PanAndZoom PanAndZoom() => new PanAndZoom();

        public InsertLengthGauge InsertLengthGauge() => new InsertLengthGauge(model.LengthGaugeFactory());

        public InsertCompositePolygon InsertCompositePolygon(int range) => new InsertCompositePolygon(model.CompositePolygonFactory(), range);

        public InsertCompositePolygonAuto InsertCompositePolygonAuto(int range) => new InsertCompositePolygonAuto(model.CompositePolygonFactory(), model.IntersectionFactory(), range);
    }
}
