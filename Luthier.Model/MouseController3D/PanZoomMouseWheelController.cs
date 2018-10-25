using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Model.GraphicObjects;
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
                _initialIntersection = new SharpDX.Vector3(
                    (float)intersection.IntersectInWorldCoords[0],
                    (float)intersection.IntersectInWorldCoords[1],
                    (float)intersection.IntersectInWorldCoords[2]);

                double[] origin = intersection.IntersectInWorldCoords;
                double[] pu = new double[] { 1, 0, 0 };
                double[] pv = new double[] { 0, 1, 0 };
                _facingPlane = GraphicPlane.CreateRightHandedThroughPoints(origin, pu, pv);

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

                double[] newIntersection = _facingPlane.GetRayIntersection(from, to).IntersectInWorldCoords;

                _camera.LookAt.X = _initialCamera.LookAt.X + _initialIntersection.X - (float)newIntersection[0];
                _camera.LookAt.Y = _initialCamera.LookAt.Y + _initialIntersection.Y - (float)newIntersection[1];
                _camera.LookAt.Z = _initialCamera.LookAt.Z + _initialIntersection.Z - (float)newIntersection[2];
            }
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            _facingPlane = null;
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
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
