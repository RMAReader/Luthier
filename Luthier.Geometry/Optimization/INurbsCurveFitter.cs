using Luthier.Geometry.Nurbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Optimization
{
    public interface INurbsCurveFitter
    {
        event IterationCompleteEventHandler IterationCompleteEvent;

        NurbsCurve Fit(NurbsCurve initialGuess, PointCloud cloud);
    }

    public delegate void IterationCompleteEventHandler(object sender, IterationCompleteEventArgs e);

    public class IterationCompleteEventArgs : EventArgs
    {
        public int NumberOfIterations { get; set; }
        public double Error { get; set; }
        public double[] Parameters { get; set; }
    }

   
}
