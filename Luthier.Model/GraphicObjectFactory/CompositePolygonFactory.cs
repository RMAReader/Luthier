using Luthier.Core;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjectFactory
{
    public class CompositePolygonFactory
    {

        private ApplicationDocumentModel data;

        public CompositePolygonFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public ApplicationDocumentModel GetModel() => data;

        public GraphicCompositePolygon New()
        {
            var obj = new GraphicCompositePolygon();
            data.Model.Add(obj);

            Log.Instance().Append(string.Format("Created CompositePolygon. Key = {0}", obj.Key));
            return obj;
        }

        public void AddIntersection(GraphicCompositePolygon composite, GraphicIntersection intersection)
        {
            composite.Junctions.Add(intersection.Key);
        }

    }
}
