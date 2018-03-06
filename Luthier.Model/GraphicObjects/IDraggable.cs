using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public interface IDraggable
    {
        double GetDistance(double x, double y);
        void Set(double x, double y);
    }
}
