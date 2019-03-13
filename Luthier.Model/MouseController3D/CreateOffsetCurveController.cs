using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Geometry;
using Luthier.Geometry.Nurbs;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class CreateOffsetCurveController : IMouseController3D
    {
        protected IApplicationDocumentModel _model;
        protected Camera _camera;
        protected int startX;
        protected int startY;
        protected double selectionRadius = 20;

        protected double[] startCv;
        protected double[] startBiNormal;
        protected GraphicNurbsCurve startCurve;
        protected GraphicNurbsCurve offsetCurve;

        public GraphicPlane ReferencePlane { get; set; }

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
            switch (e.Button)
            {
                case MouseButtons.Left:

                    GraphicNurbsCurve nearestCurve = null;
                    int nearestCvIx = -1;
                    double distance = double.MaxValue;
                    foreach (GraphicNurbsCurve curve in _model.Model.Where(x => x is GraphicNurbsCurve))
                    {
                        for (int i = 0; i < curve.Curve.NumberOfPoints; i++)
                        {
                            var cv = curve.Curve.GetCV(i);
                            var p = _camera.ConvertFromWorldToScreen(cv);
                            double d = Math.Sqrt((p[0] - e.X) * (p[0] - e.X) + (p[1] - e.Y) * (p[1] - e.Y));
                            if (d < selectionRadius && d < distance)
                            {
                                nearestCurve = curve;
                                nearestCvIx = i;
                                distance = d;
                            }
                        }
                    }

                    if(nearestCurve != null)
                    {
                        startCurve = nearestCurve;
                        offsetCurve = new GraphicNurbsCurve(nearestCurve.Curve.DeepCopy());
                        startCv = nearestCurve.Curve.GetCV(nearestCvIx);
                        startBiNormal = GetBiNormalThroughCv(nearestCurve.Curve, nearestCvIx);

                        offsetCurve.Curve = NurbsCurveBuilder.GetOffsetCurveInPlane(offsetCurve.Curve, ReferencePlane._normal, 0);
                        _model.Model.Add(offsetCurve);
                        _model.Model.HasChanged = true;                        
                    }

                    break;

                case MouseButtons.Right:
                    nearestCurve = null;
                    offsetCurve = null;
                    startCv = null;
                    break;
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;

            if (offsetCurve != null)
            {
                var dragPlane = ReferencePlane.GetParallelPlaneThroughPoint(startCv);

                _camera.ConvertFromScreenToWorld(X, Y, out double[] from, out double[] to);

                var intersectPoint = dragPlane.GetPointOfIntersectionWorld(from, to);

                //var offsetDistance = startCv.Subtract(intersectPoint).L2Norm();
                var offsetDistance = startBiNormal.DotProduct(startCv.Subtract(intersectPoint));
                
                offsetCurve.Curve = NurbsCurveBuilder.GetOffsetCurveInPlane(startCurve.Curve, ReferencePlane._normal, offsetDistance);
                _model.Model.HasChanged = true;

            }
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
           
        }


        private double[] GetBiNormalThroughCv(NurbsCurve curve, int i)
        {
            double[] result = null;
            
                if (i == 0)
                {
                    result = curve.GetCV(i + 1).Subtract(curve.GetCV(i)).VectorProduct(ReferencePlane._normal).Normalise();

                }
                else if (i == curve.NumberOfPoints - 1)
                {
                    result = curve.GetCV(i).Subtract(curve.GetCV(i - 1)).VectorProduct(ReferencePlane._normal).Normalise();
                }
                else
                {
                    var d0 = curve.GetCV(i + 1).Subtract(curve.GetCV(i)).VectorProduct(ReferencePlane._normal).Normalise();
                    var d1 = curve.GetCV(i).Subtract(curve.GetCV(i - 1)).VectorProduct(ReferencePlane._normal).Normalise();
                    result= d1.Add(d0).Normalise();
                }
            return result;
        }
    }
}
