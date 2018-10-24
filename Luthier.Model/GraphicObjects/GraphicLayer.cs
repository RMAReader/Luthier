using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicLayer : GraphicObjectBase
    {
        [XmlElement]
        public List<UniqueKey> Objects { get; set; }

        public GraphicLayer()
        {
            Objects = new List<UniqueKey>();
        }

        public void AddToLayer(GraphicObjectBase obj)
        {
            if (obj.LayerKey != null) obj.RemoveFromLayer();
            Objects.Add(obj.Key);
            obj.LayerKey = this.Key;
        }

        public override void RemoveFromModel()
        {
            var objectsToDelete = Objects.Select(x => Model[x]).ToList();

            foreach (var obj in objectsToDelete)
            {
                obj.RemoveFromModel();
            }

            base.RemoveFromModel();
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }
    }
}
