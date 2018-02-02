using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Model;
using Luthier.Model.MouseController;
using Luthier.Core;
using Luthier.UI.ToolPathForms;
using System.IO;
using Luthier.Model.Presenter;
using Luthier.UI.GraphicObjectForms;

namespace Luthier.UI
{
    public partial class DrawingForm2D : Form
    {

        //protected IApplicationDocumentModel model;
        private readonly Drawing2DPresenter presenter;

        public DrawingForm2D(Drawing2DPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
            this.MouseWheel += DrawingForm2D_MouseWheel;
        }

        public void ToolPathMode()
        {
            this.Mode.Available = false;
        }
        

        private void DrawingForm2D_Paint(object sender, PaintEventArgs e)
        {
            presenter.Paint(e.Graphics);
        }

        private void newLinkedLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InserLinekedLine();
            this.Cursor = Cursors.Cross;
        }

        private void newCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertBSplineCurv();
            this.Cursor = Cursors.Cross;
        }

        private void newPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertPolygon();
            this.Cursor = Cursors.Cross;
        }

        private void newImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertImage();
            this.Cursor = Cursors.Cross;
        }

        private void newIntersectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertIntersectionManual(20);
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertIntersectionAuto(20);
            this.Cursor = Cursors.Cross;
        }

        private void newCompositePolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertCompositePolygonAuto(20);
            this.Cursor = Cursors.Cross;
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().PointSelector(20);
            this.Cursor = Cursors.Hand;
        }

        private void measureLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.MouseController = presenter.GetModel().MouseControllerFactory().InsertLengthGauge();
            this.Cursor = Cursors.Cross;
        }

        private void DrawingForm2D_MouseDown(object sender, MouseEventArgs e)
        {
            presenter.MouseDown(sender, e);
            this.Invalidate();
        }

        private void DrawingForm2D_MouseUp(object sender, MouseEventArgs e)
        {
            presenter.MouseUp(sender, e);
            this.Invalidate();
        }

        private void DrawingForm2D_MouseMove(object sender, MouseEventArgs e)
        {
            presenter.MouseMove(sender, e);
            this.Invalidate();
        }

        private void DrawingForm2D_MouseWheel(object sender, MouseEventArgs e)
        {
            presenter.MouseWheel(sender, e);
            this.Invalidate();
        }
    }
}
