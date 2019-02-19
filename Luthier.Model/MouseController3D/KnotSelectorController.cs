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
    public class KnotSelectorController : IMouseController3D
    {
        protected IApplicationDocumentModel _model;
        protected Camera _camera;
        protected double selectionRadius = 20;
        protected List<SelectableKnot> selectedPoints = new List<SelectableKnot>();

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
                SelectableKnot point = null;
                foreach (GraphicNurbsSurface surface in _model.Model.Where(x => x is GraphicNurbsSurface))
                {
                    foreach(SelectableKnot k in surface.GetEdgeKnots())
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
                if(point != null)
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
            if (selectedPoints[0].Surface == selectedPoints[2].Surface) return false;
            if (!PointsAreOnSurfaceEdge(selectedPoints[0], selectedPoints[1])) return false;
            if (!PointsAreOnSurfaceEdge(selectedPoints[2], selectedPoints[3])) return false;

            return true;
        }

        private bool PointsAreOnSurfaceEdge(SelectableKnot p1, SelectableKnot p2)
        {
            //selected points on different surfaces
            if (p1.Surface != p2.Surface) return false;

            //selected points not on same edge of surface
            if (p1.KnotIndices[0] != p2.KnotIndices[0] && p1.KnotIndices[1] != p2.KnotIndices[1]) return false;

            return true;
        }

        
        private NurbsSurfaceEdge CreateEdge(SelectableKnot p1, SelectableKnot p2)
        {
            int minI = Knot.MinIndex(p1.Surface.Surface.knotArray0, p1.Surface.Surface.Order0);
            int maxI = Knot.MaxIndex(p1.Surface.Surface.knotArray0, p1.Surface.Surface.Order0);
            int minJ = Knot.MinIndex(p1.Surface.Surface.knotArray1, p1.Surface.Surface.Order1);
            int maxJ = Knot.MaxIndex(p1.Surface.Surface.knotArray1, p1.Surface.Surface.Order1);

            if (p1.KnotIndices[0] == p2.KnotIndices[0])
            {
                if (p1.KnotIndices[0] == minI) return new NurbsSurfaceEdge(p1.Surface.Surface, EnumSurfaceEdge.West);
                if (p1.KnotIndices[0] == maxI) return new NurbsSurfaceEdge(p1.Surface.Surface, EnumSurfaceEdge.East);
            }
            if (p1.KnotIndices[1] == p2.KnotIndices[1])
            {
                if (p1.KnotIndices[1] == minJ) return new NurbsSurfaceEdge(p1.Surface.Surface, EnumSurfaceEdge.South);
                if (p1.KnotIndices[1] == maxJ) return new NurbsSurfaceEdge(p1.Surface.Surface, EnumSurfaceEdge.North);
            }

            return null;
        }
    }
}
