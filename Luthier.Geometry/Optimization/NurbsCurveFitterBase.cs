using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry.Nurbs;

namespace Luthier.Geometry.Optimization
{
    public class NurbsCurveFitterBase : INurbsCurveFitter
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


        public virtual NurbsCurve Fit(NurbsCurve initialGuess, PointCloud cloud)
        {
            throw new NotImplementedException();
        }


    }
}
