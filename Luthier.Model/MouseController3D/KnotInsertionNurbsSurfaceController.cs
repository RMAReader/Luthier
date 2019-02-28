using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Geometry.Intersections;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class KnotInsertionNurbsSurfaceController : IMouseController3D
    {
        protected IApplicationDocumentModel _model;
        protected Camera _camera;

        protected RayIntersection _nearestIntersection;

        protected List<GraphicNurbsSurface> _activeSurfaces;

        public int X { get; private set; }

        public int Y { get; private set; }

        public KnotInsertionNurbsSurfaceController(GraphicNurbsSurface surface = null)
        {
            if(surface != null)
            {
                _activeSurfaces = new List<GraphicNurbsSurface> { surface };
            }
            else
            {
                _activeSurfaces = _model.Model.Select(x => x as GraphicNurbsSurface).Where(x => x != null && x.IsVisible == true).ToList();
            }
        }


        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
            foreach (GraphicNurbsSurface surface in EnumerateSurfaces())
            {
                surface.DrawKnotSpans = true;
            }
            _model.Model.HasChanged = true;
        }

        public void Bind(Camera wvp)
        {
            _camera = wvp;
        }

        public void Close()
        {
            foreach (GraphicNurbsSurface surface in EnumerateSurfaces())
            {
                surface.DrawKnotSpans = false;
            }
            _model.Model.HasChanged = true;
        }

        public void MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                TryInsertKnot();
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;

            UpdateNearestIntersection(X, Y);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            
        }


        private IEnumerable<GraphicNurbsSurface> EnumerateSurfaces()
        {
            return _activeSurfaces;
        }

        private void UpdateNearestIntersection(int screenX, int screenY)
        {
            _camera.ConvertFromScreenToWorld(screenX, screenY, out double[] from, out double[] to);

            _nearestIntersection = null;
            foreach (GraphicNurbsSurface surface in EnumerateSurfaces())
            {
                var intersection = surface.GetRayIntersection(from, to);
                if (intersection.ObjectHit)
                {
                    if (_nearestIntersection == null || intersection.RayParameter < _nearestIntersection.RayParameter)
                        _nearestIntersection = intersection;
                }
            }
        }


        private void TryInsertKnot()
        {
            if (_nearestIntersection != null)
            {
                var surface = _nearestIntersection.Object as GraphicNurbsSurface;

                var d0 = DistanceToNearestKnot(surface.Surface.knotArray0, _nearestIntersection.ObjectParameters[0]);
                var d1 = DistanceToNearestKnot(surface.Surface.knotArray1, _nearestIntersection.ObjectParameters[1]);

                if(d0 < d1)
                {
                    surface.Surface = surface.Surface.InsertKnot(1, new double[] { _nearestIntersection.ObjectParameters[1] });
                }
                else
                {
                    surface.Surface = surface.Surface.InsertKnot(0, new double[] { _nearestIntersection.ObjectParameters[0] });
                }

                _model.Model.HasChanged = true;
            }
        }

        private double DistanceToNearestKnot(double[] knot, double parameter)
        {
            return knot.Min(x => Math.Abs(x - parameter));
        }
    }
}
