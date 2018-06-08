using Luthier.Model.Presenter;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.MouseController3D
{
    class OrbitZoomMouseWheel : IMouseController3D
    {
        private IApplicationDocumentModel model;
        private Camera wvp;

        private float startx, starty;
        private float startCumPitch, startCumYaw;
        private bool panning = false;
        private Matrix initialView;

        public void Bind(IApplicationDocumentModel model)
        {
            this.model = model;
        }
        public void Bind(Camera wvp)
        {
            this.wvp = wvp;
        }


        public int X { get; private set; }
        public int Y { get; private set; }


        public void Close()
        {
           
        }

        public void MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    //startx = e.X;
                    //starty = e.Y;
                    //startCumPitch = wvp.CumulativePitch;
                    //startCumYaw = wvp.CumulativeYaw;
                    //initialView = wvp.View;
                    //panning = true;
                break;
            }

            
        }


        public void MouseMove(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    //wvp.CumulativePitch = startCumPitch + (e.Y - starty)/300;
                    //wvp.CumulativeYaw = startCumYaw + (e.X - startx) / 300;
                    break;
            }
            X = e.X;
            Y = e.Y;
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    panning = false;
                    break;
            }
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            
        }
    }
}
