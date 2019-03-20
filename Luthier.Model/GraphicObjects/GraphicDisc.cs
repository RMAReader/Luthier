using Luthier.Geometry;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicDisc : 
        GraphicObjectBase, 
        IDrawableLines,
        IPolygon2D
    {
        public Disc Disc { get; set; }


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }

        #region "IDrawableLines implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            int numberOfLines = 20;

            int startIndex = vertices.Count;

            var colour = new SharpDX.Vector4(1, 0, 1, 1);

            foreach (var p in Disc.PerimeterToLines(numberOfLines))
            {
                vertices.Add(new StaticColouredVertex
                {
                    Position = new SharpDX.Vector3((float)p[0], (float)p[1], (float)p[2]),
                    Color = colour,
                });
            }

            for(int i = 0; i < numberOfLines; i++ )
            {
                indices.Add(i + startIndex);
                indices.Add((i + 1) % numberOfLines + startIndex);
            }
        }


        #endregion


        #region "IPolygon2D implementation"

        public Polygon2D ToPolygon2D(IApplicationDocumentModel model)
        {
            throw new NotSupportedException();
        }

        public Polygon2D ToPolygon2D()
        {
            return new Polygon2D(Disc.PerimeterToLines(1000).Select(p => new Point2D(p.X, p.Y)).ToList());
        }

        UniqueKey IPolygon2D.Key()
        {
            return Key;
        }

        #endregion
    }
}
