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
    class SketchNurbsCurve : SketchObjectBase
    {
        private bool curveInProgress = false;
        private GraphicNurbsCurve _nurbsCurve;

    
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:

                    double[] p = CalculateIntersection(e.X, e.Y);

                    if (curveInProgress)
                    {
                        int lastCVIndex = _nurbsCurve.Curve.NumberOfPoints - 1;
                        _nurbsCurve.Curve.SetCV(lastCVIndex, p);
                        _nurbsCurve.Curve.ExtendBack(p);
                    }
                    else
                    {
                        _nurbsCurve = new GraphicNurbsCurve(dimension: 3, isRational: false, order: 3, cvCount: 2);
                        _nurbsCurve.Curve.SetCV(0, p);
                        _nurbsCurve.Curve.SetCV(1, p);

                        _model.Model.Add(_nurbsCurve);
                        curveInProgress = true;
                    }
                    break;

                case MouseButtons.Right:
                    curveInProgress = false;
                    break;
            }
            _model.Model.HasChanged = true;
        }


        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (curveInProgress)
            {
                double[] p = CalculateIntersection(e.X, e.Y);
                int lastCVIndex = _nurbsCurve.Curve.NumberOfPoints - 1;
                _nurbsCurve.Curve.SetCV(lastCVIndex, p);
                _model.Model.HasChanged = true;
            }
            
        }


    }
}
