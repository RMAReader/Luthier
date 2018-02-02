using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController
{
    public enum PanAndZoomState
    {
        Inactive,
        Panning,
        Zooming
    }

    public class PanZoomMouseWheel : IMouseController
    {
        private PointF startPos;
        private ViewMapper2D startMapper;
        private int cumulativeDelta;

        protected PanAndZoomState panZoomState = PanAndZoomState.Inactive;

        public ViewMapper2D ViewMapper { get; set; }

        public void MouseMiddleButtonDown(int x, int y)
        {
            panZoomState = PanAndZoomState.Panning;

            startMapper = ViewMapper.Copy();
            startPos = new PointF(x, y);
        }

        public void MouseMiddleButtonUp(int x, int y)
        {
            panZoomState = PanAndZoomState.Inactive;
        }


        public void MouseMove(int x, int y)
        {
            if (panZoomState == PanAndZoomState.Panning)
            {
                ViewMapper.CopyPropertyValues(startMapper.PanView(startPos, new PointF(x, y)));
            }
            OnMouseMove(x, y);
        }

        public void MouseWheel(int x, int y, int delta)
        {
            if (panZoomState == PanAndZoomState.Inactive || x != startPos.X || y != startPos.Y)
            {
                cumulativeDelta = 0;
                startMapper = ViewMapper.Copy();
                startPos = new PointF(x, y);
                panZoomState = PanAndZoomState.Zooming;
            }
            cumulativeDelta += delta;
            ViewMapper.CopyPropertyValues(startMapper.ZoomFixedPoint(new PointF(x, y), Math.Exp((double)cumulativeDelta / 1000)));
        }

 



        public virtual void Close() { }
        public virtual void MouseLeftButtonDown(int x, int y) { }
        public virtual void MouseLeftButtonUp(int x, int y) { }
        public virtual void MouseRightButtonDown(int x, int y) { }
        public virtual void MouseRightButtonUp(int x, int y) { }

        public virtual void OnMouseMove(int x, int y) { }
    }
}
