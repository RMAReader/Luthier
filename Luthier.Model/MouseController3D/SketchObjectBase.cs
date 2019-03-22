using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Geometry;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class SketchObjectBase : IMouseController3D
    {
        protected Camera _camera;
        protected IApplicationDocumentModel _model;
        protected ISketchCanvas _canvas = GraphicPlane.CreateRightHandedXY(new double[] { 0, 0, 0 });

        public ISketchCanvas Canvas { get => _canvas; set => _canvas = value; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Point3D WorldIntersection { get; private set; }

        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
        }

        public void Bind(Camera camera)
        {
            _camera = camera;
        }

        public virtual void Close()
        {

        }

        public virtual void MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        public virtual void MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public virtual void MouseDown(object sender, MouseEventArgs e)
        {

        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;

            WorldIntersection = new Point3D(CalculateIntersection(e.X, e.Y));

            OnMouseMove(sender, e);
        }

        public virtual void MouseUp(object sender, MouseEventArgs e)
        {
        }

        public virtual void MouseWheel(object sender, MouseEventArgs e)
        {
        }


      
        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
        }


        protected double[] CalculateIntersection(int screenX, int screenY)
        {
            _camera.ConvertFromScreenToWorld(screenX, screenY, out double[] from, out double[] to);

            return _canvas.GetPointOfIntersectionWorld(from, to);
        }

       
    }
}
