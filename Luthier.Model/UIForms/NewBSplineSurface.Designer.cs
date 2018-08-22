namespace Luthier.Model.UIForms
{
    partial class NewBSplineSurface
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
            this.numericUpDownUControlPoints = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownVControlPoints = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUControlPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVControlPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of U control points";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of V control points";
            // 
            // numericUpDownUControlPoints
            // 
            this.numericUpDownUControlPoints.Location = new System.Drawing.Point(153, 19);
            this.numericUpDownUControlPoints.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownUControlPoints.Name = "numericUpDownUControlPoints";
            this.numericUpDownUControlPoints.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownUControlPoints.TabIndex = 3;
            this.numericUpDownUControlPoints.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDownVControlPoints
            // 
            this.numericUpDownVControlPoints.Location = new System.Drawing.Point(152, 48);
            this.numericUpDownVControlPoints.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownVControlPoints.Name = "numericUpDownVControlPoints";
            this.numericUpDownVControlPoints.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownVControlPoints.TabIndex = 4;
            this.numericUpDownVControlPoints.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(83, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Create new surface";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // NewBSplineSurface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 104);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDownVControlPoints);
            this.Controls.Add(this.numericUpDownUControlPoints);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewBSplineSurface";
            this.Text = "NewBSplineSurface";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUControlPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVControlPoints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownUControlPoints;
        private System.Windows.Forms.NumericUpDown numericUpDownVControlPoints;
        private System.Windows.Forms.Button button1;
    }
}