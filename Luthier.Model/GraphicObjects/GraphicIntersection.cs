using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicIntersection : GraphicObjectBase
    {
        [XmlElement]
        public UniqueKey Centre;
        [XmlElement]
        public UniqueKey Radius;
        [XmlElement]
        public UniqueKey Object1;
        [XmlElement]
        public UniqueKey Object2;


        public GraphicIntersection() { }

        public GraphicIntersection(UniqueKey centre, UniqueKey radius)
        {
            Centre = centre;
            Radius = radius;
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return model.Objects()[Centre].GetDistance(model, x, y);
        }
    }


 
}
