using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController3D
{
    public class ControlPointDraggerNormalToPlane : ControlPointDraggerBase
    {

        protected override void OnMouseMoveLeftButtonDown()
        {
            foreach (var point in selectedPoints)
            {
                var dz = startY - Y;
                point.Values = new double[] { startWorldPoint[0], startWorldPoint[1], startWorldPoint[2] + dz };
                _model.Model.HasChanged = true;
            }
        }
    }
}
