using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{
    public interface INurbsSurfaceFitter
    {
        event IterationCompleteEventHandler IterationCompleteEvent;

        NurbsSurface Fit(NurbsSurface initialGuess, PointCloud cloud);
    }

  
}
