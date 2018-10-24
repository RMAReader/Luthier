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
            this.curveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCanvasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dragPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parallelToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dragParallelToYZPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dragParallelToZXPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.constructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perspectiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orthonormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightingOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sketchToolStripMenuItem,
            this.constructToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sketchToolStripMenuItem
            // 
            this.sketchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.curveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectCanvasToolStripMenuItem,
            this.dragPointsToolStripMenuItem});
            this.sketchToolStripMenuItem.Name = "sketchToolStripMenuItem";
            this.sketchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.sketchToolStripMenuItem.Text = "Sketch";
            // 
            // curveToolStripMenuItem
            // 
            this.curveToolStripMenuItem.Name = "curveToolStripMenuItem";
            this.curveToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.curveToolStripMenuItem.Text = "Curve";
            this.curveToolStripMenuItem.Click += new System.EventHandler(this.curveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(141, 6);
            // 
            // selectCanvasToolStripMenuItem
            // 
            this.selectCanvasToolStripMenuItem.Name = "selectCanvasToolStripMenuItem";
            this.selectCanvasToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.selectCanvasToolStripMenuItem.Text = "Select canvas";
            this.selectCanvasToolStripMenuItem.Click += new System.EventHandler(this.selectCanvasToolStripMenuItem_Click);
            // 
            // dragPointsToolStripMenuItem
            // 
            this.dragPointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parallelToPlaneToolStripMenuItem,
            this.dragParallelToYZPlaneToolStripMenuItem,
            this.dragParallelToZXPlaneToolStripMenuItem,
            this.normalToPlaneToolStripMenuItem});
            this.dragPointsToolStripMenuItem.Name = "dragPointsToolStripMenuItem";
            this.dragPointsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.dragPointsToolStripMenuItem.Text = "Drag points...";
            // 
            // parallelToPlaneToolStripMenuItem
            // 
            this.parallelToPlaneToolStripMenuItem.Name = "parallelToPlaneToolStripMenuItem";
            this.parallelToPlaneToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.parallelToPlaneToolStripMenuItem.Text = "Drag parallel to XY plane";
            this.parallelToPlaneToolStripMenuItem.Click += new System.EventHandler(this.parallelToPlaneToolStripMenuItem_Click);
            // 
            // dragParallelToYZPlaneToolStripMenuItem
            // 
            this.dragParallelToYZPlaneToolStripMenuItem.Name = "dragParallelToYZPlaneToolStripMenuItem";
            this.dragParallelToYZPlaneToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.dragParallelToYZPlaneToolStripMenuItem.Text = "Drag parallel to YZ plane";
            this.dragParallelToYZPlaneToolStripMenuItem.Click += new System.EventHandler(this.dragParallelToYZPlaneToolStripMenuItem_Click);
            // 
            // dragParallelToZXPlaneToolStripMenuItem
            // 
            this.dragParallelToZXPlaneToolStripMenuItem.Name = "dragParallelToZXPlaneToolStripMenuItem";
            this.dragParallelToZXPlaneToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.dragParallelToZXPlaneToolStripMenuItem.Text = "Drag parallel to ZX plane";
            this.dragParallelToZXPlaneToolStripMenuItem.Click += new System.EventHandler(this.dragParallelToZXPlaneToolStripMenuItem_Click);
            // 
            // normalToPlaneToolStripMenuItem
            // 
            this.normalToPlaneToolStripMenuItem.Name = "normalToPlaneToolStripMenuItem";
            this.normalToPlaneToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.normalToPlaneToolStripMenuItem.Text = "Drag normal to plane";
            this.normalToPlaneToolStripMenuItem.Click += new System.EventHandler(this.normalToPlaneToolStripMenuItem_Click);
            // 
            // constructToolStripMenuItem
            // 
            this.constructToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.planeToolStripMenuItem,
            this.surfaceToolStripMenuItem,
            this.insertImageToolStripMenuItem});
            this.constructToolStripMenuItem.Name = "constructToolStripMenuItem";
            this.constructToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.constructToolStripMenuItem.Text = "Construct";
            // 
            // planeToolStripMenuItem
            // 
            this.planeToolStripMenuItem.Name = "planeToolStripMenuItem";
            this.planeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.planeToolStripMenuItem.Text = "Plane";
            this.planeToolStripMenuItem.Click += new System.EventHandler(this.planeToolStripMenuItem_Click);
            // 
            // surfaceToolStripMenuItem
            // 
            this.surfaceToolStripMenuItem.Name = "surfaceToolStripMenuItem";
            this.surfaceToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.surfaceToolStripMenuItem.Text = "Surface";
            this.surfaceToolStripMenuItem.Click += new System.EventHandler(this.surfaceToolStripMenuItem_Click);
            // 
            // insertImageToolStripMenuItem
            // 
            this.insertImageToolStripMenuItem.Name = "insertImageToolStripMenuItem";
            this.insertImageToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.insertImageToolStripMenuItem.Text = "Insert Image";
            this.insertImageToolStripMenuItem.Click += new System.EventHandler(this.insertImageToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perspectiveToolStripMenuItem,
            this.orthonormalToolStripMenuItem,
            this.lightingOptionsToolStripMenuItem,
            this.objectExplorerToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // perspectiveToolStripMenuItem
            // 
            this.perspectiveToolStripMenuItem.Name = "perspectiveToolStripMenuItem";
            this.perspectiveToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.perspectiveToolStripMenuItem.Text = "Perspective";
            this.perspectiveToolStripMenuItem.Click += new System.EventHandler(this.perspectiveToolStripMenuItem_Click);
            // 
            // orthonormalToolStripMenuItem
            // 
            this.orthonormalToolStripMenuItem.Name = "orthonormalToolStripMenuItem";
            this.orthonormalToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.orthonormalToolStripMenuItem.Text = "Orthonormal";
            this.orthonormalToolStripMenuItem.Click += new System.EventHandler(this.orthonormalToolStripMenuItem_Click);
            // 
            // lightingOptionsToolStripMenuItem
            // 
            this.lightingOptionsToolStripMenuItem.Name = "lightingOptionsToolStripMenuItem";
            this.lightingOptionsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.lightingOptionsToolStripMenuItem.Text = "Lighting options...";
            this.lightingOptionsToolStripMenuItem.Click += new System.EventHandler(this.lightingOptionsToolStripMenuItem_Click);
            // 
            // objectExplorerToolStripMenuItem
            // 
            this.objectExplorerToolStripMenuItem.Name = "objectExplorerToolStripMenuItem";
            this.objectExplorerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.objectExplorerToolStripMenuItem.Text = "Object Explorer";
            this.objectExplorerToolStripMenuItem.Click += new System.EventHandler(this.objectExplorerToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem dragParallelToYZPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dragParallelToZXPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightingOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCanvasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectExplorerToolStripMenuItem;
    }
}