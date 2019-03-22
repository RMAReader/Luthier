namespace Luthier.Model.UIForms
{
    partial class RenderForm3d
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.curveDegree1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.curveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCanvasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dragPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parallelToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.constructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetCurveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compositeCurveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perspectiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthonormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightingOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surfaceDrawingStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createJoiningSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMouldOutlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.recalculateAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportGcodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sketchToolStripMenuItem,
            this.constructToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolPathToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // sketchToolStripMenuItem
            // 
            this.sketchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.curveDegree1ToolStripMenuItem,
            this.curveToolStripMenuItem,
            this.discToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectCanvasToolStripMenuItem,
            this.dragPointsToolStripMenuItem});
            this.sketchToolStripMenuItem.Name = "sketchToolStripMenuItem";
            this.sketchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.sketchToolStripMenuItem.Text = "Sketch";
            // 
            // curveDegree1ToolStripMenuItem
            // 
            this.curveDegree1ToolStripMenuItem.Name = "curveDegree1ToolStripMenuItem";
            this.curveDegree1ToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.curveDegree1ToolStripMenuItem.Text = "Curve (Degree 1)";
            this.curveDegree1ToolStripMenuItem.Click += new System.EventHandler(this.curveDegree1ToolStripMenuItem_Click);
            // 
            // curveToolStripMenuItem
            // 
            this.curveToolStripMenuItem.Name = "curveToolStripMenuItem";
            this.curveToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.curveToolStripMenuItem.Text = "Curve (Degree 2)";
            this.curveToolStripMenuItem.Click += new System.EventHandler(this.curveDegree2ToolStripMenuItem_Click);
            // 
            // discToolStripMenuItem
            // 
            this.discToolStripMenuItem.Name = "discToolStripMenuItem";
            this.discToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.discToolStripMenuItem.Text = "Disc";
            this.discToolStripMenuItem.Click += new System.EventHandler(this.discToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 6);
            // 
            // selectCanvasToolStripMenuItem
            // 
            this.selectCanvasToolStripMenuItem.Name = "selectCanvasToolStripMenuItem";
            this.selectCanvasToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.selectCanvasToolStripMenuItem.Text = "Select canvas";
            this.selectCanvasToolStripMenuItem.Click += new System.EventHandler(this.selectCanvasToolStripMenuItem_Click);
            // 
            // dragPointsToolStripMenuItem
            // 
            this.dragPointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parallelToPlaneToolStripMenuItem,
            this.normalToPlaneToolStripMenuItem});
            this.dragPointsToolStripMenuItem.Name = "dragPointsToolStripMenuItem";
            this.dragPointsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.dragPointsToolStripMenuItem.Text = "Drag points...";
            // 
            // parallelToPlaneToolStripMenuItem
            // 
            this.parallelToPlaneToolStripMenuItem.Name = "parallelToPlaneToolStripMenuItem";
            this.parallelToPlaneToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.parallelToPlaneToolStripMenuItem.Text = "Drag parallel to plane";
            this.parallelToPlaneToolStripMenuItem.Click += new System.EventHandler(this.parallelToPlaneToolStripMenuItem_Click);
            // 
            // normalToPlaneToolStripMenuItem
            // 
            this.normalToPlaneToolStripMenuItem.Name = "normalToPlaneToolStripMenuItem";
            this.normalToPlaneToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.normalToPlaneToolStripMenuItem.Text = "Drag normal to plane";
            this.normalToPlaneToolStripMenuItem.Click += new System.EventHandler(this.normalToPlaneToolStripMenuItem_Click);
            // 
            // constructToolStripMenuItem
            // 
            this.constructToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.planeToolStripMenuItem,
            this.surfaceToolStripMenuItem,
            this.insertImageToolStripMenuItem,
            this.offsetCurveToolStripMenuItem,
            this.compositeCurveToolStripMenuItem});
            this.constructToolStripMenuItem.Name = "constructToolStripMenuItem";
            this.constructToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.constructToolStripMenuItem.Text = "Construct";
            // 
            // planeToolStripMenuItem
            // 
            this.planeToolStripMenuItem.Name = "planeToolStripMenuItem";
            this.planeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.planeToolStripMenuItem.Text = "Plane";
            this.planeToolStripMenuItem.Click += new System.EventHandler(this.planeToolStripMenuItem_Click);
            // 
            // surfaceToolStripMenuItem
            // 
            this.surfaceToolStripMenuItem.Name = "surfaceToolStripMenuItem";
            this.surfaceToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.surfaceToolStripMenuItem.Text = "Surface";
            this.surfaceToolStripMenuItem.Click += new System.EventHandler(this.surfaceToolStripMenuItem_Click);
            // 
            // insertImageToolStripMenuItem
            // 
            this.insertImageToolStripMenuItem.Name = "insertImageToolStripMenuItem";
            this.insertImageToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.insertImageToolStripMenuItem.Text = "Insert Image";
            this.insertImageToolStripMenuItem.Click += new System.EventHandler(this.insertImageToolStripMenuItem_Click);
            // 
            // offsetCurveToolStripMenuItem
            // 
            this.offsetCurveToolStripMenuItem.Name = "offsetCurveToolStripMenuItem";
            this.offsetCurveToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.offsetCurveToolStripMenuItem.Text = "Offset Curve";
            this.offsetCurveToolStripMenuItem.Click += new System.EventHandler(this.offsetCurveToolStripMenuItem_Click);
            // 
            // compositeCurveToolStripMenuItem
            // 
            this.compositeCurveToolStripMenuItem.Name = "compositeCurveToolStripMenuItem";
            this.compositeCurveToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.compositeCurveToolStripMenuItem.Text = "Composite Curve";
            this.compositeCurveToolStripMenuItem.Click += new System.EventHandler(this.compositeCurveToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perspectiveToolStripMenuItem,
            this.orthonormalToolStripMenuItem,
            this.lightingOptionsToolStripMenuItem,
            this.objectExplorerToolStripMenuItem,
            this.panToolStripMenuItem,
            this.surfaceDrawingStyleToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // perspectiveToolStripMenuItem
            // 
            this.perspectiveToolStripMenuItem.Name = "perspectiveToolStripMenuItem";
            this.perspectiveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.perspectiveToolStripMenuItem.Text = "Perspective";
            this.perspectiveToolStripMenuItem.Click += new System.EventHandler(this.perspectiveToolStripMenuItem_Click);
            // 
            // orthonormalToolStripMenuItem
            // 
            this.orthonormalToolStripMenuItem.Name = "orthonormalToolStripMenuItem";
            this.orthonormalToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.orthonormalToolStripMenuItem.Text = "Orthonormal";
            this.orthonormalToolStripMenuItem.Click += new System.EventHandler(this.orthonormalToolStripMenuItem_Click);
            // 
            // lightingOptionsToolStripMenuItem
            // 
            this.lightingOptionsToolStripMenuItem.Name = "lightingOptionsToolStripMenuItem";
            this.lightingOptionsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.lightingOptionsToolStripMenuItem.Text = "Lighting options...";
            this.lightingOptionsToolStripMenuItem.Click += new System.EventHandler(this.lightingOptionsToolStripMenuItem_Click);
            // 
            // objectExplorerToolStripMenuItem
            // 
            this.objectExplorerToolStripMenuItem.Name = "objectExplorerToolStripMenuItem";
            this.objectExplorerToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.objectExplorerToolStripMenuItem.Text = "Object Explorer";
            this.objectExplorerToolStripMenuItem.Click += new System.EventHandler(this.objectExplorerToolStripMenuItem_Click);
            // 
            // panToolStripMenuItem
            // 
            this.panToolStripMenuItem.Name = "panToolStripMenuItem";
            this.panToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.panToolStripMenuItem.Text = "Pan...";
            this.panToolStripMenuItem.Click += new System.EventHandler(this.panToolStripMenuItem_Click);
            // 
            // surfaceDrawingStyleToolStripMenuItem
            // 
            this.surfaceDrawingStyleToolStripMenuItem.Name = "surfaceDrawingStyleToolStripMenuItem";
            this.surfaceDrawingStyleToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.surfaceDrawingStyleToolStripMenuItem.Text = "Surface drawing style...";
            this.surfaceDrawingStyleToolStripMenuItem.Click += new System.EventHandler(this.surfaceDrawingStyleToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scaleModelToolStripMenuItem,
            this.createJoiningSurfaceToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // scaleModelToolStripMenuItem
            // 
            this.scaleModelToolStripMenuItem.Name = "scaleModelToolStripMenuItem";
            this.scaleModelToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.scaleModelToolStripMenuItem.Text = "Scale model...";
            this.scaleModelToolStripMenuItem.Click += new System.EventHandler(this.scaleModelToolStripMenuItem_Click);
            // 
            // createJoiningSurfaceToolStripMenuItem
            // 
            this.createJoiningSurfaceToolStripMenuItem.Name = "createJoiningSurfaceToolStripMenuItem";
            this.createJoiningSurfaceToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.createJoiningSurfaceToolStripMenuItem.Text = "Create joining surface...";
            this.createJoiningSurfaceToolStripMenuItem.Click += new System.EventHandler(this.createJoiningSurfaceToolStripMenuItem_Click);
            // 
            // toolPathToolStripMenuItem
            // 
            this.toolPathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMouldOutlineToolStripMenuItem,
            this.toolStripMenuItem2,
            this.recalculateAllToolStripMenuItem,
            this.exportGcodeToolStripMenuItem});
            this.toolPathToolStripMenuItem.Name = "toolPathToolStripMenuItem";
            this.toolPathToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.toolPathToolStripMenuItem.Text = "Tool Path";
            // 
            // newMouldOutlineToolStripMenuItem
            // 
            this.newMouldOutlineToolStripMenuItem.Name = "newMouldOutlineToolStripMenuItem";
            this.newMouldOutlineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newMouldOutlineToolStripMenuItem.Text = "New mould outline";
            this.newMouldOutlineToolStripMenuItem.Click += new System.EventHandler(this.newMouldOutlineToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // recalculateAllToolStripMenuItem
            // 
            this.recalculateAllToolStripMenuItem.Name = "recalculateAllToolStripMenuItem";
            this.recalculateAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.recalculateAllToolStripMenuItem.Text = "Recalculate all";
            this.recalculateAllToolStripMenuItem.Click += new System.EventHandler(this.recalculateAllToolStripMenuItem_Click);
            // 
            // exportGcodeToolStripMenuItem
            // 
            this.exportGcodeToolStripMenuItem.Name = "exportGcodeToolStripMenuItem";
            this.exportGcodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportGcodeToolStripMenuItem.Text = "Export gcode";
            this.exportGcodeToolStripMenuItem.Click += new System.EventHandler(this.exportGcodeToolStripMenuItem_Click);
            // 
            // RenderForm3d
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RenderForm3d";
            this.Text = "RenderForm3d";
            this.MouseEnter += new System.EventHandler(this.RenderForm3d_MouseEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem curveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem constructToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem planeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perspectiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orthonormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dragPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parallelToPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightingOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCanvasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem panToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scaleModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surfaceDrawingStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createJoiningSurfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offsetCurveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem curveDegree1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compositeCurveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMouldOutlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem recalculateAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportGcodeToolStripMenuItem;
    }
}