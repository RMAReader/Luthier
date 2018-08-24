namespace Luthier.Model.UIForms
{
    partial class LightingOptions
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.AmbientColorButton = new System.Windows.Forms.Button();
            this.ambientLabel = new System.Windows.Forms.Label();
            this.ambientCoeffTrackBar = new System.Windows.Forms.TrackBar();
            this.diffuseCoeffTrackBar = new System.Windows.Forms.TrackBar();
            this.specularCoeffTrackBar = new System.Windows.Forms.TrackBar();
            this.shininessTrackBar = new System.Windows.Forms.TrackBar();
            this.diffuseLabel = new System.Windows.Forms.Label();
            this.specularLabel = new System.Windows.Forms.Label();
            this.shininessLabel = new System.Windows.Forms.Label();
            this.DiffuseColorButton = new System.Windows.Forms.Button();
            this.SpecularColorButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ambientCoeffTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseCoeffTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.specularCoeffTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shininessTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.13726F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.86274F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Controls.Add(this.AmbientColorButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ambientLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ambientCoeffTrackBar, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.diffuseCoeffTrackBar, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.specularCoeffTrackBar, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.shininessTrackBar, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.diffuseLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.specularLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.shininessLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.DiffuseColorButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.SpecularColorButton, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(322, 124);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // AmbientColorButton
            // 
            this.AmbientColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AmbientColorButton.Location = new System.Drawing.Point(283, 3);
            this.AmbientColorButton.Name = "AmbientColorButton";
            this.AmbientColorButton.Size = new System.Drawing.Size(36, 25);
            this.AmbientColorButton.TabIndex = 1;
            this.AmbientColorButton.UseVisualStyleBackColor = true;
            this.AmbientColorButton.Click += new System.EventHandler(this.AmbientColorButton_Click);
            // 
            // ambientLabel
            // 
            this.ambientLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ambientLabel.AutoSize = true;
            this.ambientLabel.Location = new System.Drawing.Point(3, 0);
            this.ambientLabel.Name = "ambientLabel";
            this.ambientLabel.Size = new System.Drawing.Size(115, 31);
            this.ambientLabel.TabIndex = 0;
            this.ambientLabel.Text = "Ambient light";
            this.ambientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ambientCoeffTrackBar
            // 
            this.ambientCoeffTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ambientCoeffTrackBar.LargeChange = 20;
            this.ambientCoeffTrackBar.Location = new System.Drawing.Point(124, 3);
            this.ambientCoeffTrackBar.Maximum = 100;
            this.ambientCoeffTrackBar.Name = "ambientCoeffTrackBar";
            this.ambientCoeffTrackBar.Size = new System.Drawing.Size(153, 25);
            this.ambientCoeffTrackBar.SmallChange = 5;
            this.ambientCoeffTrackBar.TabIndex = 1;
            this.ambientCoeffTrackBar.TickFrequency = 10;
            this.ambientCoeffTrackBar.Value = 50;
            this.ambientCoeffTrackBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // diffuseCoeffTrackBar
            // 
            this.diffuseCoeffTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diffuseCoeffTrackBar.LargeChange = 20;
            this.diffuseCoeffTrackBar.Location = new System.Drawing.Point(124, 34);
            this.diffuseCoeffTrackBar.Maximum = 100;
            this.diffuseCoeffTrackBar.Name = "diffuseCoeffTrackBar";
            this.diffuseCoeffTrackBar.Size = new System.Drawing.Size(153, 25);
            this.diffuseCoeffTrackBar.SmallChange = 5;
            this.diffuseCoeffTrackBar.TabIndex = 2;
            this.diffuseCoeffTrackBar.TickFrequency = 10;
            this.diffuseCoeffTrackBar.Value = 50;
            this.diffuseCoeffTrackBar.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // specularCoeffTrackBar
            // 
            this.specularCoeffTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.specularCoeffTrackBar.LargeChange = 20;
            this.specularCoeffTrackBar.Location = new System.Drawing.Point(124, 65);
            this.specularCoeffTrackBar.Maximum = 100;
            this.specularCoeffTrackBar.Name = "specularCoeffTrackBar";
            this.specularCoeffTrackBar.Size = new System.Drawing.Size(153, 25);
            this.specularCoeffTrackBar.SmallChange = 5;
            this.specularCoeffTrackBar.TabIndex = 3;
            this.specularCoeffTrackBar.TickFrequency = 10;
            this.specularCoeffTrackBar.Value = 50;
            this.specularCoeffTrackBar.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // shininessTrackBar
            // 
            this.shininessTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shininessTrackBar.LargeChange = 20;
            this.shininessTrackBar.Location = new System.Drawing.Point(124, 96);
            this.shininessTrackBar.Maximum = 100;
            this.shininessTrackBar.Name = "shininessTrackBar";
            this.shininessTrackBar.Size = new System.Drawing.Size(153, 25);
            this.shininessTrackBar.TabIndex = 4;
            this.shininessTrackBar.TickFrequency = 10;
            this.shininessTrackBar.Value = 50;
            this.shininessTrackBar.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // diffuseLabel
            // 
            this.diffuseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diffuseLabel.AutoSize = true;
            this.diffuseLabel.Location = new System.Drawing.Point(3, 31);
            this.diffuseLabel.Name = "diffuseLabel";
            this.diffuseLabel.Size = new System.Drawing.Size(115, 31);
            this.diffuseLabel.TabIndex = 5;
            this.diffuseLabel.Text = "Diffuse reflection";
            this.diffuseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // specularLabel
            // 
            this.specularLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.specularLabel.AutoSize = true;
            this.specularLabel.Location = new System.Drawing.Point(3, 62);
            this.specularLabel.Name = "specularLabel";
            this.specularLabel.Size = new System.Drawing.Size(115, 31);
            this.specularLabel.TabIndex = 6;
            this.specularLabel.Text = "Specular reflection";
            this.specularLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shininessLabel
            // 
            this.shininessLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shininessLabel.AutoSize = true;
            this.shininessLabel.Location = new System.Drawing.Point(3, 93);
            this.shininessLabel.Name = "shininessLabel";
            this.shininessLabel.Size = new System.Drawing.Size(115, 31);
            this.shininessLabel.TabIndex = 7;
            this.shininessLabel.Text = "Specular shininess";
            this.shininessLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiffuseColorButton
            // 
            this.DiffuseColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DiffuseColorButton.Location = new System.Drawing.Point(283, 34);
            this.DiffuseColorButton.Name = "DiffuseColorButton";
            this.DiffuseColorButton.Size = new System.Drawing.Size(36, 25);
            this.DiffuseColorButton.TabIndex = 8;
            this.DiffuseColorButton.UseVisualStyleBackColor = true;
            this.DiffuseColorButton.Click += new System.EventHandler(this.DiffuseColorButton_Click);
            // 
            // SpecularColorButton
            // 
            this.SpecularColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpecularColorButton.Location = new System.Drawing.Point(283, 65);
            this.SpecularColorButton.Name = "SpecularColorButton";
            this.SpecularColorButton.Size = new System.Drawing.Size(36, 25);
            this.SpecularColorButton.TabIndex = 9;
            this.SpecularColorButton.UseVisualStyleBackColor = true;
            this.SpecularColorButton.Click += new System.EventHandler(this.SpecularColorButton_Click);
            // 
            // LightingOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 149);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LightingOptions";
            this.ShowInTaskbar = false;
            this.Text = "LightingOptions";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ambientCoeffTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffuseCoeffTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.specularCoeffTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shininessTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ambientLabel;
        private System.Windows.Forms.TrackBar ambientCoeffTrackBar;
        private System.Windows.Forms.TrackBar diffuseCoeffTrackBar;
        private System.Windows.Forms.TrackBar specularCoeffTrackBar;
        private System.Windows.Forms.TrackBar shininessTrackBar;
        private System.Windows.Forms.Label diffuseLabel;
        private System.Windows.Forms.Label specularLabel;
        private System.Windows.Forms.Label shininessLabel;
        private System.Windows.Forms.Button AmbientColorButton;
        private System.Windows.Forms.Button DiffuseColorButton;
        private System.Windows.Forms.Button SpecularColorButton;
    }
}