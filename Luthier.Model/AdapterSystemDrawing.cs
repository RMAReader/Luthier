using Luthier.Core;
using Luthier.Geometry;
using Luthier.Geometry.BSpline;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Luthier.Geometry.Intersection;

namespace Luthier.Model
{
    public class AdapterSystemDrawing : IAdapterSystemDrawing
    {
        protected readonly IApplicationDocumentModel model;

        public AdapterSystemDrawing(IApplicationDocumentModel model)
        {
            this.model = model;
        }

        public IEnumerable<PointF[]> GetLinkedLine2DPoints()
        {
            var result = new List<PointF[]>();
            foreach (GraphicLinkedLine2D line in model.Objects().Values.Where(x => x is GraphicLinkedLine2D))
            {
                List<PointF> points = new List<PointF>();
                foreach(var pointKey in line.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D) model.Objects()[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetCurveControlPoints()
        {
            var result = new List<PointF[]>();
            foreach (GraphicBSplineCurve line in model.Objects().Values.Where(x => x is GraphicBSplineCurve))
            {
                List<PointF> points = new List<PointF>();
                foreach (var pointKey in line.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Objects()[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetCurves()
        {
            var result = new List<PointF[]>();
            foreach (GraphicBSplineCurve curve in model.Objects().Values.Where(x => x is GraphicBSplineCurve))
            {
                List<Point2D> points = new List<Point2D>();
                foreach (var pointKey in curve.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Objects()[pointKey];
                    points.Add(new Point2D(point.X, point.Y));
                }

                var approxCurve = new Geometry.BSpline.Curve(points, curve.GetKnot()).ToLines(100);

                result.Add(approxCurve.Select(p => new PointF((float)p.x, (float)p.y)).ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetPolygon2DPoints()
        {
            var result = new List<PointF[]>();
            foreach (GraphicPolygon2D polygon in model.Objects().Values.Where(x => x is GraphicPolygon2D))
            {
                List<PointF> points = new List<PointF>();
                foreach (var pointKey in polygon.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Objects()[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetSelectedPolygon2DPoints(PolygonSelector selector)
        {
            var result = new List<PointF[]>();
            foreach (GraphicPolygon2D polygon in selector.selectedPolygons)
            {
                List<PointF> points = new List<PointF>();
                foreach (var pointKey in polygon.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Objects()[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetSelectedPolygon2DPoints(List<UniqueKey> keys)
        {
            var result = new List<PointF[]>();
            foreach (var key in keys)
            {
                var polygon = model.Objects()[key] as GraphicPolygon2D;
                if (polygon == null) return result;

                List<PointF> points = new List<PointF>();
                foreach (var pointKey in polygon.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Objects()[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<ImageData> GetImages()
        {
            var result = new List<ImageData>();
            foreach (GraphicImage image in model.Objects().Values.Where(x => x is GraphicImage))
            {
                var data = new ImageData();
                for (int i = 0; i<3; i++)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Objects()[image.pointsKeys[i]];
                    data.points[i] = new PointF((float)point.X, (float)point.Y);
                }
                data.image = image.GetImage();
                result.Add(data);
            }
            return result;

        }

        public IEnumerable<LengthGaugeData> GetLengthGauges()
        {
            var result = new List<LengthGaugeData>();
            foreach (GraphicLengthGauge gauge in model.Objects().Values.Where(x => x is GraphicLengthGauge))
            {
                
                var p1 = (GraphicPoint2D)model.Objects()[gauge.fromPoint];
                var p2 = (GraphicPoint2D)model.Objects()[gauge.toPoint];
                var line = new LineSegment2D(new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y));
                result.Add(new LengthGaugeData()
                {
                    P1 = new PointF((float)p1.X, (float)p1.Y),
                    P2 = new PointF((float)p2.X, (float)p2.Y),
                    Length = line.Length()
                });
            }
            return result;
        }


        public IEnumerable<IntersectionData> GetIntersections()
        {
            var result = new List<IntersectionData>();
            foreach (GraphicIntersection intersection in model.Objects().Values.Where(x => x is GraphicIntersection))
            {
                GraphicPoint2D centre = (GraphicPoint2D)model.Objects()[intersection.Centre];
                GraphicPoint2D radius = (GraphicPoint2D)model.Objects()[intersection.Radius];
                result.Add(new IntersectionData()
                {
                    Centre = new PointF((float)centre.X, (float)centre.Y),
                    Radius = new PointF((float)radius.X, (float)radius.Y),
                });
            }
            return result;
        }

        public IEnumerable<PointF[]> GetCompositePolygons()
        {
            var result = new List<PointF[]>();
            foreach (GraphicCompositePolygon polygon in model.Objects().Values.Where(x => x is GraphicCompositePolygon))
            {
                result.Add(polygon.ToPolygon2D(model).GetPoints().Select(p => new PointF((float)p.x, (float)p.y)).ToArray());
            }
            return result;
        }

        
        

    }




    
}
