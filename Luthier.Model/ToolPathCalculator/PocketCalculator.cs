using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.Geometry;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathSpecification;
using Luthier.ToolPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathCalculator
{
    public class PocketCalculator : ToolPathCalculatorBase
    {
        private readonly IApplicationDocumentModel model;
        private readonly PocketSpecification specification;

        public PocketCalculator(PocketSpecification specification, IApplicationDocumentModel model)
        {
            this.model = model;
            this.specification = specification;
        }

        public override ToolPath.ToolPath Execute()
        {
            path  = new ToolPath.ToolPath();

            path.SetAbsolutePositioning();
            path.SetSpindleState(specification.SpindleState);
            path.SetSpindleSpeed(specification.SpindleSpeed);
            path.SetFeedRate(specification.FeedRate);
            path.SetTool(specification.Tool);
            path.MoveToPoint(null, null, specification.SafeHeight, null);

            List<Polygon2D> polygonList = new List<Polygon2D>();
            foreach (var key in specification.BoundaryPolygonKey)
            {
                var polygon = model.Model[key] as IPolygon2D;
                if (polygon != null)
                {
                    var poly2D = polygon.ToPolygon2D(model);
                    poly2D.RemoveRedundantPoints(0.01);
                    polygonList.Add(poly2D);
                }
            }
            

            List<Polygon2D> boundary = new List<Polygon2D>();
            foreach (var polygon in polygonList)
            {
                boundary.AddRange(ClipperWrapper.OffsetPolygon(polygon, specification.Tool.Diameter * 0.5));
            }

            foreach (var polygon in boundary) polygon.RemoveRedundantPoints(0.01);

            ScanLinePath2D scanLinePath = new ScanLinePath2D(boundary, specification.StepLength);

            foreach(var section in scanLinePath.Sections)
            {
                path.MoveToPoint(null, null, specification.SafeHeight, null);
                path.MoveToPoint(section.First().X, section.First().Y, null, null);

                foreach (var point in section.Points)
                {
                    path.MoveToPoint(point.X, point.Y, specification.CutHeight, null);
                }

                path.MoveToPoint(null, null, specification.SafeHeight, null);
            }


            //cut out borders
            foreach(var polygon in boundary)
            {
                path.MoveToPoint(null, null, specification.SafeHeight, null);
                path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, null, null);
                path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, specification.CutHeight, null);
                foreach (var point in polygon.GetPoints())
                {
                    path.MoveToPoint(point.X, point.Y, null, null);
                }
                path.MoveToPoint(polygon.GetPoints().First().X, polygon.GetPoints().First().Y, null, null);
                path.MoveToPoint(null, null, specification.SafeHeight, null);
            }

            path.SetSpindleState(EnumSpindleState.Off);

            return path;
        }
    }
}
