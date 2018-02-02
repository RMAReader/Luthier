using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjects;

namespace Luthier.Model.GraphicObjectFactory
{
    public interface ILinkedLine2DFactory
    {
        GraphicLinkedLine2D New();
        GraphicPoint2D AppendPoint(GraphicLinkedLine2D line, double x, double y);
        void DeletePoint(GraphicLinkedLine2D line, GraphicPoint2D point);
        void DeleteLine(GraphicLinkedLine2D line);
    }

}
