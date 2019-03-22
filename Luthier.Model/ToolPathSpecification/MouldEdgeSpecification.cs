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

            path.SetAbsolutePositioning();
            path.SetSpindleState(SpindleState);
            path.SetSpindleSpeed(SpindleSpeed);
            path.SetFeedRate(FreeVerticalFeedRate);
            path.SetTool(Tool);
            path.MoveToPoint(null, null, SafeHeight, null);

            var cutHeights = (CutHeights != null) ? CutHeights : GetCutHeights();

            foreach (var polygon in boundary)
            {
                path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, null, FreeHorizontalFeedRate);

                foreach (double height in cutHeights)
                {
                    path.MoveToPoint(null, null, height, CuttingVerticalFeedRate);
                    path.SetFeedRate(CuttingHorizontalFeedRate);

                    foreach (var point in polygon.GetPoints())
                    {
                        path.MoveToPoint(point.X, point.Y, null, null);
                    }

                    path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, null, null);
                }

                path.MoveToPoint(null, null, SafeHeight, FreeVerticalFeedRate);
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


        private List<Polygon2D> GetOffsetBoundaries()
        {
            List<Polygon2D> polygonList = new List<Polygon2D>();
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

            foreach(var poly1 in polygonList)
            {
                int numberOfSurroundingPolygons = 0;
                foreach(var poly2 in polygonList.Where(p => p != poly1))
                {
                    if (poly2.IsInternal(poly1.GetPoints().First()))
                        numberOfSurroundingPolygons++;
                }

                if(numberOfSurroundingPolygons % 2 == 1)
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

            foreach (var polygon in boundary)
                polygon.RemoveRedundantPoints(maxDistance: 0.005, minAngle: Math.PI * 0.95);

            boundary = OptimizeCuttingOrder(boundary);

            return boundary;
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

     


    }
}
