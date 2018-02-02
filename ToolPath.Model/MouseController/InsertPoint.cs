using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.Presenter;
using System.Drawing;

namespace Luthier.Model.MouseController
{
    public class InsertPoint : PanZoomMouseWheel
    {
        private readonly IPoint2DFactory factory;

        public InsertPoint(IPoint2DFactory factory)  { this.factory = factory; }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            factory.New(p.X,p.Y);
        }

    }
}
