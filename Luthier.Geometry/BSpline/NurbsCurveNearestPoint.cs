using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.BSpline
{
    public class NurbsCurveNearestPoint
    {
        private int _numberOfLineSegments = 1000;

        public NurbsCurveNearestPointResult Calculate(NurbsCurve curve, PointCloud cloud)
        {
            var result = new NurbsCurveNearestPointResult
            {
                Distances = new double[cloud.PointCount],
                Coordinates = new double[cloud.PointCount * cloud.Dimension],
            };

            var footPoints = curve.ToLines(_numberOfLineSegments);

            double[] cp = new double[cloud.Dimension];

            for (int i = 0; i < cloud.PointCount; i++)
            {
                cloud.GetPointFast(i, cp);

                double distance = double.MaxValue;
                int footPointIndex = 0;
                for(int j = 0; j < footPoints.Count; j++)
                {
                    double[] fp = footPoints[j];

                    double dx = fp[0] - cp[0];
                    double dy = fp[1] - cp[1];

                    if (Math.Abs(dx) < distance && Math.Abs(dy) < distance)
                    {
                        double d = Math.Sqrt(dx * dx + dy * dy);
                        if (d < distance)
                        {
                            distance = d;
                            footPointIndex = j;
                        }
                    }
                }
                result.Distances[i] = distance;
                Array.Copy(footPoints[footPointIndex], 0, result.Coordinates, i * cloud.Dimension, cloud.Dimension);
                
            }

            return result;
        }


    }

    public class NurbsCurveNearestPointResult
    {
        public double[] Parameters;
        public double[] Distances;
        public double[] Coordinates;
    }
}
