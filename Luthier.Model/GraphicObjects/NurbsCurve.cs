using Luthier.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Geometry.BSpline;
using SharpHelper;

namespace Luthier.Model.GraphicObjects
{
    public class NurbsCurve : GraphicObjectBase, IDrawableLines
    {

        private int _dimension;
        private int _order;
        private bool _isRational;
        private int _cvSize;

        public double[] knot;
        public double[] cvDataBlock; // 
        private int _cvCount;


        public NurbsCurve(int dimension, bool isRational, int order, int cvCount)
        {
            _dimension = dimension;
            _isRational = isRational;
            _order = order;
            _cvSize = (isRational) ? dimension + 1 : dimension;
            _cvCount = cvCount;

            knot = new double[_cvCount + _order - 2];
            for (int i = 0; i < knot.Length; i++) knot[i] = i;

            cvDataBlock = new double[_cvSize * _cvCount];
        }


        public double[] GetCV(int IX)
        {
            double[] cv = new double[_cvSize];
            for (int i = 0; i < _cvSize; i++) cv[i] = cvDataBlock[IX + i * _cvCount];
            return cv;
        }

        public void SetCV(int IX, double[] cv)
        {
            for (int i = 0; i < _cvSize; i++) cvDataBlock[IX + i * _cvCount] = cv[i];
        }
        public double GetKnot(int IX)
        {
            return knot[IX];
        }
        public int GetDegree() => _order - 1;
        public int NumberOfPoints { get => _cvCount; }

        public Interval Domain
        {
            get => new Interval(knot[_order - 2], knot[knot.Length - _order + 1]);
        }

        public double[] Evaluate(double t)
        {
            double[] p = new double[_cvSize];
            int knotIX = Geometry.BSpline.Algorithm.Find_Knot_Span(_order - 1, knot, t);
            int cvIX = knotIX - _order + 2;
            for (int i = 0; i < _cvSize; i++)
            {
                p[i] = Geometry.BSpline.Algorithm.Evaluate_CurveSpan_Deboor(_order - 1, knotIX, ref knot, ref cvDataBlock, i * _cvCount + cvIX, cvStride: 1, t: t);
            }
            return p;
        }


        public List<double[]> ToLines(int numberOfLines) => ToLines(numberOfLines, double.MinValue, double.MaxValue);
        public List<double[]> ToLines(int numberOfLines, double from, double to)
        {
            from = Math.Max(from, Domain.Min);
            to = Math.Min(to, Domain.Max);
            var result = new List<double[]>();

            for (int i = 0; i <= numberOfLines; i++)
            {
                var t = ((double)i / numberOfLines) * to + (1 - (double)i / numberOfLines) * from;
                var point = Evaluate(t);
                if (point != null) result.Add(point);
            }
            return result;
        }

        public void InsertKnots(List<double> knots)
        {
            //var newKnot = new Knot { p = knot.p, data = new List<double>(knot.data) };
            //newKnot.data.AddRange(knots);
            //newKnot.data.Sort();
            //points = BSpline.Algorithm.olso_insertion(points, knot.p, knot.data, newKnot.data);
            //knot = newKnot;
        }

        public void CloseFront()
        {
            //var newknot = new List<double>();
            //var minParam = knot.minParam;
            //for (int i = 0; i < knot.Continuity(minParam); i++) newknot.Add(minParam);
            //InsertKnots(newknot);
            //points = points.GetRange(newknot.Count, points.Count - newknot.Count);
            //knot.data = knot.data.GetRange(newknot.Count, knot.data.Count - newknot.Count);
        }

        public void CloseBack()
        {
            //var newknots = new List<double>();
            //var maxParam = knot.maxParam;
            //for (int i = 0; i < knot.Continuity(maxParam); i++) newknots.Add(maxParam);
            //InsertKnots(newknots);
            //points = points.GetRange(0, points.Count - newknots.Count);
            //knot.data = knot.data.GetRange(0, knot.data.Count - newknots.Count);
        }


        public NurbsCurve DeepCopy()
        {
            return this;
            //return new NurbsCurve(new List<Point2D>(points), knot.DeepCopy());
        }



        public void ExtendFront(double[] cv)
        {
            if (cv.Length != _cvSize) return;

            var newKnot = new double[knot.Length + 1];
            Array.Copy(knot, 0, newKnot, 1, knot.Length);
            newKnot[0] = 2 * newKnot[1] - newKnot[2];

            var newCvDataBlock = new double[cvDataBlock.Length + _cvSize];
            Array.Copy(cvDataBlock, 0, newCvDataBlock, _cvSize, cvDataBlock.Length);
            Array.Copy(cv, 0, newCvDataBlock, 0, _cvSize);

            _cvCount++;

            cvDataBlock = newCvDataBlock;
            knot = newKnot;
        }

        public void ExtendBack(double[] cv)
        {
            if (cv.Length != _cvSize) return;

            var newKnot = new double[knot.Length + 1];
            Array.Copy(knot, 0, newKnot, 0, knot.Length);
            newKnot[newKnot.Length-1] = 2 * newKnot[newKnot.Length - 2] - newKnot[newKnot.Length - 3];

            var newCvDataBlock = new double[cvDataBlock.Length + _cvSize];
            for(int i = 0; i < _cvCount; i++)
            {
                for(int j=0; j < _cvSize; j++)
                {
                    newCvDataBlock[i + j * (_cvCount + 1)] = cvDataBlock[i + j * _cvCount];
                }
            }
            for (int j = 0; j < _cvSize; j++)
            {
                newCvDataBlock[_cvCount + j * (_cvCount + 1)] = cv[j];
            }
           
            _cvCount++;

            cvDataBlock = newCvDataBlock;
            knot = newKnot;
        }




        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }

        #region "IDrawableLines implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {

            var startIndex = vertices.Count();
            for (int i = 0; i < NumberOfPoints; i++)
            {
                var pos = GetCV(i);
                vertices.Add(new StaticColouredVertex
                {
                    Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                    Color = new SharpDX.Vector4(0, 1, 0, 1)
                });
            }
            for (int i = startIndex; i < startIndex + NumberOfPoints - 1; i++)
            {
                indices.AddRange(new int[] { i, i + 1 });
            }


            int numberOfLineSegments = 1000;
            if (NumberOfPoints > 2)
            {
                startIndex = vertices.Count();
                foreach (var pos in ToLines(numberOfLineSegments))
                {
                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                        Color = new SharpDX.Vector4(0, 1, 0, 1)
                    });
                }
                for (int i = startIndex; i < startIndex + numberOfLineSegments; i++)
                {
                    indices.AddRange(new int[] { i, i + 1 });
                }
            }
        }

        #endregion
    }

    public class DraggableCurveCV : IDraggable
    {
        private NurbsCurve _curve;
        private int _cvIndex;
 
        public DraggableCurveCV(NurbsCurve curve, int i)
        {
            _curve = curve;
            _cvIndex = i;
        }

        public double[] Values
        {
            get => _curve.GetCV(_cvIndex);
            set => _curve.SetCV(_cvIndex, value);
        }
    }
}
