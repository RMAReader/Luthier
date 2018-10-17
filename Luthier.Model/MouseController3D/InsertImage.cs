using Luthier.Model.Extensions;
using Luthier.Model.GraphicObjects;
using SharpDX;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                                _model.Model.Add(_image);

                                state = EnumSketchSurfaceStatus.SecondPointPending;
                            }
                            break;

                        case EnumSketchSurfaceStatus.SecondPointPending:
                            if (_image != null)
                            {
                                _image.LowerRight = CalculateIntersection(e.X, e.Y);
                                _image.UpperLeft = GetUpperLeft(_image.LowerLeft, _image.LowerRight, _image.AspectRatio);
                                state = EnumSketchSurfaceStatus.FirstPointPending;
                                _image = null;
                            }
                            break;

                    }
                    break;

                case MouseButtons.Right:
                    if (_image != null)
                    {
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
                _image.LowerRight = CalculateIntersection(e.X, e.Y);
                _image.UpperLeft = GetUpperLeft(_image.LowerLeft, _image.LowerRight, _image.AspectRatio);
            }
        }


        private double[] GetUpperLeft(double[] lowerLeft, double[] lowerRight, double aspectRatio)
        {
            var normal = _canvas.GetNormalAtPointOfIntersectionWorld(null, null);

            double[] bottomEdge = lowerRight.Subtract(lowerLeft);
            Vector3 axis = new Vector3((float)normal[0], (float)normal[1], (float)normal[2]);
            var rotation = Matrix.RotationAxis(axis, (float)Math.PI / 2);
            double[] leftEdge4 = SharpUtilities.Mul(rotation, bottomEdge);
            double[] leftEdge3 = new double[] { leftEdge4[0], leftEdge4[1], leftEdge4[2] };

            return _image.LowerLeft.Add(leftEdge3.Multiply(1 / _image.AspectRatio));
        }
    }
}
