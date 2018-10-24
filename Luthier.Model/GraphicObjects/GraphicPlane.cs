using System.Numerics;
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
    public class GraphicPlane : GraphicObjectBase, ISketchCanvas, IDrawableLines, ISelectable
    {
        protected double[] _origin;
        protected double[] _normal;
        protected double[] _unitU;
        protected double[] _unitV;
        protected double _minU = -200;
        protected double _minV = -200;
        protected double _maxU = 200;
        protected double _maxV = 200;

        private bool _isVisible;
        public override bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                if (_isVisible == false)
                    IsSelected = false;
            }
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return Double.MaxValue;
        }


        public GraphicPlane GetParallelPlaneThroughPoint(double[] point)
        {
            return new GraphicPlane
            {
                _origin = point,
                _normal = this._normal,
                _unitU = this._unitU,
                _unitV = this._unitV,
            };
        }


        public static GraphicPlane CreateRightHandedXY(double[] origin)
        {
            return new GraphicPlane
            {
                _origin = origin,
                _normal = new double[] { 0, 0, 1.0 },
                _unitU = new double[] { 1.0, 0, 0 },
                _unitV = new double[] { 0, 1.0, 0 },
            };
        }
        public static GraphicPlane CreateRightHandedYZ(double[] origin)
        {
            return new GraphicPlane
            {
                _origin = origin,
                _normal = new double[] { 1.0, 0, 0 },
                _unitU = new double[] { 0, 1.0, 0 },
                _unitV = new double[] { 0, 0, 1.0 },
            };
        }
        public static GraphicPlane CreateRightHandedZX(double[] origin)
        {
            return new GraphicPlane
            {
                _origin = origin,
                _normal = new double[] { 0, 1.0, 0 },
                _unitU = new double[] { 0, 0, 1.0 },
                _unitV = new double[] { 1.0, 0, 0 },
            };
        }


        public static GraphicPlane CreateRightHandedThroughPoints(double[] origin, double[] pu, double[] pv)
        {
            var unitU = pu.Subtract(origin);
            unitU.Normalise();
            
            var unitV = pv.Subtract(origin);
            unitV.Normalise();

            var normal = unitU.VectorProduct(unitV);

            return new GraphicPlane
            {
                _normal = normal,
                _origin = origin,
                _unitU = unitU,
                _unitV = unitV,
            };
        }

        #region "ISketchCanvas implementation"

        public double[] GetPointOfIntersectionWorld(double[] from, double[] to)
        {
            return GetRayIntersection(from, to).IntersectInWorldCoords;
        }

        public double[] GetNormalAtPointOfIntersectionWorld(double[] from, double[] to)
        {
            return _normal;
        }

        #endregion

        #region "ISelectable implementation"


        public bool IsSelected { get; set; }


        public RayIntersection GetRayIntersection(double[] from, double[] to)
        {
            RayIntersection result = null;

            Matrix4x4 m = new Matrix4x4((float)_unitU[0], (float)_unitU[1], (float)_unitU[2], 0,
                                        (float)_unitV[0], (float)_unitV[1], (float)_unitV[2], 0,
                                        (float)(from[0] - to[0]), (float)(from[1] - to[1]), (float)(from[2] - to[2]), 0,
                                        0,0,0,1f);
            
            if (Matrix4x4.Invert(m, out Matrix4x4 mInverse))
            {
                Vector3 f = new Vector3((float)from[0], (float)from[1], (float)from[2]);
                Vector3 o = new Vector3((float)_origin[0], (float)_origin[1], (float)_origin[2]);

                var coeffs = Vector3.Transform(f - o, mInverse);
                var intersectWorld = new double[]
                {
                    (1 - coeffs.Z) * from[0] + coeffs.Z * to[0],
                    (1 - coeffs.Z) * from[1] + coeffs.Z * to[1],
                    (1 - coeffs.Z) * from[2] + coeffs.Z * to[2]
                };

                result = new RayIntersection
                {
                    Object = this,
                    IntersectInWorldCoords = intersectWorld,
                    RayParameter = coeffs.Z,
                    ObjectParameters = new double[] { coeffs.X, coeffs.Y },
                    ObjectHit = _minU <= coeffs.X && coeffs.X <= _maxU && _minV <= coeffs.Y && coeffs.Y <= _maxV
                };
            }
            else
            {
                result = new RayIntersection
                {
                    Object = this,
                    ObjectHit = false
                };
            }
          
            return result;
            
        }

        #endregion

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

            var setting = (IsSelected) ? Settings.Default.GraphicPlaneGridAppearanceSelected : Settings.Default.GraphicPlaneGridAppearance;

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
