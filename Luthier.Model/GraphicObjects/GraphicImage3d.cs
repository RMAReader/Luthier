using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.Extensions;
using SharpDX;
using SharpHelper;

namespace Luthier.Model.GraphicObjects
{
    public class GraphicImage3d : GraphicObjectBase, IDrawableTextured, ISelectable, ISketchCanvas
    {
        public double[] UpperLeft;
        public double[] LowerLeft;
        public double[] LowerRight;
        public double[] Normal;
        public string FileName;

        private System.Drawing.Image _image;
        public System.Drawing.Image Image
        {
            get
            {
                if (_image == null) _image = System.Drawing.Image.FromFile(FileName);
                return _image;
            }
        }


        public double AspectRatio => (double)Image.Width / Image.Height;

        public bool IsValid => _image != null;


        public GraphicImage3d(){ }
        public GraphicImage3d(string fileName)
        {
            FileName = fileName;
        }


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }




        #region "ISelectable implementation


        public bool IsSelected { get; set; }


        public RayIntersection GetRayIntersection(double[] from, double[] to)
        {
            RayIntersection result = null;

            var _origin = LowerLeft;
            var _unitU = LowerRight.Subtract(LowerLeft);
            //_unitU.Normalise();
            var _unitV = UpperLeft.Subtract(LowerLeft);
            //_unitV.Normalise();

            Matrix4x4 m = new Matrix4x4((float)_unitU[0], (float)_unitU[1], (float)_unitU[2], 0,
                                        (float)_unitV[0], (float)_unitV[1], (float)_unitV[2], 0,
                                        (float)(from[0] - to[0]), (float)(from[1] - to[1]), (float)(from[2] - to[2]), 0,
                                        0, 0, 0, 1f);

            if (Matrix4x4.Invert(m, out Matrix4x4 mInverse))
            {
                System.Numerics.Vector3 f = new System.Numerics.Vector3((float)from[0], (float)from[1], (float)from[2]);
                System.Numerics.Vector3 o = new System.Numerics.Vector3((float)_origin[0], (float)_origin[1], (float)_origin[2]);

                var coeffs = System.Numerics.Vector3.Transform(f - o, mInverse);
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
                    ObjectHit = 0 <= coeffs.X && coeffs.X <= 1 && 0 <= coeffs.Y && coeffs.Y <= 1
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


        #region "ISketchCanvas implementation"


        public double[] GetPointOfIntersectionWorld(double[] from, double[] to)
        {
            return GetRayIntersection(from, to).ObjectParameters;
        }

        public double[] GetNormalAtPointOfIntersectionWorld(double[] from, double[] to)
        {
            return Normal;
        }


        #endregion



        #region "IDrawableTextured implementation"

        public Image Texture => Image;

        public string TextureName => FileName;


        public void GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices)
        {

            var indexOffset = vertices.Count;
            var upperRight = UpperLeft.Add(LowerRight).Subtract(LowerLeft);
            var normal = (LowerRight.Subtract(LowerLeft)).VectorProduct(UpperLeft.Subtract(LowerLeft));

            var sw = new SharpDX.Vector3((float)LowerLeft[0], (float)LowerLeft[1], (float)LowerLeft[2]);
            var nw = new SharpDX.Vector3((float)UpperLeft[0], (float)UpperLeft[1], (float)UpperLeft[2]);
            var ne = new SharpDX.Vector3((float)upperRight[0], (float)upperRight[1], (float)upperRight[2]);
            var se = new SharpDX.Vector3((float)LowerRight[0], (float)LowerRight[1], (float)LowerRight[2]);
            var n = new SharpDX.Vector3((float)normal[0], (float)normal[1], (float)normal[2]);

            vertices.AddRange(new TangentVertex[]
            {
                new TangentVertex{ Position = sw, Normal = n, TextureCoordinate = new SharpDX.Vector2(0, 1)},
                new TangentVertex{ Position = se, Normal = n, TextureCoordinate = new SharpDX.Vector2(1, 1)},
                new TangentVertex{ Position = ne, Normal = n, TextureCoordinate = new SharpDX.Vector2(1, 0)},
                new TangentVertex{ Position = nw, Normal = n, TextureCoordinate = new SharpDX.Vector2(0, 0)},

                new TangentVertex{ Position = sw, Normal = -n, TextureCoordinate = new SharpDX.Vector2(0, 1)},
                new TangentVertex{ Position = se, Normal = -n, TextureCoordinate = new SharpDX.Vector2(1, 1)},
                new TangentVertex{ Position = ne, Normal = -n, TextureCoordinate = new SharpDX.Vector2(1, 0)},
                new TangentVertex{ Position = nw, Normal = -n, TextureCoordinate = new SharpDX.Vector2(0, 0)},
            });

            var rawIndices = new int[] { 0, 2, 1, 2, 0, 3, 4, 5, 6, 6, 7, 4 };
            foreach (var index in rawIndices)
            {
                indices.Add(index + indexOffset);
            }


        }

   

        #endregion
    }
}

//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using SharpDX;
//using SharpHelper;

//namespace Luthier.Model.GraphicObjects
//{
//    [Serializable]
//    public class GraphicImage3d : GraphicObjectBase, ISelectable, IDrawableTextured
//    {
//        public UniqueKey ReferencePlaneKey;

//        public System.Numerics.Vector2 UpperLeftIntrinsic;
//        public System.Numerics.Vector2 LowerLeftIntrinsic;
//        public System.Numerics.Vector2 LowerRightIntrinsic;

//        public System.Numerics.Vector2 ControlPoint1Intrinsic;
//        public System.Numerics.Vector2 ControlPoint2Intrinsic;

//        public GraphicPlane Plane => (GraphicPlane)base.Model[ReferencePlaneKey];

//        public string FileName;
        
//        private System.Drawing.Image _image;
//        public System.Drawing.Image Image
//        {
//            get
//            {
//                if(_image == null) _image = System.Drawing.Image.FromFile(FileName);
//                return _image;
//            }
//        }

//        public double AspectRatio => (double)Image.Width / Image.Height;

//        public bool IsValid => _image != null;

//        public GraphicImage3d(string fileName, GraphicPlane plane)
//        {
//            FileName = fileName;
//            ReferencePlaneKey = plane.Key;
//            LayerKey = plane.LayerKey;
//        }

//        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
//        {
//            return double.MaxValue;
//        }



//        private SharpDX.Vector3 ConvertIntrinsicToWorld(System.Numerics.Vector2 intrisicCoords)
//        {
//            var origin = Helpers.Convert.NumericsVector3(Plane._origin);
//            var unitU = Helpers.Convert.NumericsVector3(Plane._unitU);
//            var unitV = Helpers.Convert.NumericsVector3(Plane._unitV);

//            return Helpers.Convert.SharpDXVector3(origin + intrisicCoords.X * unitU + intrisicCoords.Y * unitV);
//        }


//        #region "ISelectable implementation"


//        public RayIntersection GetRayIntersection(double[] from, double[] to)
//        {
//            var planeIntersection = Plane.GetRayIntersection(from, to);

//            var p = Helpers.Convert.NumericsVector2(planeIntersection.ObjectParameters) - LowerLeftIntrinsic;

//            System.Numerics.Matrix4x4 m = new System.Numerics.Matrix4x4(
//                LowerRightIntrinsic.X, LowerRightIntrinsic.Y, 0, 0,
//                UpperLeftIntrinsic.X, UpperLeftIntrinsic.Y, 0, 0,
//                0, 0, 1, 0,
//                0, 0, 0, 1);

//            System.Numerics.Matrix4x4.Invert(m, out System.Numerics.Matrix4x4 mInverse);

//            var coeffs = System.Numerics.Vector4.Transform(new System.Numerics.Vector4(p.X, p.Y, 0, 0), mInverse);

//            planeIntersection.ObjectHit = 0 <= coeffs.X && coeffs.X <= 1 && 0 <= coeffs.Y && coeffs.Y <= 1;


//            return planeIntersection;

//        }



//        #endregion




//        #region "IDrawableTextured implementation"

//        public Image Texture => Image;

//        public string TextureName => FileName;

//        public bool IsSelected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public void GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices)
//        {
            
//            var indexOffset = vertices.Count;
          
//            var sw = ConvertIntrinsicToWorld(LowerLeftIntrinsic);
//            var nw = ConvertIntrinsicToWorld(UpperLeftIntrinsic);
//            var se = ConvertIntrinsicToWorld(LowerRightIntrinsic);
//            var ne = nw + se - sw;
//            var n = Helpers.Convert.SharpDXVector3(Plane._normal);

//            vertices.AddRange(new TangentVertex[]
//            {
//                new TangentVertex{ Position = sw, Normal = n, TextureCoordinate = new Vector2(0, 1)},
//                new TangentVertex{ Position = se, Normal = n, TextureCoordinate = new Vector2(1, 1)},
//                new TangentVertex{ Position = ne, Normal = n, TextureCoordinate = new Vector2(1, 0)},
//                new TangentVertex{ Position = nw, Normal = n, TextureCoordinate = new Vector2(0, 0)},

//                new TangentVertex{ Position = sw, Normal = -n, TextureCoordinate = new Vector2(0, 1)},
//                new TangentVertex{ Position = se, Normal = -n, TextureCoordinate = new Vector2(1, 1)},
//                new TangentVertex{ Position = ne, Normal = -n, TextureCoordinate = new Vector2(1, 0)},
//                new TangentVertex{ Position = nw, Normal = -n, TextureCoordinate = new Vector2(0, 0)},
//            });

//            var rawIndices = new int[] { 0, 2, 1, 2, 0, 3, 4, 5, 6, 6, 7, 4 };
//            foreach(var index in rawIndices)
//            {
//                indices.Add(index + indexOffset);
//            }

            
//        }

     
//        #endregion
//    }
//}
