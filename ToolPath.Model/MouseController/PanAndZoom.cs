//using Luthier.Model.Presenter;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Luthier.Model.MouseController
//{




//    public class PanAndZoom : IMouseController
//    {
//        private PanAndZoomState state;
//        private PointF startPos;
//        private ViewMapper2D startMapper;

//        public ViewMapper2D ViewMapper { get; set; }


//        public void Close() {  }


//        public void MouseLeftButtonDown(int x, int y)
//        {
//            state = PanAndZoomState.Panning;
//            startMapper = ViewMapper.Copy();
//            startPos = new PointF(x, y);
//        }

//        public void MouseLeftButtonUp(int x, int y)
//        {
//        }

//        public void MouseMiddleButtonDown(int x, int y)
//        {
//        }

//        public void MouseMiddleButtonUp(int x, int y)
//        {
//        }

//        public void MouseMove(int x, int y)
//        {
//            switch(state)
//            {
//                case PanAndZoomState.Panning:
//                    ViewMapper.CopyPropertyValues(startMapper.PanView(startPos, new PointF(x, y)));
//                    break;

//                case PanAndZoomState.Zooming:
//                    //ViewMapper.CopyPropertyValues(startMapper.ZoomFixedPoint(startPos, (double) Math.Exp((y - startPos.Y) / 100)));
//                    break;
//            }
//        }

//        public void MouseRightButtonDown(int x, int y)
//        {
//            state = PanAndZoomState.Zooming;
//            startMapper = ViewMapper.Copy();
//            startPos = new PointF(x, y);
//        }

//        public void MouseRightButtonUp(int x, int y)
//        {
//            state = PanAndZoomState.Inactive;
//        }

//        public void MouseWheel(int x, int y, int delta)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
