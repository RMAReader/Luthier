using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    //[XmlRoot(ElementName = "GraphicObjectBase")]
    //[XmlType(TypeName = "GraphicObjectBase")]
    //[XmlInclude(typeof(GraphicPoint2D))]
    //[XmlInclude(typeof(GraphicPolygon2D))]
    //[XmlInclude(typeof(ToolPathSpecificationBase))]
    //[XmlInclude(typeof(PocketSpecification))]
    public abstract class GraphicObjectBase
    {
        [XmlElement()]
        public UniqueKey Key { get; set; }
        
        public GraphicObjectBase()
        {
           Key = new UniqueKey();
        }


        public abstract double GetDistance(ApplicationDocumentModel model, double x, double y);

    }

    [Serializable]
    //[XmlRoot(ElementName= "UniqueKey")]
    //[XmlType(TypeName= "UniqueKey")]
    public class UniqueKey
    {
        [XmlAttribute()]
        public string data { get; set; }

        public UniqueKey() => data = Guid.NewGuid().ToString();
        public UniqueKey(string data) => this.data = data;

        public override int GetHashCode() => data.GetHashCode();

        public override bool Equals(object obj) => (obj is UniqueKey) ? data.Equals(((UniqueKey)obj).data) : false;

        public override string ToString() => data;

        public static bool operator ==(UniqueKey obj1, UniqueKey obj2)
        {
            if (obj1 is null && obj2 is null) return true;
            if (!(obj1 is null) && !(obj2 is null)) return obj1.Equals(obj2);
            return false;
        }
        public static bool operator !=(UniqueKey obj1, UniqueKey obj2) => !(obj1 == obj2);

    }

}
