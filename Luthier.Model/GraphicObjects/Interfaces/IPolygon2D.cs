using Luthier.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public interface IPolygon2D
    {
        UniqueKey Key();
        Polygon2D ToPolygon2D(IApplicationDocumentModel model);
        Polygon2D ToPolygon2D();
        Polygon2D ToPolygon2D(GraphicPlane referencePlane);
    }
}
