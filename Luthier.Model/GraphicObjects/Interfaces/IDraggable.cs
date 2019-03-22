using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public interface IDraggable2d
    {
        double GetDistance(double x, double y);
        void Set(double x, double y);
    }

    public interface IDraggable3d
    {
        double GetDistance(double x, double y, double z);
        void Set(double x, double y, double z);
    }

    public interface IDraggable
    {
        double[] Values { get; set; }
    }

    public interface IHasDraggable
    {
        IEnumerable<IDraggable> GetDraggableObjects();
    }
}
