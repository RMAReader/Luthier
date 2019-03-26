using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.Geometry;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecification
{
    public class MouldEdgeSpecification : ToolPathSpecificationBase
    {
        public List<UniqueKey> BoundaryPolygonKey { get; set; }
        public double TopHeight { get; set; }
        public double BottomHeight { get; set; }
        public double MaximumCutDepth { get; set; }
        public List<double> CutHeights { get; set; }
        public string Description { get; set; }
        public double SafeHeight { get; set; }
        public EnumSpindleState SpindleState { get; set; }
        public int SpindleSpeed { get; set; }
        public int CuttingHorizontalFeedRate { get; set; }
        public int CuttingVerticalFeedRate { get; set; }
        public int FreeHorizontalFeedRate { get; set; }
        public int FreeVerticalFeedRate { get; set; }
        public BaseTool Tool { get; set; }

        public override void Calculate()
        {
            var path = new ToolPath.ToolPath();

            var boundary = GetOffsetBoundaries();

            var cutHeights = (CutHeights != null) ? CutHeights : GetCutHeights();

            var translations = GetTranslations(boundary, 4);//new List<Point2D> { new Point2D(0, 0) };//

            path.SetAbsolutePositioning();
            path.SetSpindleState(SpindleState);
            path.SetSpindleSpeed(SpindleSpeed);
            path.SetFeedRate(FreeVerticalFeedRate);
            path.SetTool(Tool);
            path.MoveToPoint(null, null, SafeHeight, null);

            foreach (var t in translations)
            {
                foreach (var polygon in boundary)
                {
                    path.MoveToPoint(polygon.GetPoints().First().X + t.X, polygon.GetPoints().First().Y + t.Y, null, FreeHorizontalFeedRate);

                    foreach (double height in cutHeights)
                    {
                        path.MoveToPoint(null, null, height, CuttingVerticalFeedRate);
                        path.SetFeedRate(CuttingHorizontalFeedRate);

                        foreach (var point in polygon.GetPoints())
                        {
                            path.MoveToPoint(point.X + t.X, point.Y + t.Y, null, null);
                        }

                        path.MoveToPoint(polygon.GetPoints().First().X + t.X, polygon.GetPoints().First().Y + t.Y, null, null);
                    }

                    path.MoveToPoint(null, null, SafeHeight, FreeVerticalFeedRate);
                }
            }

            path.SetSpindleState(EnumSpindleState.Off);
            
            ToolPath = path;

            var duration = path.GetDuration();
            var distance = path.GetDistance();
            var feedRateDurations = path.GetFeedRateDurations();
        }

        private List<double> GetCutHeights()
        {
            var heights = new List<double>();

            double exactLevels = (TopHeight - BottomHeight) / MaximumCutDepth;

            int numberOfLevels = (int) Math.Ceiling(exactLevels) + 1; 
            for(int i=1; i < numberOfLevels; i++)
            {
                double alpha = (double)i / (numberOfLevels - 1);
                double h = (1 - alpha) * TopHeight + alpha * BottomHeight;
                heights.Add(h);
            }
            return heights;
        }

        private List<Point2D> GetTranslations(List<Polygon2D> boundary, int numberOfCopies)
        {
            var result = new List<Point2D>();

            double minX = boundary.Min(p => p.MinX);
            double maxX = boundary.Max(p => p.MaxX);
            double minY = boundary.Min(p => p.MinY);
            double maxY = boundary.Max(p => p.MaxY);

            double offset = maxX - minX+10;

            for(int i=0; i < numberOfCopies; i++)
            {
                result.Add(new Point2D(i * offset, 0));
            }
            return result;
        }


        private List<Polygon2D> GetOffsetBoundaries()
        {
            
            //1. get polygons
            var polygonList = new List<Polygon2D>();
            foreach (var key in BoundaryPolygonKey)
            {
                var curve = Model[key] as IPolygon2D;
                if (curve != null)
                {
                    var poly2D = curve.ToPolygon2D(ReferencePlane);
                    poly2D.RemoveRedundantPoints(maxDistance: 0.005, minAngle: Math.PI * 0.95);
                    polygonList.Add(poly2D);
                }
            }

            //2. orient polygons correctly depending on internal/external
            foreach (var poly1 in polygonList)
            {
                if (IsInternal(poly1, polygonList))
                {
                    if(poly1.SignedArea() < 0)
                        poly1.Reverse();
                }
                else if (poly1.SignedArea() > 0)
                {
                    poly1.Reverse();
                }
            }

            List<Polygon2D> boundary = GetOffsetPolygons(polygonList, Tool.Diameter * 0.5);

            //clean up final polygons
            foreach (var polygon in boundary)
            {
                polygon.RemoveRedundantPoints(maxDistance: 0.005, minAngle: Math.PI * 0.95);
                var centre = polygon.Centre;
                var closestToCentre = polygon.GetPoints().Select(x => (Point: x, Distance: x.Distance(centre))).OrderBy(x => x.Distance).First().Point;
                polygon.CyclePointsToStartFrom(closestToCentre);
            }

            var result = new List<Polygon2D>();

            //first add internal paths 
            result.AddRange(OptimizeCuttingOrder(boundary.Where(x => IsInternal(x, boundary)).ToList()));

            //then add external paths
            result.AddRange(OptimizeCuttingOrder(boundary.Where(x => !IsInternal(x, boundary)).ToList()));

            return result;
        }
    
 
        private List<Polygon2D> GetOffsetPolygons(List<Polygon2D> polygons, double offset)
        {
            return ClipperWrapper.OffsetPolygons(polygons, Tool.Diameter * 0.5);
        }



        private List<Polygon2D> OptimizeCuttingOrder(List<Polygon2D> boundary)
        {
            if (boundary.Count < 2)
                return boundary;

            var result = new List<Polygon2D> { boundary[0]};
            boundary.RemoveAt(0);

            while (boundary.Count > 0)
            {
                double minDistance = double.MaxValue;
                Polygon2D closestPolygon = null;
                foreach (var polygon in boundary)
                {
                    double d = polygon.GetPoints()[0].Distance(result.Last().GetPoints()[0]);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        closestPolygon = polygon;
                    }
                }
                result.Add(closestPolygon);
                boundary.Remove(closestPolygon);
            }

            return result;
        }

     
        private bool IsInternal(Polygon2D polygon, List<Polygon2D> polygonList)
        {
            int numberOfSurroundingPolygons = 0;
            foreach (var poly2 in polygonList.Where(p => p != polygon))
            {
                if (poly2.IsInternal(polygon.GetPoints().First()))
                    numberOfSurroundingPolygons++;
            }

            return numberOfSurroundingPolygons % 2 == 1;
        }
        

    }
}
