using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjects;

namespace Luthier.Model.GraphicObjectFactory
{
    public interface IPoint2DFactory
    {
        GraphicPoint2D New(double x, double y);
        void Delete(GraphicPoint2D point);
    }
}
