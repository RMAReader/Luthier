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
    public class ControlPointDragger : IMouseController3D
    {
        private IApplicationDocumentModel _model;
        private Camera _camera;
        private double selectionRadius = 20;
        private List<IDraggable> selectedPoints = new List<IDraggable>();

        private int startX;
        private int startY;
        private double[] startWorldPoint;

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

                    foreach (var point in selectedPoints)
                    {
                        var dz = startY - Y;
                        point.Values = new double[]{ startWorldPoint[0], startWorldPoint[1], startWorldPoint[2] + dz};
                        _model.Model.HasChanged = true;
                    }
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



    }
}
