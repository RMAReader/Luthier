using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using Luthier.Model.ToolPathSpecification;

namespace Luthier.Model.MouseController3D
{
    public class SelectToolPathBoundaryCurvesController : IMouseController3D
    {
        protected Camera _camera;
        protected IApplicationDocumentModel _model;

        public GraphicPlane ReferencePlane;

        public int X { get; private set; }

        public int Y { get; private set; }

        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
        }

        public void Bind(Camera camera)
        {
            _camera = camera;
        }

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
            var mouldOutline = new MouldEdgeSpecification
            {
                ReferencePlaneKey = ReferencePlane.Key,
                BoundaryPolygonKey = new List<UniqueKey>(),
                SafeHeight = 50,
                TopHeight = 20,
                BottomHeight = 0,
                MaximumCutDepth = 5,
                SpindleState = CncOperation.EnumSpindleState.OnClockwise,
                SpindleSpeed = 22000,
                FeedRate = 500,
                Tool = new CncTool.EndMill { Diameter = 6.35 }
            };

            foreach (var obj in _model.Model.VisibleObjects().Where(x => x is IPolygon2D))
            {
                mouldOutline.BoundaryPolygonKey.Add(obj.Key);
            }

            _model.Model.Add(mouldOutline);

            mouldOutline.Calculate();

            _model.Model.HasChanged = true;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
            OnMouseMove(sender, e);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
          
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
           
        }

        public virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
