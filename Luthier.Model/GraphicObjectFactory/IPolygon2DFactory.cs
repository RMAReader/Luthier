using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjects;

namespace Luthier.Model.GraphicObjectFactory
{
    public interface IPolygon2DFactory
    {
        GraphicPolygon2D New();
        GraphicPoint2D AppendPoint(GraphicPolygon2D line, double x, double y);
        void DeletePoint(GraphicPolygon2D line, GraphicPoint2D point);
        void Delete(GraphicPolygon2D line);
    }
}
