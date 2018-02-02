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
    public class PocketSpecificationFactory : IPocketSpecificationFactory
    {
        private ApplicationDocumentModel model;

        public PocketSpecificationFactory(ApplicationDocumentModel model)
        {
            this.model = model;
        }


        public PocketSpecification New()
        {
            var pocketSpec  = new PocketSpecification();
            model.objects.Add(pocketSpec);
            return pocketSpec;
        }

     

        public void AddBoundaryPolygons(PocketSpecification spec, List<GraphicPolygon2D> polygons)
        {
            spec.BoundaryPolygonKey.AddRange(polygons.Select(x => x.Key).Where(x => spec.BoundaryPolygonKey.Contains(x) == false));
        }

        public void RemoveBoundaryPolygons(PocketSpecification spec, List<GraphicPolygon2D> polygons)
        {
            foreach (var polygon in polygons) spec.BoundaryPolygonKey.Remove(polygon.Key);
        }

        public void SetTool(PocketSpecification spec, BaseTool tool)
        {
            spec.Tool = tool;
        }
    }
}
