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
    public class ControlPointDraggerBase : IMouseController3D
    {
        protected IApplicationDocumentModel _model;
        protected Camera _camera;
        protected double selectionRadius = 20;
        protected List<IDraggable> selectedPoints = new List<IDraggable>();

        protected int startX;
        protected int startY;
        protected double[] startWorldPoint;

        public Plane referencePlane { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
        }

        public void Bind(Camera camera)
        {
            _camera = camera;
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
            switch (e.Button)
            {
                case MouseButtons.Left:

                    startX = e.X;
                    startY = e.Y;

                    _camera.ConvertFromScreenToWorld(e.X, e.Y, out double[] from, out double[] to);


                    double distance = double.MaxValue;
                    foreach (IDraggable point in _model.Model.GetDraggableObjects())
                    {
                        var p = _camera.ConvertFromWorldToScreen(point.Values);
                        double d = Math.Sqrt((p[0] - e.X) * (p[0] - e.X) + (p[1] - e.Y) * (p[1] - e.Y));
                        if (d < selectionRadius && d < distance)
                        {
                            selectedPoints.Clear();
                            selectedPoints.Add(point);
                            distance = d;
                            startWorldPoint = point.Values;
                        }
                    }
                    break;
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;

            switch (e.Button)
            {
                case MouseButtons.Left:

                    OnMouseMoveLeftButtonDown();
                    break;
            }
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            selectedPoints.Clear();
            _model.Model.HasChanged = true;
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            
        }


        protected virtual void OnMouseMoveLeftButtonDown() { }
    }
}
