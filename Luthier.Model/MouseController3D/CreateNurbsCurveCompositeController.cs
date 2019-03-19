using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Geometry.Nurbs;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class CreateNurbsCurveCompositeController : IMouseController3D
    {
        protected IApplicationDocumentModel _model;
        protected Camera _camera;
        protected double selectionRadius = 20;
        public GraphicPlane ReferencePlane;

        protected GraphicNurbsCurveComposite composite;

        public int X { get; protected set; }

        public int Y { get; protected set; }

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
            switch (e.Button)
            {
                case MouseButtons.Left:

                    List<(GraphicNurbsCurve, double)> curvesAndDistances = new List<(GraphicNurbsCurve, double)>();

                    foreach (GraphicNurbsCurve curve in _model.Model.VisibleObjects().Where(x => x is GraphicNurbsCurve))
                    {
                        double curveDistance = double.MaxValue;
                        foreach (var point in curve.Curve.ToLines(100))
                        {
                            var p = _camera.ConvertFromWorldToScreen(point);
                            double d = Math.Sqrt((p[0] - e.X) * (p[0] - e.X) + (p[1] - e.Y) * (p[1] - e.Y));
                            if (d < selectionRadius && d < curveDistance)
                            {
                                curveDistance = d;
                            }
                        }
                        if (curveDistance < selectionRadius)
                            curvesAndDistances.Add((curve, curveDistance));
                    }

                    if (curvesAndDistances.Count > 1)
                    {
                        if(composite == null)
                        {
                            composite = new GraphicNurbsCurveComposite
                            {
                                ReferencePlaneKey = ReferencePlane.Key,
                                Components = new List<NurbsCurveCompositeJoin>(),
                            };
                            _model.Model.Add(composite);
                        }

                        curvesAndDistances = curvesAndDistances.OrderBy(x => x.Item2).Take(2).ToList();

                        _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

                        var intersectPoint = ReferencePlane.GetPointOfIntersectionWorld(from, to);

                        NurbsCurveCompositeJoin join = new NurbsCurveCompositeJoin
                        {
                            Centre = new Geometry.Point3D(intersectPoint),
                            Radius = 20,
                            Curve1Key = curvesAndDistances[0].Item1.Key,
                            Curve2Key = curvesAndDistances[1].Item1.Key
                        };
                        composite.Components.Add(join);
                    }

                    break;

                case MouseButtons.Right:
                    composite = null;
                   
                    break;
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
          
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            
        }
    }
}
