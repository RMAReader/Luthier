using Luthier.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry.Nurbs;
using SharpHelper;
using System.Xml.Serialization;
using Luthier.Model.Properties;
using Luthier.Model.GraphicObjects.Interfaces;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicNurbsCurve : 
        GraphicObjectBase, 
        IDrawableLines,
        IScalable,
        IHasDraggable
    {
        public NurbsCurve Curve { get; set; }
        public bool DrawControlNet { get; set; }

        public GraphicNurbsCurve() { DrawControlNet = false; }
        public GraphicNurbsCurve(int dimension, bool isRational, int order, int cvCount)
        {
            Curve = new NurbsCurve(dimension, isRational, order, cvCount);
            DrawControlNet = false;
        }
        public GraphicNurbsCurve(NurbsCurve curve)
        {
            this.Curve = curve.DeepCopy();
            DrawControlNet = false;
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }

        #region "IDrawableLines implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            var curveColour = new SharpDX.Vector4(1, 0, 1, 1);
            var controlPolygonColour = new SharpDX.Vector4(0, 1, 0, 1);

            GetVertexAndIndexListsForCurve(ref vertices, ref indices, curveColour, Curve.Domain.Min, Curve.Domain.Max);

            if (DrawControlNet) GetVertexAndIndexListsForControlPolygon(ref vertices, ref indices, controlPolygonColour);
        }

        public void GetVertexAndIndexListsForCurve(ref List<StaticColouredVertex> vertices, ref List<int> indices, SharpDX.Vector4 color, double from, double to)
        {
            int numberOfLineSegments = 1000;
            if (Curve.NumberOfPoints >= Curve._order)
            {
                var points = Curve.ToLines(numberOfLineSegments, from, to);
                int startIndex = vertices.Count;
                foreach (var pos in points)
                {
                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                        Color = color
                    });
                }
                for (int i = startIndex; i < startIndex + points.Count - 1; i++)
                {
                    indices.AddRange(new int[] { i, i + 1 });
                }
            }
        }

        private void GetVertexAndIndexListsForControlPolygon(ref List<StaticColouredVertex> vertices, ref List<int> indices, SharpDX.Vector4 color)
        {
            int startIndex = vertices.Count();
            for (int i = 0; i < Curve.NumberOfPoints; i++)
            {
                var pos = Curve.GetCV(i);
                vertices.Add(new StaticColouredVertex
                {
                    Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                    Color = color
                });
            }
            for (int i = startIndex; i < startIndex + Curve.NumberOfPoints - 1; i++)
            {
                indices.AddRange(new int[] { i, i + 1 });
            }
        }


        #endregion

        public void ScaleObject(double scaleFactor)
        {
            Curve = Curve.Scale(scaleFactor);
        }

        public IEnumerable<IDraggable> GetDraggableObjects()
        {
            if (DrawControlNet)
            {
                for (int i = 0; i < Curve.ControlPoints.CvCount[0]; i++)
                {
                    yield return new DraggableCurveCV(this, i);
                }
            }
        }

        public IEnumerable<SelectableCurveKnot> GetSelectableKnots()
        {
            int minI = Knot.MinIndex(Curve.knot, Curve._order);
            int maxI = Knot.MaxIndex(Curve.knot, Curve._order);
            
            for (int i = minI; i <= maxI; i++)
            {
                yield return new SelectableCurveKnot(this, i);
            }
        }
    }

    public class DraggableCurveCV : IDraggable
    {
        private NurbsCurve _curve;
        private int _cvIndex;
 
        public DraggableCurveCV(GraphicNurbsCurve curve, int i)
        {
            _curve = curve.Curve;
            _cvIndex = i;
        }

        public double[] Values
        {
            get => _curve.GetCV(_cvIndex);
            set => _curve.SetCV(_cvIndex, value);
        }
    }
}
