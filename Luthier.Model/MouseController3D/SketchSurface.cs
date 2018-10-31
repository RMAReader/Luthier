using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{

    enum EnumSketchSurfaceStatus
    {
        FirstPointPending,
        SecondPointPending,
        ThirdPointPending
    }


    public class SketchSurface : SketchObjectBase
    {
        private EnumSketchSurfaceStatus state;
        private double[] firstPoint;
        private double[] secondPoint;
        private double[] thirdPoint;
        private int _cvCount0;
        private int _cvCount1;
        private GraphicNurbsSurface _surface;

        public SketchSurface(int cvCount0, int cvCount1)
        {
            _cvCount0 = cvCount0;
            _cvCount1 = cvCount1;
        }


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
                            state = EnumSketchSurfaceStatus.ThirdPointPending;
                            break;

                        case EnumSketchSurfaceStatus.ThirdPointPending:
                            thirdPoint = CalculateIntersection(e.X, e.Y);

                            var surface = BSplineFactory.CreateSurface(_cvCount0, _cvCount1, firstPoint, secondPoint, thirdPoint);

                            _model.Model.Add(surface);

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
