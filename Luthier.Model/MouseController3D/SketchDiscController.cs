using Luthier.Geometry;
using Luthier.Model.GraphicObjects;
using Luthier.Model.UIForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{
    public class SketchDiscController : SketchObjectBase
    {
        protected GraphicDisc _disc;


        public override void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:

                    Point3D p = new Point3D(CalculateIntersection(e.X, e.Y));

                    if (_disc != null)
                    {
                         _disc.Disc.Radius = (p - _disc.Disc.Centre).L2Norm();
                        _disc = null;
                    }
                    else
                    {
                        _disc = new GraphicDisc { Disc = new Geometry.Disc { Centre = p, Normal = new Geometry.Point3D(GetNormal(e.X, e.Y)), Radius = 0 }};
                        _model.Model.Add(_disc);
                    }
                    break;

                case MouseButtons.Right:

                    if (_disc != null)
                    {
                        var dialog = new DiscDialog(_disc.Disc);
                        dialog.StartPosition = FormStartPosition.Manual;
                        dialog.Location = new System.Drawing.Point(e.X, e.Y);
                        dialog.ShowDialog();
                        if (dialog.DialogResult == DialogResult.OK)
                        {
                            _disc.Disc = dialog.Disc.DeepCopy();
                            _disc = null;
                        }
                    }
                    break;
            }
            _model.Model.HasChanged = true;
        }


        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_disc != null)
            {
                Point3D p = new Point3D(CalculateIntersection(e.X, e.Y));
                _disc.Disc.Radius = (p - _disc.Disc.Centre).L2Norm();
                _model.Model.HasChanged = true;
            }
        }


        protected double[] GetNormal (int screenX, int screenY)
        {
            _camera.ConvertFromScreenToWorld(screenX, screenY, out double[] from, out double[] to);

            return _canvas.GetNormalAtPointOfIntersectionWorld(from, to);
        }
    }
}
