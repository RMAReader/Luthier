namespace Luthier.Model.UIForms
{
    partial class DiscDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.radiusTextBox = new System.Windows.Forms.TextBox();
            this.confirmValueButton = new System.Windows.Forms.Button();
            this.xCoordLabel = new System.Windows.Forms.Label();
            this.yCoordLabel = new System.Windows.Forms.Label();
            this.zCoordLabel = new System.Windows.Forms.Label();
            this.xCoordTextBox = new System.Windows.Forms.TextBox();
            this.yCoordTextBox = new System.Windows.Forms.TextBox();
            this.zCoordTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Radius (mm):";
            // 
            // radiusTextBox
            // 
            this.radiusTextBox.Location = new System.Drawing.Point(88, 13);
            this.radiusTextBox.Name = "radiusTextBox";
            this.radiusTextBox.Size = new System.Drawing.Size(100, 20);
            this.radiusTextBox.TabIndex = 1;
            // 
            // confirmValueButton
            // 
            this.confirmValueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmValueButton.Location = new System.Drawing.Point(46, 109);
            this.confirmValueButton.Name = "confirmValueButton";
            this.confirmValueButton.Size = new System.Drawing.Size(114, 23);
            this.confirmValueButton.TabIndex = 2;
            this.confirmValueButton.Text = "Confirm Value";
            this.confirmValueButton.UseVisualStyleBackColor = true;
            this.confirmValueButton.Click += new System.EventHandler(this.confirmValueButton_Click);
            // 
            // xCoordLabel
            // 
            this.xCoordLabel.AutoSize = true;
            this.xCoordLabel.Location = new System.Drawing.Point(16, 44);
            this.xCoordLabel.Name = "xCoordLabel";
            this.xCoordLabel.Size = new System.Drawing.Size(14, 13);
            this.xCoordLabel.TabIndex = 3;
            this.xCoordLabel.Text = "X";
            // 
            // yCoordLabel
            // 
            this.yCoordLabel.AutoSize = true;
            this.yCoordLabel.Location = new System.Drawing.Point(16, 57);
            this.yCoordLabel.Name = "yCoordLabel";
            this.yCoordLabel.Size = new System.Drawing.Size(14, 13);
            this.yCoordLabel.TabIndex = 4;
            this.yCoordLabel.Text = "Y";
            // 
            // zCoordLabel
            // 
            this.zCoordLabel.AutoSize = true;
            this.zCoordLabel.Location = new System.Drawing.Point(16, 74);
            this.zCoordLabel.Name = "zCoordLabel";
            this.zCoordLabel.Size = new System.Drawing.Size(14, 13);
            this.zCoordLabel.TabIndex = 5;
            this.zCoordLabel.Text = "Z";
            // 
            // xCoordTextBox
            // 
            this.xCoordTextBox.Location = new System.Drawing.Point(88, 40);
            this.xCoordTextBox.Name = "xCoordTextBox";
            this.xCoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.xCoordTextBox.TabIndex = 6;
            // 
            // yCoordTextBox
            // 
            this.yCoordTextBox.Location = new System.Drawing.Point(88, 57);
            this.yCoordTextBox.Name = "yCoordTextBox";
            this.yCoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.yCoordTextBox.TabIndex = 7;
            // 
            // zCoordTextBox
            // 
            this.zCoordTextBox.Location = new System.Drawing.Point(88, 74);
            this.zCoordTextBox.Name = "zCoordTextBox";
            this.zCoordTextBox.Size = new System.Drawing.Size(100, 20);
            this.zCoordTextBox.TabIndex = 8;
            // 
            // DiscDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 144);
            this.Controls.Add(this.zCoordTextBox);
            this.Controls.Add(this.yCoordTextBox);
            this.Controls.Add(this.xCoordTextBox);
            this.Controls.Add(this.zCoordLabel);
            this.Controls.Add(this.yCoordLabel);
            this.Controls.Add(this.xCoordLabel);
            this.Controls.Add(this.confirmValueButton);
            this.Controls.Add(this.radiusTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DiscDialog";
            this.Text = "DiscDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox radiusTextBox;
        private System.Windows.Forms.Button confirmValueButton;
        private System.Windows.Forms.Label xCoordLabel;
        private System.Windows.Forms.Label yCoordLabel;
        private System.Windows.Forms.Label zCoordLabel;
        private System.Windows.Forms.TextBox xCoordTextBox;
        private System.Windows.Forms.TextBox yCoordTextBox;
        private System.Windows.Forms.TextBox zCoordTextBox;
    }
}