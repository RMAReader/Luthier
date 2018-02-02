//using Luthier.Model;
//using Luthier.Model.MouseController;
//using Luthier.Model.Presenter;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Luthier.UI
//{
//    public partial class Drawing2DMouseController : Form
//    {

//        public IMouseController MouseController { get; set; }
//        public IDrawing2DPainter Painter { get; set; }

//        public Drawing2DMouseController(IMouseController mouseController, IDrawing2DPainter painter)
//        {
//            this.MouseController = mouseController;
//            this.Painter = painter;
//            InitializeComponent();
//        }

//        private void Drawing2DMouseController_MouseDown(object sender, MouseEventArgs e)
//        {
//            switch (e.Button)
//            {
//                case MouseButtons.Left:
//                    MouseController.MouseLeftButtonDown(e.X, e.Y);
//                    break;

//                case MouseButtons.Right:
//                    MouseController.MouseRightButtonDown(e.X, e.Y);
//                    break;
//            }
//            Invalidate();
//        }

//        private void Drawing2DMouseController_MouseEnter(object sender, MouseEventArgs e)
//        {
            
//        }

//        private void Drawing2DMouseController_MouseHover(object sender, MouseEventArgs e)
//        {
            
//        }

//        private void Drawing2DMouseController_MouseLeave(object sender, MouseEventArgs e)
//        {
            
//        }

//        private void Drawing2DMouseController_MouseMove(object sender, MouseEventArgs e)
//        {
//            MouseController.MouseMove(e.X, e.Y);
//            Invalidate();
//        }

//        private void Drawing2DMouseController_MouseUp(object sender, MouseEventArgs e)
//        {
//            switch (e.Button)
//            {
//                case MouseButtons.Left:
//                    MouseController.MouseLeftButtonUp(e.X, e.Y);
//                    break;

//                case MouseButtons.Right:
//                    MouseController.MouseRightButtonUp(e.X, e.Y);
//                    break;
//            }
//            Invalidate();
//        }

//        private void Drawing2DMouseController_Paint(object sender, PaintEventArgs e)
//        {
//            Painter.Paint(e.Graphics);
//        }
//    }
//}
