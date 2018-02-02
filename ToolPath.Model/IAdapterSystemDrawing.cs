using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Luthier.Model.MouseController;
using Luthier.Model.GraphicObjects;

namespace Luthier.Model
{
    public interface IAdapterSystemDrawing
    {
        IEnumerable<PointF[]> GetLinkedLine2DPoints();
        IEnumerable<PointF[]> GetCurveControlPoints();
        IEnumerable<PointF[]> GetCurves();
        IEnumerable<PointF[]> GetPolygon2DPoints();
        IEnumerable<PointF[]> GetSelectedPolygon2DPoints(PolygonSelector selector);
        IEnumerable<PointF[]> GetSelectedPolygon2DPoints(List<UniqueKey> keys);
        IEnumerable<IntersectionData> GetIntersections();
        IEnumerable<PointF[]> GetCompositePolygons();
        IEnumerable<ImageData> GetImages();
        IEnumerable<LengthGaugeData> GetLengthGauges();
    }


    public class ImageData
    {
        public Image image;
        public PointF[] points;

        public ImageData()
        {
            points = new PointF[3];
        }
    }

    public class LengthGaugeData
    {
        public PointF P1;
        public PointF P2;
        public double Length;
    }

    public class IntersectionData
    {
        public PointF Centre;
        public PointF Radius;
    }
}
