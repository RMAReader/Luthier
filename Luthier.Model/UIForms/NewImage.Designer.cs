namespace Luthier.Model.UIForms
{
    partial class NewImageForm
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
            this.imageFilePathTextBox = new System.Windows.Forms.TextBox();
            this.selectImageBrowseButton = new System.Windows.Forms.Button();
            this.transparencyLabel = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.imagePictureBox = new System.Windows.Forms.PictureBox();
            this.placeImageButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.imageFilePathTextBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.selectImageBrowseButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.transparencyLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.trackBar1, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(285, 74);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // imageFilePathTextBox
            // 
            this.imageFilePathTextBox.Location = new System.Drawing.Point(3, 3);
            this.imageFilePathTextBox.Name = "imageFilePathTextBox";
            this.imageFilePathTextBox.Size = new System.Drawing.Size(136, 20);
            this.imageFilePathTextBox.TabIndex = 0;
            this.imageFilePathTextBox.Text = "Enter image filepath...";
            this.imageFilePathTextBox.TextChanged += new System.EventHandler(this.imageFilePathTextBox_TextChanged);
            // 
            // selectImageBrowseButton
            // 
            this.selectImageBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectImageBrowseButton.Location = new System.Drawing.Point(145, 3);
            this.selectImageBrowseButton.Name = "selectImageBrowseButton";
            this.selectImageBrowseButton.Size = new System.Drawing.Size(137, 31);
            this.selectImageBrowseButton.TabIndex = 1;
            this.selectImageBrowseButton.Text = "Browse...";
            this.selectImageBrowseButton.UseVisualStyleBackColor = true;
            this.selectImageBrowseButton.Click += new System.EventHandler(this.selectImageBrowseButton_Click);
            // 
            // transparencyLabel
            // 
            this.transparencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.transparencyLabel.AutoSize = true;
            this.transparencyLabel.Location = new System.Drawing.Point(3, 37);
            this.transparencyLabel.Name = "transparencyLabel";
            this.transparencyLabel.Size = new System.Drawing.Size(136, 37);
            this.transparencyLabel.TabIndex = 2;
            this.transparencyLabel.Text = "Transparency";
            this.transparencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(145, 40);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(137, 31);
            this.trackBar1.TabIndex = 3;
            // 
            // imagePictureBox
            // 
            this.imagePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imagePictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.imagePictureBox.Location = new System.Drawing.Point(13, 97);
            this.imagePictureBox.Name = "imagePictureBox";
            this.imagePictureBox.Size = new System.Drawing.Size(285, 179);
            this.imagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagePictureBox.TabIndex = 1;
            this.imagePictureBox.TabStop = false;
            // 
            // placeImageButton
            // 
            this.placeImageButton.Location = new System.Drawing.Point(13, 283);
            this.placeImageButton.Name = "placeImageButton";
            this.placeImageButton.Size = new System.Drawing.Size(139, 38);
            this.placeImageButton.TabIndex = 2;
            this.placeImageButton.Text = "Place image";
            this.placeImageButton.UseVisualStyleBackColor = true;
            this.placeImageButton.Click += new System.EventHandler(this.placeImageButton_Click);
            // 
            // NewImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 333);
            this.Controls.Add(this.placeImageButton);
            this.Controls.Add(this.imagePictureBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewImage";
            this.Text = "NewImage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox imageFilePathTextBox;
        private System.Windows.Forms.Button selectImageBrowseButton;
        private System.Windows.Forms.Label transparencyLabel;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox imagePictureBox;
        private System.Windows.Forms.Button placeImageButton;
    }
}