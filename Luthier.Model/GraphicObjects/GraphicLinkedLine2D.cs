using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicLinkedLine2D : GraphicObjectBase
    {
        [XmlArray()]
        public List<UniqueKey> pointsKeys;
        public GraphicLinkedLine2D() => pointsKeys = new List<UniqueKey>();
        public void InsertPoint(int i, GraphicPoint2D p)
        {
            pointsKeys.Insert(i, p.Key);
            p.parentObjectKeys.Add(this.Key);
        }
        public void AddPoint(GraphicPoint2D p)
        {
            pointsKeys.Add(p.Key);
            p.parentObjectKeys.Add(this.Key);
        }
        public void RemovePoint(GraphicPoint2D p)
        {
            pointsKeys.Remove(p.Key);
            p.parentObjectKeys.Remove(this.Key);
        }


        public override bool Equals(object obj)
        {
            if (obj is GraphicLinkedLine2D)
            {
                var line = (GraphicLinkedLine2D)obj;
                if (line.Key.Equals(Key) == false) return false;
                for (int i = 0; i < pointsKeys.Count; i++)
                {
                    if (pointsKeys[i].Equals(line.pointsKeys[i]) == false) return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return model.Model.VisibleObjects()
              .Where(o => pointsKeys.Contains(o.Key))
              .Select(o => o.GetDistance(model, x, y))
              .OrderBy(o => o)
              .FirstOrDefault();
        }
    }
}
