namespace Luthier.UI
{
    partial class DrawingForm2D
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingForm2D));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Mode = new System.Windows.Forms.ToolStripDropDownButton();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.newLinkedLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCurveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCompositeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCompositePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.measureLengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Mode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(689, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Mode
            // 
            this.Mode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Mode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.newLinkedLineToolStripMenuItem,
            this.newCurveToolStripMenuItem,
            this.newSurfaceToolStripMenuItem,
            this.newPolygonToolStripMenuItem,
            this.newImageToolStripMenuItem,
            this.newCompositeToolStripMenuItem,
            this.newCompositePolygonToolStripMenuItem,
            this.toolStripMenuItem1,
            this.measureLengthToolStripMenuItem});
            this.Mode.Image = ((System.Drawing.Image)(resources.GetObject("Mode.Image")));
            this.Mode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(51, 22);
            this.Mode.Text = "Mode";
            this.Mode.ToolTipText = "Mode";
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.selectToolStripMenuItem.Text = "Select...";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(203, 6);
            // 
            // newLinkedLineToolStripMenuItem
            // 
            this.newLinkedLineToolStripMenuItem.Name = "newLinkedLineToolStripMenuItem";
            this.newLinkedLineToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newLinkedLineToolStripMenuItem.Text = "New Linked Line";
            this.newLinkedLineToolStripMenuItem.Click += new System.EventHandler(this.newLinkedLineToolStripMenuItem_Click);
            // 
            // newCurveToolStripMenuItem
            // 
            this.newCurveToolStripMenuItem.Name = "newCurveToolStripMenuItem";
            this.newCurveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newCurveToolStripMenuItem.Text = "New Curve";
            this.newCurveToolStripMenuItem.Click += new System.EventHandler(this.newCurveToolStripMenuItem_Click);
            // 
            // newPolygonToolStripMenuItem
            // 
            this.newPolygonToolStripMenuItem.Name = "newPolygonToolStripMenuItem";
            this.newPolygonToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newPolygonToolStripMenuItem.Text = "New Polygon";
            this.newPolygonToolStripMenuItem.Click += new System.EventHandler(this.newPolygonToolStripMenuItem_Click);
            // 
            // newImageToolStripMenuItem
            // 
            this.newImageToolStripMenuItem.Name = "newImageToolStripMenuItem";
            this.newImageToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newImageToolStripMenuItem.Text = "New Image";
            this.newImageToolStripMenuItem.Click += new System.EventHandler(this.newImageToolStripMenuItem_Click);
            // 
            // newCompositeToolStripMenuItem
            // 
            this.newCompositeToolStripMenuItem.Name = "newCompositeToolStripMenuItem";
            this.newCompositeToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newCompositeToolStripMenuItem.Text = "New Intersection";
            this.newCompositeToolStripMenuItem.Click += new System.EventHandler(this.newIntersectionToolStripMenuItem_Click);
            // 
            // newCompositePolygonToolStripMenuItem
            // 
            this.newCompositePolygonToolStripMenuItem.Name = "newCompositePolygonToolStripMenuItem";
            this.newCompositePolygonToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newCompositePolygonToolStripMenuItem.Text = "New Composite Polygon";
            this.newCompositePolygonToolStripMenuItem.Click += new System.EventHandler(this.newCompositePolygonToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // measureLengthToolStripMenuItem
            // 
            this.measureLengthToolStripMenuItem.Name = "measureLengthToolStripMenuItem";
            this.measureLengthToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.measureLengthToolStripMenuItem.Text = "Measure Length";
            this.measureLengthToolStripMenuItem.Click += new System.EventHandler(this.measureLengthToolStripMenuItem_Click);
            // 
            // newSurfaceToolStripMenuItem
            // 
            this.newSurfaceToolStripMenuItem.Name = "newSurfaceToolStripMenuItem";
            this.newSurfaceToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newSurfaceToolStripMenuItem.Text = "New Surface";
            this.newSurfaceToolStripMenuItem.Click += new System.EventHandler(this.newSurfaceToolStripMenuItem_Click);
            // 
            // DrawingForm2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 489);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "DrawingForm2D";
            this.Text = "DrawingForm2D";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawingForm2D_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawingForm2D_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawingForm2D_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawingForm2D_MouseUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Mode;
        private System.Windows.Forms.ToolStripMenuItem newLinkedLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem newCurveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem measureLengthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCompositeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCompositePolygonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSurfaceToolStripMenuItem;
    }
}