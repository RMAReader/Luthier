using Luthier.Model.CustomSettings;
using Luthier.Model.Extensions;
using Luthier.Model.Properties;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    public class Plane : GraphicObjectBase, ISketchCanvas, IDrawableLines
    {
        protected double[] _origin;
        protected double[] _normal;
        protected double[] _unitU;
        protected double[] _unitV;

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

        public double[] GetNormalAtPointOfIntersectionWorld(double[] from, double[] to)
        {
            return _normal;
        }

        public Plane GetParallelPlaneThroughPoint(double[] point)
        {
            return new Plane
            {
                _origin = point,
                _normal = this._normal,
                _unitU = this._unitU,
                _unitV = this._unitV,
            };
        }


        public static Plane CreateRightHandedXY(double[] origin)
        {
            return new Plane
            {
                _origin = origin,
                _normal = new double[] { 0, 0, 1.0 },
                _unitU = new double[] { 1.0, 0, 0 },
                _unitV = new double[] { 0, 1.0, 0 },
            };
        }
        public static Plane CreateRightHandedYZ(double[] origin)
        {
            return new Plane
            {
                _origin = origin,
                _normal = new double[] { 1.0, 0, 0 },
                _unitU = new double[] { 0, 1.0, 0 },
                _unitV = new double[] { 0, 0, 1.0 },
            };
        }
        public static Plane CreateRightHandedZX(double[] origin)
        {
            return new Plane
            {
                _origin = origin,
                _normal = new double[] { 0, 1.0, 0 },
                _unitU = new double[] { 0, 0, 1.0 },
                _unitV = new double[] { 1.0, 0, 0 },
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
                _normal = normal,
                _origin = origin,
                _unitU = unitU,
                _unitV = unitV,
            };
        }

        #region "IDrawable implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            var indexOffset = vertices.Count;

            int uStep = 10;
            int vStep = 10;
            int uMin = -20;
            int uMax = 20;
            int vMin = -20;
            int vMax = 20;

            var setting = Settings.Default.GraphicPlaneGridAppearance;

            //horizontal
            for (int i = uMin; i <= uMax; i++)
            {
                var colour = (i % 5 == 0) ? setting.MajorAxisColour : setting.MinorAxisColour;

                foreach (int j in new int[] { vMin, vMax })
                {
                    indices.Add(vertices.Count);

                    var position = _origin.Add(_unitU.Multiply(i * uStep)).Add(_unitV.Multiply(j * vStep));

                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3(0, 0, 1),
                        Color = colour
                    });
                    
                }
            }

            //vertical
            for (int j = vMin; j <= vMax; j++)
            {
                var colour = (j % 5 == 0) ? setting.MajorAxisColour : setting.MinorAxisColour;

                foreach (int i in new int[] { uMin, uMax })
                {
                    indices.Add(vertices.Count);

                    var position = _origin.Add(_unitU.Multiply(i * uStep)).Add(_unitV.Multiply(j * vStep));

                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3(0, 0, 1),
                        Color = colour
                    });

                }
            }

        }


      
        #endregion

    }
}
