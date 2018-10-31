using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public interface ISketchCanvas
    {
        double[] GetPointOfIntersectionWorld(double[] from, double[] to);
        double[] GetNormalAtPointOfIntersectionWorld(double[] from, double[] to);
    }
}
