using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Luthier.Core;
using Luthier.Geometry;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    //[XmlRoot(ElementName = "GraphicPoint2D", Namespace = Serializer<GraphicPoint2D>.StyleResearchNamespace)]
    //[XmlType("GraphicPoint2D")]
    public class GraphicPoint2D : GraphicObjectBase
    {
        [XmlAttribute()]
        public double X;
        [XmlAttribute()]
        public double Y;
        [XmlArray]
        public List<UniqueKey> parentObjectKeys;

        public GraphicPoint2D() { parentObjectKeys = new List<UniqueKey>(); }
        public GraphicPoint2D(double x, double y) { this.X = x; this.Y = y; parentObjectKeys = new List<UniqueKey>(); }
        public void Set(double x, double y) { this.X = x; this.Y = y; }

        public override bool Equals(object obj) 
        {
            if (obj is GraphicPoint2D)
            {
                var p = (GraphicPoint2D)obj;
                if (p.Key.Equals(Key) && p.X == X && p.Y == Y) return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return (double) Math.Sqrt((X - x) * (X - x) + (Y - y) * (Y - y));
        }


        public Point2D ToPrimitive()
        {
            return new Point2D(X, Y);
        }
    }
}
