using Luthier.Model.Extensions;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{
    public class InsertPlane : SketchObjectBase
    {

        private EnumSketchSurfaceStatus state;
        private double[] firstPoint;
        private double[] secondPoint;
      
      
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (state)
                    {
                        case EnumSketchSurfaceStatus.FirstPointPending:

                            firstPoint = CalculateIntersection(e.X, e.Y);
                            state = EnumSketchSurfaceStatus.SecondPointPending;
                            break;

                        case EnumSketchSurfaceStatus.SecondPointPending:
                            secondPoint = CalculateIntersection(e.X, e.Y);

                            var origin = firstPoint.Add(secondPoint).Multiply(0.5);
                            var pu = secondPoint;
                            var pv = origin.Add(base._canvas.GetNormalAtPointOfIntersectionWorld(null, null));

                            var plane = Plane.CreateRightHandedThroughPoints(origin, pu, pv);

                            _model.Model.Add(plane);

                            state = EnumSketchSurfaceStatus.FirstPointPending;
                            break;
                    }
                    break;

                case MouseButtons.Right:
                    state = EnumSketchSurfaceStatus.FirstPointPending;
                    break;
            }
        }

    }
}
