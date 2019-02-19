using SharpDX.Windows;
using System;


namespace Luthier.Model.UIForms
{
    public partial class RenderForm3d : RenderForm
    {
        public RenderForm3d()
        {
            InitializeComponent();
        }

        public EventHandler DoCurveToolStripItem_Click;
        public EventHandler DoPlaneToolStripMenuItem_Click;
        public EventHandler DoSurfaceToolStripMenuItem_Click;
        public EventHandler DoPerspectiveToolStripMenuItem_Click;
        public EventHandler DoOrthonormalToolStripMenuItem_Click;
        public EventHandler DoDragParallelToPlaneToolStripMenuItem_Click;
        public EventHandler DoDragNormalToPlaneToolStripMenuItem_Click;
        public EventHandler DoLightingOptionsToolStripMenuItem_Click;
        public EventHandler DoInsertImageToolStripMenuItem_Click;
        public EventHandler DoSelectCanvasToolStripMenuItem_Click;
        public EventHandler DoObjectExplorerToolStripMenuItem_Click;
        public EventHandler DoPanToolStripMenuItem_Click;
        public EventHandler DoScaleModelStripMenuItem_Click;
        public EventHandler DoSurfaceDrawingStyleToolStripMenuItem_Click;
        public EventHandler DoCreateJoiningSurfaceToolStripMenuItem_Click;

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCurveToolStripItem_Click(sender, e);
        }

        private void planeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPlaneToolStripMenuItem_Click(sender, e);
        }

        private void perspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPerspectiveToolStripMenuItem_Click(sender, e);
        }

        private void orthonormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoOrthonormalToolStripMenuItem_Click(sender, e);
        }

        private void parallelToPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoDragParallelToPlaneToolStripMenuItem_Click(sender, e);
        }

        private void normalToPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoDragNormalToPlaneToolStripMenuItem_Click(sender, e);
        }

        private void surfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSurfaceToolStripMenuItem_Click(sender, e);
        }

        private void lightingOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoLightingOptionsToolStripMenuItem_Click(sender, e);
        }

        private void insertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoInsertImageToolStripMenuItem_Click(sender, e);
        }

        private void selectCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSelectCanvasToolStripMenuItem_Click(sender, e);
        }

        private void objectExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoObjectExplorerToolStripMenuItem_Click(sender, e);
        }

        private void RenderForm3d_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void panToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoPanToolStripMenuItem_Click(sender, e);
        }

        private void scaleModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoScaleModelStripMenuItem_Click(sender, e);
        }

        private void surfaceDrawingStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSurfaceDrawingStyleToolStripMenuItem_Click(sender, e);
        }

        private void createJoiningSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoCreateJoiningSurfaceToolStripMenuItem_Click(sender, e);
        }
    }
}
