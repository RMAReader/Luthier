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
    class SketchNurbsCurve : IMouseController3D
    {
        private Camera _camera;
        private IApplicationDocumentModel _model;
        private bool curveInProgress = false;
        private NurbsCurve _nurbsCurve;
        private ISketchCanvas _canvas = Plane.CreateRightHandedXY(new double[] { 0, 0, 0 });

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

                    double[] p = CalculateIntersection(e.X, e.Y);

                    if (curveInProgress)
                    {
                        int lastCVIndex = _nurbsCurve.NumberOfPoints - 1;
                        _nurbsCurve.SetCV(lastCVIndex, p);
                        _nurbsCurve.ExtendBack(p);
                    }
                    else
                    {
                        _nurbsCurve = new NurbsCurve(dimension: 3, isRational: false, order: 3, cvCount: 2);
                        _nurbsCurve.SetCV(0, p);
                        _nurbsCurve.SetCV(1, p);

                        _model.Model.Objects.Add(_nurbsCurve);
                        curveInProgress = true;
                    }
                    break;

                case MouseButtons.Right:
                    curveInProgress = false;
                    break;
            }
            _model.Model.HasChanged = true;
        }


        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;

            if (curveInProgress)
            {
                double[] p = CalculateIntersection(e.X, e.Y);
                int lastCVIndex = _nurbsCurve.NumberOfPoints - 1;
                _nurbsCurve.SetCV(lastCVIndex, p);
                _model.Model.HasChanged = true;
            }
            
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            
        }


        //TODO: update so can draw on any plane, rather than just XY plane
        private double[] CalculateIntersection(int screenX, int screenY)
        {
            _camera.ConvertFromScreenToWorld(screenX, screenY, out double[] from, out double[] to);

            //var roundtripFrom = _camera.ConvertFromWorldToScreen(from);
            //var roundtripTo = _camera.ConvertFromWorldToScreen(to);

            //double propFrom = to[2] / (to[2] - from[2]);

            //return new double[] { from[0] * propFrom + to[0] * (1 - propFrom), from[1] * propFrom + to[1] * (1 - propFrom), 0 };

            return _canvas.GetPointOfIntersectionWorld(from, to);
        }

    }
}
