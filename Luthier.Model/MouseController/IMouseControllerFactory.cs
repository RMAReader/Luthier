using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController
{
    public interface IMouseControllerFactory
    {
        InsertLinkedLine InserLinekedLine();
        InsertBSplineCurve InsertBSplineCurv();
        InsertSurface InsertSurface();
        InsertPoint InsertPoint();
        InsertPolygon InsertPolygon();
        InsertImage InsertImage();
        InsertLengthGauge InsertLengthGauge();
        InsertIntersectionAuto InsertIntersectionAuto(int range);
        InsertIntersectionManual InsertIntersectionManual(int range);
        InsertCompositePolygon InsertCompositePolygon(int range);
        InsertCompositePolygonAuto InsertCompositePolygonAuto(int range);
        PointSelector PointSelector(int range);
        PolygonSelector PolygonSelector(int range);
    }
}
