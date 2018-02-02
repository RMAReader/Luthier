using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.Presenter;
using System.Drawing;
using Luthier.Model.GraphicObjects;
using Luthier.Model.GraphicObjectFactory;

namespace Luthier.Model.MouseController
{
    public class InsertLengthGauge : PanZoomMouseWheel
    {
        private readonly LengthGaugeFactory factory;
        private GraphicLengthGauge gauge;
        public bool gaugeInProgress = false;


        public InsertLengthGauge(LengthGaugeFactory factory)
        {
            this.factory = factory;
        }


        public override void Close()
        {
            if (gaugeInProgress)
            {
                factory.Delete(gauge);
            }
        }

        public override void MouseLeftButtonDown(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (gaugeInProgress)
            {
                factory.SetEndPoint(gauge, p.X, p.Y);
                gaugeInProgress = false;
            }
            else
            {
                gauge = factory.New(p.X,p.Y);
                gaugeInProgress = true;
            }
        }

        public override void OnMouseMove(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            if (gaugeInProgress)
            {
                factory.SetEndPoint(gauge, p.X, p.Y);
            }
        }

    }
}
