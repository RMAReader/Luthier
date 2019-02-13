namespace Luthier.Model.UIForms
{
    partial class CoordinateDialog
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxXCoord = new System.Windows.Forms.TextBox();
            this.textBoxYCoord = new System.Windows.Forms.TextBox();
            this.textBoxZCoord = new System.Windows.Forms.TextBox();
            this.buttonConfirmValues = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Z:";
            // 
            // textBoxXCoord
            // 
            this.textBoxXCoord.Location = new System.Drawing.Point(37, 13);
            this.textBoxXCoord.Name = "textBoxXCoord";
            this.textBoxXCoord.Size = new System.Drawing.Size(100, 20);
            this.textBoxXCoord.TabIndex = 3;
            this.textBoxXCoord.TextChanged += new System.EventHandler(this.textBoxXCoord_TextChanged);
            // 
            // textBoxYCoord
            // 
            this.textBoxYCoord.Location = new System.Drawing.Point(37, 30);
            this.textBoxYCoord.Name = "textBoxYCoord";
            this.textBoxYCoord.Size = new System.Drawing.Size(100, 20);
            this.textBoxYCoord.TabIndex = 4;
            this.textBoxYCoord.TextChanged += new System.EventHandler(this.textBoxYCoord_TextChanged);
            // 
            // textBoxZCoord
            // 
            this.textBoxZCoord.Location = new System.Drawing.Point(37, 47);
            this.textBoxZCoord.Name = "textBoxZCoord";
            this.textBoxZCoord.Size = new System.Drawing.Size(100, 20);
            this.textBoxZCoord.TabIndex = 5;
            this.textBoxZCoord.TextChanged += new System.EventHandler(this.textBoxZCoord_TextChanged);
            // 
            // buttonConfirmValues
            // 
            this.buttonConfirmValues.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonConfirmValues.Location = new System.Drawing.Point(37, 69);
            this.buttonConfirmValues.Name = "buttonConfirmValues";
            this.buttonConfirmValues.Size = new System.Drawing.Size(100, 23);
            this.buttonConfirmValues.TabIndex = 6;
            this.buttonConfirmValues.Text = "Confirm values";
            this.buttonConfirmValues.UseVisualStyleBackColor = true;
            this.buttonConfirmValues.Click += new System.EventHandler(this.buttonConfirmValues_Click);
            // 
            // CoordinateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 104);
            this.Controls.Add(this.buttonConfirmValues);
            this.Controls.Add(this.textBoxZCoord);
            this.Controls.Add(this.textBoxYCoord);
            this.Controls.Add(this.textBoxXCoord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CoordinateDialog";
            this.Text = "CoordinateDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxXCoord;
        private System.Windows.Forms.TextBox textBoxYCoord;
        private System.Windows.Forms.TextBox textBoxZCoord;
        private System.Windows.Forms.Button buttonConfirmValues;
    }
}