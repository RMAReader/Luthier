using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Luthier.Geometry;
using Luthier.Geometry.Nurbs;
using Luthier.Model.GraphicObjects;
using Luthier.Model.GraphicObjects.Interfaces;
using Luthier.Model.Presenter;

namespace Luthier.Model.MouseController3D
{
    public class KnotSelectorController : IMouseController3D
    {
        protected IApplicationDocumentModel _model;
        protected Camera _camera;
        protected double selectionRadius = 20;
        protected List<ISelectableKnot> selectedPoints = new List<ISelectableKnot>();

        public int X {get; private set;}

        public int Y { get; private set; }

        public void Bind(IApplicationDocumentModel model)
        {
            _model = model;
            foreach (GraphicNurbsSurface surface in _model.Model.Where(x => x is GraphicNurbsSurface))
            {
                surface.DrawKnotSpans= true;
            }
            _model.Model.HasChanged = true;
            selectedPoints.Clear();
        }

        public void Bind(Camera wvp)
        {
            _camera = wvp;
        }

        public void Close()
        {
            foreach (GraphicNurbsSurface surface in _model.Model.Where(x => x is GraphicNurbsSurface))
            {
                surface.DrawKnotSpans = false;
            }
            _model.Model.HasChanged = false;
            selectedPoints.Clear();
        }

        public void MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                double distance = double.MaxValue;
                ISelectableKnot point = null;
                foreach (GraphicNurbsSurface surface in _model.Model.Where(x => x is GraphicNurbsSurface))
                {
                    foreach(SelectableSurfaceKnot k in surface.GetEdgeKnots())
                    {
                        var p = _camera.ConvertFromWorldToScreen(k.Coords);
                        double d = Math.Sqrt((p[0] - e.X) * (p[0] - e.X) + (p[1] - e.Y) * (p[1] - e.Y));

                        if (d < selectionRadius && d < distance && !selectedPoints.Contains(k))
                        {
                            point = k;
                            distance = d;
                        }
                    }

                }
                foreach (GraphicNurbsCurve curve in _model.Model.Where(x => x is GraphicNurbsCurve))
                {
                    foreach (SelectableCurveKnot k in curve.GetSelectableKnots())
                    {
                        var p = _camera.ConvertFromWorldToScreen(k.Coords);
                        double d = Math.Sqrt((p[0] - e.X) * (p[0] - e.X) + (p[1] - e.Y) * (p[1] - e.Y));

                        if (d < selectionRadius && d < distance && !selectedPoints.Contains(k))
                        {
                            point = k;
                            distance = d;
                        }
                    }

                }
                if (point != null)
                {
                    selectedPoints.Add(point);
                }

                if(selectedPoints.Count == 4)
                {
                    CreateJoiningSurface();
                    selectedPoints.Clear();
                }
            }
        }

        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            
        }


        private void CreateJoiningSurface()
        {
            if(SelectionIsValid())
            {
                var edge1 = CreateEdge(selectedPoints[0], selectedPoints[1]);
                var edge2 = CreateEdge(selectedPoints[2], selectedPoints[3]);

                var bridgingSurface = NurbsSurfaceJoiner.CreateBridgingSurface(edge1, edge2);

                var graphicBridgingSurface = new GraphicNurbsSurface(bridgingSurface);

                _model.Model.Add(graphicBridgingSurface);
            }
            else
            {
                MessageBox.Show("Selected knots must define single edges of 2 different surfaces.");
            }
        }

        private bool SelectionIsValid()
        {
            if (selectedPoints[0].Object == selectedPoints[2].Object) return false;
            if (!PointsAreOnSurfaceEdge(selectedPoints[0], selectedPoints[1])) return false;
            if (!PointsAreOnSurfaceEdge(selectedPoints[2], selectedPoints[3])) return false;

            return true;
        }

        private bool PointsAreOnSurfaceEdge(ISelectableKnot p1, ISelectableKnot p2)
        {
            //selected points on different surfaces
            if (p1.Object != p2.Object) return false;

            //selected points not on same edge of surface
            if (p1.Object is GraphicNurbsCurve && p1.KnotIndices[0] == p2.KnotIndices[0]) return false;
            if (p1.Object is GraphicNurbsSurface && p1.KnotIndices[0] != p2.KnotIndices[0] && p1.KnotIndices[1] != p2.KnotIndices[1]) return false;

            return true;
        }

        
        private INurbsSurfaceEdge CreateEdge(ISelectableKnot p1, ISelectableKnot p2)
        {
            if (p1.Object is GraphicNurbsSurface)
                return CreateEdgeFromSurface((SelectableSurfaceKnot)p1, (SelectableSurfaceKnot)p2);

            if (p1.Object is GraphicNurbsCurve)
                return CreateEdgeFromCurve((SelectableCurveKnot)p1, (SelectableCurveKnot)p2);

            return null;
        }

        private NurbsCurveEdge CreateEdgeFromCurve(SelectableCurveKnot p1, SelectableCurveKnot p2)
        {
            return new NurbsCurveEdge(p1.Curve.Curve);
        }

        private NurbsSurfaceEdge CreateEdgeFromSurface(SelectableSurfaceKnot p1, SelectableSurfaceKnot p2)
        {
            int minI = Knot.MinIndex(p1.Surface.Surface.knotArray0, p1.Surface.Surface.Order0);
            int maxI = Knot.MaxIndex(p1.Surface.Surface.knotArray0, p1.Surface.Surface.Order0);
            int minJ = Knot.MinIndex(p1.Surface.Surface.knotArray1, p1.Surface.Surface.Order1);
            int maxJ = Knot.MaxIndex(p1.Surface.Surface.knotArray1, p1.Surface.Surface.Order1);

            if (p1.KnotIndices[0] == p2.KnotIndices[0])
            {
                if (p1.KnotIndices[0] == minI)
                {
                    int minJIx = Math.Min(p1.KnotIndices[1], p2.KnotIndices[1]);
                    int maxJIx = Math.Max(p1.KnotIndices[1], p2.KnotIndices[1]);

                    var edgeSurface = p1.Surface.Surface.SubSurface(minI,minI + 1, minJIx, maxJIx);

                    return new NurbsSurfaceEdge(edgeSurface, EnumSurfaceEdge.West);
                }
                if (p1.KnotIndices[0] == maxI)
                {
                    int minJIx = Math.Min(p1.KnotIndices[1], p2.KnotIndices[1]);
                    int maxJIx = Math.Max(p1.KnotIndices[1], p2.KnotIndices[1]);

                    var edgeSurface = p1.Surface.Surface.SubSurface(maxI - 1, maxI, minJIx, maxJIx);

                    return new NurbsSurfaceEdge(edgeSurface, EnumSurfaceEdge.East);
                }
            }
            if (p1.KnotIndices[1] == p2.KnotIndices[1])
            {
                if (p1.KnotIndices[1] == minJ)
                {
                    double minU = Math.Min(p1.Parameters[0], p2.Parameters[0]);
                    double maxU = Math.Max(p1.Parameters[0], p2.Parameters[0]);
                    Interval domain = p1.Surface.Surface.Domain1();

                    var edgeSurface = p1.Surface.Surface.SubSurface(minU, maxU, domain.Min, domain.Max);

                    return new NurbsSurfaceEdge(edgeSurface, EnumSurfaceEdge.South);
                }
                if (p1.KnotIndices[1] == maxJ)
                {
                    double minU = Math.Min(p1.Parameters[0], p2.Parameters[0]);
                    double maxU = Math.Max(p1.Parameters[0], p2.Parameters[0]);
                    Interval domain = p1.Surface.Surface.Domain1();

                    var edgeSurface = p1.Surface.Surface.SubSurface(minU, maxU, domain.Min, domain.Max);

                    return new NurbsSurfaceEdge(edgeSurface, EnumSurfaceEdge.North);
                }
            }

            return null;
        }
    }
}
