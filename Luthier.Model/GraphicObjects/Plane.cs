using Luthier.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public class Plane : GraphicObjectBase, ISketchCanvas
    {
        private double[] _origin;
        private double[] _normal;
        private double[] _unitU;
        private double[] _unitV;

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return Double.MaxValue;
        }

      

        public double[] GetPointOfIntersectionWorld(double[] from, double[] to)
        {
            double d = _normal[0] * (to[0] - from[0]) + _normal[1] * (to[1] - from[1]) + _normal[2] * (to[2] - from[2]);
            if (Math.Abs(d) < Single.Epsilon) return null;

            double r = (_normal[0] * (_origin[0] - from[0]) + _normal[1] * (_origin[1] - from[1]) + _normal[2] * (_origin[2] - from[2])) / d;

            return new double[] 
            {
                (1 - r) * from[0] + r * to[0],
                (1 - r) * from[1] + r * to[1],
                (1 - r) * from[2] + r * to[2]
            };
        }


        public static Plane CreateRightHandedXY(double[] origin)
        {
            return new Plane
            {
                _normal = new double[] { 0, 0, 1.0 },
                _origin = origin,
                _unitU = new double[] { 1.0, 0, 0 },
                _unitV = new double[] { 0, 1.0, 0 },
            };
        }

        public static Plane CreateRightHandedThroughPoints(double[] origin, double[] pu, double[] pv)
        {
            var unitU = pu.Subtract(origin);
            unitU.Normalise();
            
            var unitV = pv.Subtract(origin);
            unitV.Normalise();

            var normal = unitU.VectorProduct(unitV);

            return new Plane
            {
                _normal = new double[] { 0, 0, 1.0 },
                _origin = origin,
                _unitU = new double[] { 1.0, 0, 0 },
                _unitV = new double[] { 0, 1.0, 0 },
            };
        }
    }
}
