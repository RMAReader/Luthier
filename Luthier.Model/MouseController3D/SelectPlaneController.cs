using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class SelectPlaneController : IMouseController3D
    {
        protected Camera _camera;
        protected IApplicationDocumentModel _model;
 
        public int X { get; private set; }

        public int Y { get; private set; }

        public GraphicPlane Plane { get; private set; }

        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
        }

        public void Bind(Camera camera)
        {
            _camera = camera;
        }

        public virtual void Close()
        {

        }

        public virtual void MouseClick(object sender, MouseEventArgs e)
        {
            _camera.ConvertFromScreenToWorld(e.X, e.Y, out double[] from, out double[] to);

            var selectedPlanes = new List<GraphicPlane>();
            var intersections = new List<RayIntersection>();
            foreach (GraphicPlane plane in _model.Model.VisibleObjects().Where(X => X is GraphicPlane))
            {
                var query = plane.GetRayIntersection(from, to);

                if (query.ObjectHit)
                    intersections.Add(query);

                if (plane.IsSelected)
                    selectedPlanes.Add(plane);
            }

            if (intersections.Count > 0)
            {
                foreach(var plane in selectedPlanes)
                    plane.IsSelected = false;

                Plane = (GraphicPlane)intersections.OrderBy(x => x.RayParameter).First().Object;
                Plane.IsSelected = true;
                _model.Model.HasChanged = true;
            }
        }

        public virtual void MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public virtual void MouseDown(object sender, MouseEventArgs e)
        {

        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
            OnMouseMove(sender, e);
        }

        public virtual void MouseUp(object sender, MouseEventArgs e)
        {
        }

        public virtual void MouseWheel(object sender, MouseEventArgs e)
        {
        }



        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
        }


       
    }
}
