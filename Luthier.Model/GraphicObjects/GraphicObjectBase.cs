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
        [XmlIgnore]
        public GraphicModel Model { get; set; }

        [XmlElement()]
        public UniqueKey Key { get; set; }

        [XmlElement()]
        public UniqueKey LayerKey { get; set; }

        [XmlElement()]
        public string Name { get; set; }

        [XmlElement()]
        public virtual bool IsVisible { get; set; }


        public GraphicObjectBase()
        {
            Key = new UniqueKey();
            IsVisible = true;
        }
        public GraphicObjectBase(string name)
        {
            Key = new UniqueKey();
            Name = name;
            IsVisible = true;
        }


     
        public override string ToString()
        {
            return (String.IsNullOrEmpty(Name)) ? Key.ToString() : Name;
        }

        public abstract double GetDistance(ApplicationDocumentModel model, double x, double y);


        public virtual void RemoveFromLayer()
        {
            if (LayerKey != null)
            {
                var layer = (GraphicLayer) Model[LayerKey];
                layer.Objects.Remove(Key);
                LayerKey = null;
            }
        }


        public virtual void RemoveAllReferences()
        {
            RemoveFromLayer();
        }

        public virtual void RemoveFromModel()
        {
            RemoveAllReferences();
            Model.Remove(this);
        }
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
