using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicCompositePolygon : GraphicObjectBase
    {
        [XmlArray]
        public List<UniqueKey> Junctions;

        public GraphicCompositePolygon() => Junctions = new List<UniqueKey>();


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return float.MaxValue;
        }
    }

   
}
