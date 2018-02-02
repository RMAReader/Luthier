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
    //[XmlRoot(ElementName = "GraphicPolygon2D", Namespace = Serializer<ApplicationDocumentModel>.StyleResearchNamespace)]
    public class GraphicPolygon2D : GraphicLinkedLine2D, IPolygon2D
    {
        public GraphicPolygon2D() { pointsKeys = new List<UniqueKey>();  }

        public Polygon2D ToPolygon2D(IApplicationDocumentModel model)
        {
            List<Point2D> points = new List<Point2D>();
            foreach(var key in pointsKeys)
            {
                GraphicPoint2D point = (GraphicPoint2D) model.Objects()[key];
                points.Add(new Point2D(point.X, point.Y));
            }
            return new Polygon2D(points);
        }


        public override bool Equals(object obj)
        {
            if (obj is GraphicPolygon2D)
            {
                var p = (GraphicPolygon2D)obj;
                if (p.Key.Equals(Key) == false) return false;
                for(int i = 0; i < pointsKeys.Count; i++)
                {
                    if (pointsKeys[i].Equals(p.pointsKeys[i]) == false) return false;
                }
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        UniqueKey IPolygon2D.Key() => base.Key;
        
    }
}
