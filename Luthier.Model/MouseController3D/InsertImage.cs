using Luthier.Model.Extensions;
using Luthier.Model.GraphicObjects;
//using SharpDX;
//using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using Luthier.Geometry;

namespace Luthier.Model.MouseController3D
{
    public class InsertImage : SketchObjectBase
    {
        public string Path { get; set; }

        private EnumSketchSurfaceStatus state;
        private Luthier.Model.GraphicObjects.GraphicImage3d _image;

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (state)
                    {
                        case EnumSketchSurfaceStatus.FirstPointPending:

                            if (Path != null)
                            {
                                var point = CalculateIntersection(e.X, e.Y);

                                _image = new GraphicImage3d(Path);
                                _image.LowerLeft = point;
                                _image.LowerRight = point;
                                _image.UpperLeft = point;
                                _image.Normal = _canvas.GetNormalAtPointOfIntersectionWorld(null, null);

                                _model.Model.Add(_image);

                                state = EnumSketchSurfaceStatus.SecondPointPending;
                            }
                            break;

                        case EnumSketchSurfaceStatus.SecondPointPending:
                            if (_image != null)
                            {
                                _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

                                _image.LowerRight = _canvas.GetPointOfIntersectionWorld(from, to);

                                var rayDirection = to.Subtract(from);

                                _image.UpperLeft = GetUpperLeft(rayDirection, _image.LowerLeft, _image.LowerRight, _image.AspectRatio);
                                state = EnumSketchSurfaceStatus.FirstPointPending;
                                _image = null;
                            }
                            break;

                    }
                    break;

                case MouseButtons.Right:
                    if (_image != null)
                    {
                        _model.Model.Remove(_image);
                        state = EnumSketchSurfaceStatus.FirstPointPending;
                        _image = null;
                    }
                    break;
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_image != null)
            {
                _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

                _image.LowerRight = _canvas.GetPointOfIntersectionWorld(from, to);

                var rayDirection = to.Subtract(from);

                _image.UpperLeft = GetUpperLeft(rayDirection, _image.LowerLeft, _image.LowerRight, _image.AspectRatio);
                _model.Model.HasChanged = true;
            }
        }


        private double[] GetUpperLeft(double[] ray, double[] lowerLeft, double[] lowerRight, double aspectRatio)
        {
            var normal = _image.Normal;

            Vector3 n = new Vector3((float)normal[0], (float)normal[1], (float)normal[2]);
            Vector3 r = new Vector3((float)ray[0], (float)ray[1], (float)ray[2]);
            Vector3 bottomLeft = new Vector3((float)lowerLeft[0], (float)lowerLeft[1], (float)lowerLeft[2]);
            Vector3 bottomRight = new Vector3((float)lowerRight[0], (float)lowerRight[1], (float)lowerRight[2]);

            //determines whether we are viewing plan from infront or behind.  Texture will be placed so that image is not flipped
            var sign = Math.Sign(Vector3.Dot(n, r));

            Vector3 bottomEdge = bottomRight - bottomLeft;

            Vector3 leftEdge = (float)(sign / _image.AspectRatio) * Vector3.Cross(n, bottomEdge);

            Vector3 topLeft = bottomLeft + leftEdge;

            return new double[] { topLeft.X, topLeft.Y, topLeft.Z };
        }
    }
}
//using Luthier.Model.Extensions;
//using Luthier.Model.GraphicObjects;
//using System;
//using System.Windows.Forms;
//using System.Numerics;

//namespace Luthier.Model.MouseController3D
//{
//    public class InsertImage : SketchObjectBase
//    {
//        public string Path { get; set; }

//        private EnumSketchSurfaceStatus state;
//        private Luthier.Model.GraphicObjects.GraphicImage3d _image;
       
//        public override void MouseDown(object sender, MouseEventArgs e)
//        {
//            switch (e.Button)
//            {
//                case MouseButtons.Left:
//                    switch (state)
//                    {
//                        case EnumSketchSurfaceStatus.FirstPointPending:

//                            if (Path != null)
//                            {
//                                var intersection = GetRayIntersection(e.X, e.Y);

//                                var intrinsicCoords = Helpers.Convert.NumericsVector2(intersection.ObjectParameters);

//                                _image = new GraphicImage3d(Path, (GraphicPlane)_canvas);
//                                _image.LowerLeftIntrinsic = intrinsicCoords;
//                                _image.LowerRightIntrinsic = intrinsicCoords;
//                                _image.UpperLeftIntrinsic = intrinsicCoords;

//                                _model.Model.Add(_image);

//                                state = EnumSketchSurfaceStatus.SecondPointPending;
//                            }
//                            break;

//                        case EnumSketchSurfaceStatus.SecondPointPending:
//                            if (_image != null)
//                            {

//                                var intersection = GetRayIntersection(e.X, e.Y);

//                                _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

//                                _image.LowerRightIntrinsic = Helpers.Convert.NumericsVector2(intersection.ObjectParameters);

//                                var rayDirection = Helpers.Convert.NumericsVector3(to.Subtract(from));

//                                _image.UpperLeftIntrinsic = GetUpperLeft(rayDirection, _image.LowerLeftIntrinsic, _image.LowerRightIntrinsic, _image.AspectRatio);
//                                state = EnumSketchSurfaceStatus.FirstPointPending;
//                                _image = null;
//                            }
//                            break;

//                    }
//                    break;

//                case MouseButtons.Right:
//                    if (_image != null)
//                    {
//                        _model.Model.Remove(_image);
//                        state = EnumSketchSurfaceStatus.FirstPointPending;
//                        _image = null;
//                    }
//                    break;
//            }
//        }

//        public override void OnMouseMove(object sender, MouseEventArgs e)
//        {
//            if (_image != null)
//            {
//                var intersection = GetRayIntersection(e.X, e.Y);

//                _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

//                _image.LowerRightIntrinsic = Helpers.Convert.NumericsVector2(intersection.ObjectParameters);

//                var rayDirection = Helpers.Convert.NumericsVector3(to.Subtract(from));

//                _image.UpperLeftIntrinsic = GetUpperLeft(rayDirection, _image.LowerLeftIntrinsic, _image.LowerRightIntrinsic, _image.AspectRatio);
//                _model.Model.HasChanged = true;
//            }
//        }


//        private Vector2 GetUpperLeft(Vector3 ray, Vector2 lowerLeft, Vector2 lowerRight, double aspectRatio)
//        {
//            var plane = _canvas as GraphicPlane;

//            Vector3 n = Helpers.Convert.NumericsVector3(plane._normal);
//            Vector3 bottomLeft = new Vector3(lowerLeft, 0);
//            Vector3 bottomRight = new Vector3(lowerRight, 0);

//            //determines whether we are viewing plan from infront or behind.  Texture will be placed so that image is not flipped
//            var sign = Math.Sign(Vector3.Dot(n, ray));

//            Vector3 bottomEdge = bottomRight - bottomLeft;

//            Vector3 leftEdge = (float)(sign / _image.AspectRatio) * Vector3.Cross(new Vector3(0, 0, 1), bottomEdge);

//            Vector3 topLeft = bottomLeft + leftEdge;

//            return new Vector2(topLeft.X, topLeft.Y);
//        }


//        private RayIntersection GetRayIntersection(int screenX, int screenY)
//        {
//            _camera.ConvertFromScreenToWorld(screenX, screenY, out double[] from, out double[] to);

//            var plane = _canvas as GraphicPlane;

//            return plane.GetRayIntersection(from, to);
//        }
//    }
//}
