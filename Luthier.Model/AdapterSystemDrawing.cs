using Luthier.CncOperation;
using Luthier.Core;
using Luthier.Geometry;
using Luthier.Geometry.Nurbs;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using Luthier.Model.ToolPathSpecification;
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
            foreach (GraphicLinkedLine2D line in model.Model.VisibleObjects().Where(x => x is GraphicLinkedLine2D))
            {
                List<PointF> points = new List<PointF>();
                foreach(var pointKey in line.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D) model.Model[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetCurveControlPoints()
        {
            var result = new List<PointF[]>();
            foreach (GraphicBSplineCurve line in model.Model.VisibleObjects().Where(x => x is GraphicBSplineCurve))
            {
                List<PointF> points = new List<PointF>();
                foreach (var pointKey in line.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Model[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetCurves()
        {
            var result = new List<PointF[]>();
            foreach (GraphicBSplineCurve curve in model.Model.VisibleObjects().Where(x => x is GraphicBSplineCurve))
            {
                List<Point2D> points = new List<Point2D>();
                foreach (var pointKey in curve.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Model[pointKey];
                    points.Add(new Point2D(point.X, point.Y));
                }

                var approxCurve = new Geometry.Nurbs.BSplineCurve(points, curve.GetKnot()).ToLines(100);

                result.Add(approxCurve.Select(p => new PointF((float)p.x, (float)p.y)).ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetPolygon2DPoints()
        {
            var result = new List<PointF[]>();
            foreach (GraphicPolygon2D polygon in model.Model.VisibleObjects().Where(x => x is GraphicPolygon2D))
            {
                List<PointF> points = new List<PointF>();
                foreach (var pointKey in polygon.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Model[pointKey];
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
                    GraphicPoint2D point = (GraphicPoint2D)model.Model[pointKey];
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
                var polygon = model.Model[key] as GraphicPolygon2D;
                if (polygon == null) return result;

                List<PointF> points = new List<PointF>();
                foreach (var pointKey in polygon.pointsKeys)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Model[pointKey];
                    points.Add(new PointF((float)point.X, (float)point.Y));
                }
                result.Add(points.ToArray());
            }
            return result;
        }

        public IEnumerable<ImageData> GetImages()
        {
            var result = new List<ImageData>();
            foreach (GraphicImage2d image in model.Model.VisibleObjects().Where(x => x is GraphicImage2d))
            {
                var data = new ImageData();
                for (int i = 0; i<3; i++)
                {
                    GraphicPoint2D point = (GraphicPoint2D)model.Model[image.pointsKeys[i]];
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
            foreach (GraphicLengthGauge gauge in model.Model.VisibleObjects().Where(x => x is GraphicLengthGauge))
            {
                
                var p1 = (GraphicPoint2D)model.Model[gauge.fromPoint];
                var p2 = (GraphicPoint2D)model.Model[gauge.toPoint];
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
            foreach (GraphicIntersection intersection in model.Model.VisibleObjects().Where(x => x is GraphicIntersection))
            {
                GraphicPoint2D centre = (GraphicPoint2D)model.Model[intersection.Centre];
                GraphicPoint2D radius = (GraphicPoint2D)model.Model[intersection.Radius];
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
            foreach (GraphicCompositePolygon polygon in model.Model.VisibleObjects().Where(x => x is GraphicCompositePolygon))
            {
                result.Add(polygon.ToPolygon2D(model).GetPoints().Select(p => new PointF((float)p.x, (float)p.y)).ToArray());
            }
            return result;
        }

        public IEnumerable<PointF[]> GetToolPath()
        {
            var result = new List<PointF[]>();
            foreach (ToolPathSpecificationBase spec in model.Model.VisibleObjects().Where(x => x is ToolPathSpecificationBase))
            {
                if(spec.ToolPath != null)
                {
                    float currentX = 0;
                    float currentY = 0;
                    var currentPath = new List<PointF>();
                    foreach (var op in spec.ToolPath)
                    {
                        var move = op as MoveToPoint;
                        if (move != null)
                        {
                            currentX = (float)(move.GetX() ?? currentX);
                            currentY = (float)(move.GetY() ?? currentY);
                            currentPath.Add(new PointF(currentX, currentY));
                        }
                    }
                    result.Add(currentPath.ToArray());
                }
            }
            return result;
        }


        public IEnumerable<PointF[]> GetSurfaceControlPoints()
        {
            var result = new List<PointF[]>();
            foreach (GraphicNurbsSurface surface in model.Model.VisibleObjects().Where(x => x is GraphicNurbsSurface))
            {
                var net = new List<PointF>();
                for (int i = 0; i < surface.CvCount0; i++)
                {
                    for (int j = 0; j < surface.CvCount1 - 1; j++)
                    {
                        var p1 = surface.GetCV(i, j);
                        var p2 = surface.GetCV(i, j + 1);
                        net.Add(new PointF((float)p1[0], (float)p1[1]));
                        net.Add(new PointF((float)p2[0], (float)p2[1]));
                    }
                }
                for (int i = 0; i < surface.CvCount0 - 1; i++)
                {
                    for (int j = 0; j < surface.CvCount1; j++)
                    {
                        var p1 = surface.GetCV(i, j);
                        var p2 = surface.GetCV(i + 1, j);
                        net.Add(new PointF((float)p1[0], (float)p1[1]));
                        net.Add(new PointF((float)p2[0], (float)p2[1]));
                    }
                }
                result.Add(net.ToArray());
                //result.Add(surface.cvArray.Select(x => (GraphicPoint2D)model.Objects()[x]).Select(p => new PointF((float)p.X, (float)p.Y)).ToArray());
            }
            return result;
        }


    }





}
