using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{
    public class NurbsSurfaceFitterBase : INurbsSurfaceFitter
    {
        public event IterationCompleteEventHandler IterationCompleteEvent;

        protected virtual void OnIterationComplete(IterationCompleteEventArgs e)
        {
            IterationCompleteEventHandler handler = IterationCompleteEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public virtual NurbsSurface Fit(NurbsSurface initialGuess, PointCloud cloud)
        {
            throw new NotImplementedException();
        }

    
    }
}
