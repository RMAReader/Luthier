using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Geometry;
using Luthier.Model.Extensions;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class PanZoomMouseWheelController : IMouseController3D
    {
        protected Camera _camera;
        protected IApplicationDocumentModel _model;

        private SharpDX.Vector3 _initialIntersection;
        private GraphicPlane _facingPlane;
        private Camera _initialCamera;
        private int _cumulativeDelta;

        public int X { get; private set; }

        public int Y { get; private set; }

        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
        }

        public void Bind(Camera wvp)
        {
            _camera = wvp;
        }

        public void Close()
        {
        }

        public void MouseClick(object sender, MouseEventArgs e)
        {
        }

        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && TryGetIntersection(e.X, e.Y, out RayIntersection intersection))
            {
               
                _initialIntersection = Helpers.Convert.SharpDXVector3(intersection.IntersectInWorldCoords);

                var cameraUp = _camera.CameraUp;
                var cameraRight = _camera.CameraRight;

                double[] origin = intersection.IntersectInWorldCoords;
                double[] u = Helpers.Convert.ToArray(cameraRight);
                double[] v = Helpers.Convert.ToArray(cameraUp);
                _facingPlane = GraphicPlane.CreateRightHandedThroughPoints(origin, origin.Add(u), origin.Add(v));

                _initialCamera = _camera.DeepCopy();
            }
            else if(e.Button == MouseButtons.Middle)
            {
                _cumulativeDelta = 0;
                _initialCamera = _camera.DeepCopy();
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {

            X = e.X;
            Y = e.Y;

            if (_facingPlane != null)
            {
                _initialCamera.ConvertFromScreenToWorld(e.X, e.Y, out double[] from, out double[] to);

                var newIntersection = Helpers.Convert.SharpDXVector3(_facingPlane.GetRayIntersection(from, to).IntersectInWorldCoords);

                _camera.LookAt = _initialCamera.LookAt + _initialIntersection - newIntersection;
            }
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            _facingPlane = null;
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            _cumulativeDelta += e.Delta;
            _camera.ZoomFactor = _initialCamera.ZoomFactor * (float)Math.Exp((double)_cumulativeDelta / 1000);
        }


        private bool TryGetIntersection(int screenX, int screenY, out RayIntersection intersection)
        {
            intersection = null;

            _camera.ConvertFromScreenToWorld(screenX, screenY, out double[] from, out double[] to);

            var selectedPlanes = new List<GraphicPlane>();
            var intersections = new List<RayIntersection>();
            foreach (ISelectable obj in _model.Model.VisibleObjects().Where(X => X is ISelectable))
            {
                var query = obj.GetRayIntersection(from, to);

                if (query.ObjectHit)
                    intersections.Add(query);

            }

            if (intersections.Count > 0)
            {
                intersection = intersections.OrderBy(x => x.RayParameter).First();
                return true;
            }
            return false;
        }
    }
}
