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

            List<Polygon2D> boundary = new List<Polygon2D>();
            foreach (var key in specification.BoundaryPolygonKey)
            {
                var polygon = model.Objects()[key] as GraphicPolygon2D;
                if (polygon != null)
                {
                    boundary.Add(polygon.ToPolygon2D(model));
                }
            }
            ScanLinePath2D scanLinePath = new ScanLinePath2D(boundary, specification.StepLength);

            foreach(var section in scanLinePath.Sections)
            {
                path.MoveToPoint(null, null, specification.SafeHeight, null);
                path.MoveToPoint(section.First().x, section.First().y, null, null);

                foreach (var point in section.Points)
                {
                    path.MoveToPoint(point.x, point.y, specification.CutHeight, null);
                }

                path.MoveToPoint(null, null, specification.SafeHeight, null);
            }

            path.SetSpindleState(EnumSpindleState.Off);

            return path;
        }
    }
}
