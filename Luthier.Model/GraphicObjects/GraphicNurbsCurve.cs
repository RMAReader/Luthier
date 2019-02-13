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
