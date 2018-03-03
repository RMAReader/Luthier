using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry
{
    public class Interval
    {
        private double[] interval;
        public Interval(double[] interval) { this.interval = new double[] { interval[0], interval[1]}; }
        public Interval(double t0, double t1) { this.interval = new double[] { t0, t1 }; }
        public double Min { get { return interval[0]; }}
        public double Max { get { return interval[1]; } }
		void Set(double t0, double t1) { interval[0] = t0; interval[1] = t1;  }
    }
}
