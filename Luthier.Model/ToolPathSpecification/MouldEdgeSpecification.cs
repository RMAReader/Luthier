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
        public string Description { get; set; }
        public double SafeHeight { get; set; }
        public EnumSpindleState SpindleState { get; set; }
        public int SpindleSpeed { get; set; }
        public int FeedRate { get; set; }
        public BaseTool Tool { get; set; }

        public ToolPath.ToolPath Calculate()
        {
            var path = new ToolPath.ToolPath();

            var boundary = GetOffsetBoundaries();

            path.SetAbsolutePositioning();
            path.SetSpindleState(SpindleState);
            path.SetSpindleSpeed(SpindleSpeed);
            path.SetFeedRate(FeedRate);
            path.SetTool(Tool);
            path.MoveToPoint(null, null, SafeHeight, null);

            foreach (double height in GetCutHeights())
            {
                foreach (var polygon in boundary)
                {
                    path.MoveToPoint(null, null, SafeHeight, null);
                    path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, null, null);
                    path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, height, null);

                    foreach (var point in polygon.GetPoints())
                    {
                        path.MoveToPoint(point.X, point.Y, null, null);
                    }

                    path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, null, null);
                    path.MoveToPoint(null, null, SafeHeight, null);
                }
            }

            path.SetSpindleState(EnumSpindleState.Off);
            
            ToolPath = path;

            return path;
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
                    var poly2D = curve.ToPolygon2D();
                    poly2D.RemoveRedundantPoints(maxDistance: 0.005, minAngle: Math.PI * 0.95);
                    polygonList.Add(poly2D);
                }
            }

            //TODO: must determine if each polygon is internal or external, and make sure the orientation(clockwise/anti) is opposite for internal/external
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

            return boundary;
        }
    

        private List<Polygon2D> GetOffsetPolygons(List<Polygon2D> polygons, double offset)
        {
            return ClipperWrapper.OffsetPolygons(polygons, Tool.Diameter * 0.5);
        }

    }
}
