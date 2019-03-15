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
        IScalable
    {
        public NurbsCurve Curve { get; set; }
       
        public GraphicNurbsCurve() { }
        public GraphicNurbsCurve(int dimension, bool isRational, int order, int cvCount)
        {
            Curve = new NurbsCurve(dimension, isRational, order, cvCount);
        }
        public GraphicNurbsCurve(NurbsCurve curve)
        {
            this.Curve = curve.DeepCopy();
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }

        #region "IDrawableLines implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            GetVertexAndIndexListsForCurve(ref vertices, ref indices);

            GetVertexAndIndexListsForControlPolygon(ref vertices, ref indices);
        }

        private void GetVertexAndIndexListsForCurve(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            
            int numberOfLineSegments = 1000;
            if (Curve.NumberOfPoints > 2)
            {
                int startIndex = vertices.Count();
                foreach (var pos in Curve.ToLines(numberOfLineSegments))
                {
                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                        Color = new SharpDX.Vector4(1, 0, 1, 1)
                    });
                }
                for (int i = startIndex; i < startIndex + numberOfLineSegments; i++)
                {
                    indices.AddRange(new int[] { i, i + 1 });
                }
            }
        }

        private void GetVertexAndIndexListsForControlPolygon(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            int startIndex = vertices.Count();
            for (int i = 0; i < Curve.NumberOfPoints; i++)
            {
                var pos = Curve.GetCV(i);
                vertices.Add(new StaticColouredVertex
                {
                    Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                    Color = new SharpDX.Vector4(0, 1, 0, 1)
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
            for(int i = 0; i < Curve.ControlPoints.CvCount[0]; i++)
            {
                yield return new DraggableCurveCV(this, i);
            }
        }

        public IEnumerable<SelectableCurveKnot> GetSelectableKnots()
        {
            if (!DrawKnotSpans) yield break;

            int minI = Knot.MinIndex(Surface.knotArray0, Surface.Order0);
            int maxI = Knot.MaxIndex(Surface.knotArray0, Surface.Order0);
            int minJ = Knot.MinIndex(Surface.knotArray1, Surface.Order1);
            int maxJ = Knot.MaxIndex(Surface.knotArray1, Surface.Order1);

            for (int i = minI; i <= maxI; i++)
            {
                yield return new SelectableSurfaceKnot(this, i, minJ);
                yield return new SelectableSurfaceKnot(this, i, maxJ);
            }

            for (int j = minJ + 1; j < maxJ; j++)
            {
                yield return new SelectableSurfaceKnot(this, minI, j);
                yield return new SelectableSurfaceKnot(this, maxI, j);
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
