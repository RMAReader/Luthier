namespace Luthier.Model.UIForms
{
    partial class ObjectExplorerForm
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
            this.components = new System.ComponentModel.Container();
            this.modelTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNewLayerSeparater = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertKnotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dragControlPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parallelToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extendFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extendBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditSeparater = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToGcodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recalculateToolpathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelTreeView
            // 
            this.modelTreeView.AllowDrop = true;
            this.modelTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelTreeView.CheckBoxes = true;
            this.modelTreeView.ContextMenuStrip = this.contextMenuStrip1;
            this.modelTreeView.LabelEdit = true;
            this.modelTreeView.Location = new System.Drawing.Point(12, 12);
            this.modelTreeView.Name = "modelTreeView";
            this.modelTreeView.ShowNodeToolTips = true;
            this.modelTreeView.Size = new System.Drawing.Size(266, 510);
            this.modelTreeView.TabIndex = 0;
            this.modelTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.modelTreeView_AfterLabelEdit);
            this.modelTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.modelTreeView_AfterCheck);
            this.modelTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.modelTreeView_ItemDrag);
            this.modelTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.modelTreeView_DragDrop);
            this.modelTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.modelTreeView_DragEnter);
            this.modelTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.modelTreeView_DragOver);
            this.modelTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelTreeView_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newLayerToolStripMenuItem,
            this.toolStripMenuItemNewLayerSeparater,
            this.editToolStripMenuItem,
            this.toolStripMenuItemEditSeparater,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.exportToGcodeToolStripMenuItem,
            this.recalculateToolpathToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 170);
            // 
            // newLayerToolStripMenuItem
            // 
            this.newLayerToolStripMenuItem.Name = "newLayerToolStripMenuItem";
            this.newLayerToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.newLayerToolStripMenuItem.Text = "New Layer...";
            this.newLayerToolStripMenuItem.Click += new System.EventHandler(this.newLayerToolStripMenuItem_Click);
            // 
            // toolStripMenuItemNewLayerSeparater
            // 
            this.toolStripMenuItemNewLayerSeparater.Name = "toolStripMenuItemNewLayerSeparater";
            this.toolStripMenuItemNewLayerSeparater.Size = new System.Drawing.Size(179, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertKnotToolStripMenuItem,
            this.dragControlPointsToolStripMenuItem,
            this.extendFrontToolStripMenuItem,
            this.extendBackToolStripMenuItem,
            this.reverseToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // insertKnotToolStripMenuItem
            // 
            this.insertKnotToolStripMenuItem.Name = "insertKnotToolStripMenuItem";
            this.insertKnotToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.insertKnotToolStripMenuItem.Text = "Insert Knot";
            this.insertKnotToolStripMenuItem.Click += new System.EventHandler(this.insertKnotToolStripMenuItem_Click);
            // 
            // dragControlPointsToolStripMenuItem
            // 
            this.dragControlPointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parallelToPlaneToolStripMenuItem,
            this.normalToPlaneToolStripMenuItem});
            this.dragControlPointsToolStripMenuItem.Name = "dragControlPointsToolStripMenuItem";
            this.dragControlPointsToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.dragControlPointsToolStripMenuItem.Text = "Drag control points";
            // 
            // parallelToPlaneToolStripMenuItem
            // 
            this.parallelToPlaneToolStripMenuItem.Name = "parallelToPlaneToolStripMenuItem";
            this.parallelToPlaneToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.parallelToPlaneToolStripMenuItem.Text = "Parallel to plane";
            this.parallelToPlaneToolStripMenuItem.Click += new System.EventHandler(this.parallelToPlaneToolStripMenuItem_Click);
            // 
            // normalToPlaneToolStripMenuItem
            // 
            this.normalToPlaneToolStripMenuItem.Name = "normalToPlaneToolStripMenuItem";
            this.normalToPlaneToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.normalToPlaneToolStripMenuItem.Text = "Normal to plane";
            this.normalToPlaneToolStripMenuItem.Click += new System.EventHandler(this.normalToPlaneToolStripMenuItem_Click);
            // 
            // extendFrontToolStripMenuItem
            // 
            this.extendFrontToolStripMenuItem.Name = "extendFrontToolStripMenuItem";
            this.extendFrontToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.extendFrontToolStripMenuItem.Text = "Extend front";
            this.extendFrontToolStripMenuItem.Click += new System.EventHandler(this.extendFrontToolStripMenuItem_Click);
            // 
            // extendBackToolStripMenuItem
            // 
            this.extendBackToolStripMenuItem.Name = "extendBackToolStripMenuItem";
            this.extendBackToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.extendBackToolStripMenuItem.Text = "Extend back";
            this.extendBackToolStripMenuItem.Click += new System.EventHandler(this.extendBackToolStripMenuItem_Click);
            // 
            // reverseToolStripMenuItem
            // 
            this.reverseToolStripMenuItem.Name = "reverseToolStripMenuItem";
            this.reverseToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.reverseToolStripMenuItem.Text = "Reverse";
            this.reverseToolStripMenuItem.Click += new System.EventHandler(this.reverseToolStripMenuItem_Click);
            // 
            // toolStripMenuItemEditSeparater
            // 
            this.toolStripMenuItemEditSeparater.Name = "toolStripMenuItemEditSeparater";
            this.toolStripMenuItemEditSeparater.Size = new System.Drawing.Size(179, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // exportToGcodeToolStripMenuItem
            // 
            this.exportToGcodeToolStripMenuItem.Name = "exportToGcodeToolStripMenuItem";
            this.exportToGcodeToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.exportToGcodeToolStripMenuItem.Text = "Export to gcode";
            this.exportToGcodeToolStripMenuItem.Click += new System.EventHandler(this.exportToGcodeToolStripMenuItem_Click);
            // 
            // recalculateToolpathToolStripMenuItem
            // 
            this.recalculateToolpathToolStripMenuItem.Name = "recalculateToolpathToolStripMenuItem";
            this.recalculateToolpathToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.recalculateToolpathToolStripMenuItem.Text = "Recalculate toolpath";
            this.recalculateToolpathToolStripMenuItem.Click += new System.EventHandler(this.recalculateToolpathToolStripMenuItem_Click);
            // 
            // ObjectExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 534);
            this.Controls.Add(this.modelTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ObjectExplorerForm";
            this.Text = "ObjectExplorerForm";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView modelTreeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemEditSeparater;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemNewLayerSeparater;
        private System.Windows.Forms.ToolStripMenuItem insertKnotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dragControlPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parallelToPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extendFrontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extendBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToGcodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recalculateToolpathToolStripMenuItem;
    }
}