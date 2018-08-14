using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController3D
{
    public class ControlPointDraggerParallelToPlane : ControlPointDraggerBase
    {

 
        protected override void OnMouseMoveLeftButtonDown()
        {
            var dragPlane = referencePlane.GetParallelPlaneThroughPoint(startWorldPoint);

            _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

            var intersectPoint = dragPlane.GetPointOfIntersectionWorld(from, to);

            foreach (var point in selectedPoints)
            {
                point.Values = intersectPoint;
                _model.Model.HasChanged = true;
            }
        }

       
    }
}
