using Luthier.Geometry;
using Luthier.Model.Extensions;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{

    public class EditImageController3d : SketchObjectBase
    {
        private GraphicImage3d _image;
        protected double selectionRadius = 20;
        protected int selectedIndex = -1;

        public EditImageController3d(GraphicImage3d image)
        {
            _image = image;
        }


        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                var p1 = _camera.ConvertFromWorldToScreen(_image.LowerLeft);
                double d1 = Math.Sqrt((p1[0] - X) * (p1[0] - X) + (p1[1] - Y) * (p1[1] - Y));

                var p2 = _camera.ConvertFromWorldToScreen(_image.LowerRight);
                double d2 = Math.Sqrt((p2[0] - X) * (p2[0] - X) + (p2[1] - Y) * (p2[1] - Y));

                if (d1 < selectionRadius) selectedIndex = 0;
                if (d2 < selectionRadius && d2 < d1) selectedIndex = 1;
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_image != null && selectedIndex != -1)
            {
                _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);


                if (selectedIndex == 0)
                {
                    _image.LowerLeft = _image.GetRayIntersection(from, to).IntersectInWorldCoords;
                }
                else
                {
                    _image.LowerRight = _image.GetRayIntersection(from, to).IntersectInWorldCoords;
                }
                

                var rayDirection = to.Subtract(from);

                _image.UpperLeft = GetUpperLeft(rayDirection, _image.LowerLeft, _image.LowerRight, _image.AspectRatio);
                _model.Model.HasChanged = true;
            }
        }


        public override void MouseUp(object sender, MouseEventArgs e)
        {
            selectedIndex = -1;
            base.MouseUp(sender, e);
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
