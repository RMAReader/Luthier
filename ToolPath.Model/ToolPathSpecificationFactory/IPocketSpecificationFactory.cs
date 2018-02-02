using Luthier.CncTool;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.ToolPathSpecificationFactory
{
    public interface IPocketSpecificationFactory
    {
        PocketSpecification New();
        
        void AddBoundaryPolygons(PocketSpecification spec, List<GraphicPolygon2D> polygons);
        void RemoveBoundaryPolygons(PocketSpecification spec, List<GraphicPolygon2D> polygons);
        void SetTool(PocketSpecification spec, BaseTool tool);
        
    }
}
