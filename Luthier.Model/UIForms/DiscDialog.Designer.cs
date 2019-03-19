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
            this.confirmValueButton.Location = new System.Drawing.Point(49, 40);
            this.confirmValueButton.Name = "confirmValueButton";
            this.confirmValueButton.Size = new System.Drawing.Size(114, 23);
            this.confirmValueButton.TabIndex = 2;
            this.confirmValueButton.Text = "Confirm Value";
            this.confirmValueButton.UseVisualStyleBackColor = true;
            this.confirmValueButton.Click += new System.EventHandler(this.confirmValueButton_Click);
            // 
            // DiscDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 72);
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
    }
}