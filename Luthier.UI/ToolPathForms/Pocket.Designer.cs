namespace Luthier.UI.ToolPathForms
{
    partial class Pocket
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownCutHeight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxTool = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownSafeHeight = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxSpindleDirection = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownSpindleSpeed = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownFeedRate = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownStepLength = new System.Windows.Forms.NumericUpDown();
            this.pocketToolPathPresenterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonSave = new System.Windows.Forms.Button();
            this.NumberOfPolygons = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCutHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSafeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpindleSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFeedRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pocketToolPathPresenterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected Polygons";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Cut Height";
            // 
            // numericUpDownCutHeight
            // 
            this.numericUpDownCutHeight.DecimalPlaces = 3;
            this.numericUpDownCutHeight.Location = new System.Drawing.Point(152, 145);
            this.numericUpDownCutHeight.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownCutHeight.Name = "numericUpDownCutHeight";
            this.numericUpDownCutHeight.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCutHeight.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tool";
            // 
            // comboBoxTool
            // 
            this.comboBoxTool.FormattingEnabled = true;
            this.comboBoxTool.Location = new System.Drawing.Point(152, 170);
            this.comboBoxTool.Name = "comboBoxTool";
            this.comboBoxTool.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTool.Sorted = true;
            this.comboBoxTool.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(152, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 20);
            this.textBox1.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Safe Height";
            // 
            // numericUpDownSafeHeight
            // 
            this.numericUpDownSafeHeight.DecimalPlaces = 3;
            this.numericUpDownSafeHeight.Location = new System.Drawing.Point(152, 197);
            this.numericUpDownSafeHeight.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownSafeHeight.Name = "numericUpDownSafeHeight";
            this.numericUpDownSafeHeight.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSafeHeight.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Spindle Direction";
            // 
            // comboBoxSpindleDirection
            // 
            this.comboBoxSpindleDirection.FormattingEnabled = true;
            this.comboBoxSpindleDirection.Location = new System.Drawing.Point(152, 224);
            this.comboBoxSpindleDirection.Name = "comboBoxSpindleDirection";
            this.comboBoxSpindleDirection.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSpindleDirection.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 257);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Spindle Speed";
            // 
            // numericUpDownSpindleSpeed
            // 
            this.numericUpDownSpindleSpeed.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownSpindleSpeed.Location = new System.Drawing.Point(152, 255);
            this.numericUpDownSpindleSpeed.Maximum = new decimal(new int[] {
            24000,
            0,
            0,
            0});
            this.numericUpDownSpindleSpeed.Name = "numericUpDownSpindleSpeed";
            this.numericUpDownSpindleSpeed.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSpindleSpeed.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Feed Rate";
            // 
            // numericUpDownFeedRate
            // 
            this.numericUpDownFeedRate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownFeedRate.Location = new System.Drawing.Point(152, 282);
            this.numericUpDownFeedRate.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.numericUpDownFeedRate.Name = "numericUpDownFeedRate";
            this.numericUpDownFeedRate.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownFeedRate.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 311);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Step Length";
            // 
            // numericUpDownStepLength
            // 
            this.numericUpDownStepLength.DecimalPlaces = 1;
            this.numericUpDownStepLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownStepLength.Location = new System.Drawing.Point(152, 309);
            this.numericUpDownStepLength.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownStepLength.Name = "numericUpDownStepLength";
            this.numericUpDownStepLength.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownStepLength.TabIndex = 17;
            // 
            // pocketToolPathPresenterBindingSource
            // 
            this.pocketToolPathPresenterBindingSource.DataSource = typeof(Luthier.Model.Presenter.PocketToolPathPresenter);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(218, 417);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 18;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // NumberOfPolygons
            // 
            this.NumberOfPolygons.AutoSize = true;
            this.NumberOfPolygons.Location = new System.Drawing.Point(152, 35);
            this.NumberOfPolygons.Name = "NumberOfPolygons";
            this.NumberOfPolygons.Size = new System.Drawing.Size(112, 13);
            this.NumberOfPolygons.TabIndex = 19;
            this.NumberOfPolygons.Text = "No Polygons Selected";
            // 
            // Pocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(305, 452);
            this.Controls.Add(this.NumberOfPolygons);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.numericUpDownStepLength);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numericUpDownFeedRate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numericUpDownSpindleSpeed);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxSpindleDirection);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownSafeHeight);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBoxTool);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownCutHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Pocket";
            this.Text = "Pocket ToolPath Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Pocket_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCutHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSafeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpindleSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFeedRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStepLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pocketToolPathPresenterBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownCutHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxTool;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownSafeHeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxSpindleDirection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownSpindleSpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownFeedRate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownStepLength;
        private System.Windows.Forms.BindingSource pocketToolPathPresenterBindingSource;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label NumberOfPolygons;
    }
}