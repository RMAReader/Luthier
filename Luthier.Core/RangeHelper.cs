using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Core
{
    public static class RangeHelper
    {

        public static double[] Between(double min, double max, double step)
        {
            int n = (int)((max - min) / step) + 1;
            double[] range = new double[n + 1];
            for (int i = 0; i <= n; i++)
            {
                range[i] = (1 - (double)i / n) * min + (double)i / n * max;
            }
            return range;
        }
    }
}
