using Luthier.Geometry;
using Luthier.Model.GraphicObjects;
using Luthier.Model.GraphicObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{
    public class CalibrateDistanceController3D : SketchObjectBase
    {
        private double[] firstPoint;
        private double[] secondPoint;
        private GraphicPlane _plane;
        private double _requiredDistance;

        public CalibrateDistanceController3D(double requiredDistance)
        {
            _requiredDistance = requiredDistance;
        }
           

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                if (firstPoint == null)
                {
                    firstPoint = CalculateIntersection(e.X, e.Y);
                }
                else if (secondPoint == null)
                {
                    secondPoint = CalculateIntersection(e.X, e.Y);

                    var currentDistance = secondPoint.Subtract(firstPoint).L2Norm();

                    var scaleFactor = _requiredDistance / currentDistance;

                    foreach(IScalable obj in _model.Model.Where(x => x is IScalable))
                    {
                        obj.ScaleObject(scaleFactor);
                    }

                    _model.Model.HasChanged = true;
                }
            }
        }
            
    }

    
}
