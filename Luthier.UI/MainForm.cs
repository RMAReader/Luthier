using Luthier.Model;
using Luthier.Model.Presenter;
using Luthier.UI.ToolPathForms;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.UI
{
    public partial class MainForm : Form
    {

        private IApplicationDocumentModel model;

        public MainForm(IApplicationDocumentModel model)
        {
            this.model = model;
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            byte[] bytes;
                            using (BinaryReader br = new BinaryReader(myStream))
                            {
                                bytes = br.ReadBytes((int)myStream.Length);
                            }
                            model.DeserialiseFromBytes(bytes, openFileDialog1.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    var bytes = model.SerialiseToBytes();
                    myStream.Write(bytes, 0, bytes.Length);

                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem_Click(sender, e);
            model.New();
        }

        private void toolPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var toolPathForm = new ToolPathManagerForm(new ToolPathManagerPresenter(model));
            toolPathForm.Show();
        }

        private void sketchPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var DrawingForm2D = new DrawingForm2D(new Drawing2DPresenter(model));
            DrawingForm2D.Show();
        }

        private void dViewportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var viewport3DPresenter = new ViewPort3DPresenter(model);

            viewport3DPresenter.ShowRenderForm();
        }
    }
}
