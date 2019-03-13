using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Nurbs
{

    public enum EnumSurfaceEdge
    {
        North, South, East, West
    }

    public class NurbsSurfaceJoiner
    {


        public static NurbsSurface CreateBridgingSurface(INurbsSurfaceEdge edge1, INurbsSurfaceEdge edge2)
        {
            TryGetCompatibleEdges(edge1, edge2, out INurbsSurfaceEdge top, out INurbsSurfaceEdge bottom);
           
            int cvCountbridge = 3;

            var result = new NurbsSurface(top.Dimension, 
                top.IsRational, 
                top.Order, 
                top.Order, 
                top.CvCount, 
                cvCountbridge);

            Array.Copy(top.Knot, result.knotArray0, top.Knot.Length);
            result.knotArray1 = Knot.CreateUniformClosed(top.Order - 1, cvCountbridge + top.Order).data.ToArray();

            for(int i = 0; i < result.CvCount0; i++)
            {
                var topCv = top.GetCV(i);
                var bottomCv = bottom.GetCV(i);
                var midCv = topCv.Add(bottomCv).Multiply(0.5);

                result.SetCV(i, 0, bottomCv);
                result.SetCV(i,1, midCv);
                result.SetCV(i,2, topCv);
            }

            var t = 0.5 * (top.Domain.Min + top.Domain.Max);
            var topNormal = top.EvaluateNormal(t);
            var resultNormal = result.EvaluateNormal(result.Domain1().Max, t);

            if(topNormal.DotProduct(resultNormal) < 0)
            {
                //flip parity of result
                result = result.SwapParity();
            }

            return result;
        }



        private void VerifyKnotIsNormalised(double[] knot)
        {
            if (knot[0] != 0 || knot[knot.Length - 1] != 1)
                throw new ArgumentException();
        }

        private static bool TryGetCompatibleEdges(INurbsSurfaceEdge e1, INurbsSurfaceEdge e2, out INurbsSurfaceEdge c1, out INurbsSurfaceEdge c2)
        {
            var newKnots1 = new List<double>();
            var newKnots2 = new List<double>();

            int i = 0;
            int j = 0;
            while (i < e1.Knot.Length || j < e2.Knot.Length)
            {
                if (e1.Knot[i] == e2.Knot[j])
                {
                    i++;
                    j++;
                }
                else if (e1.Knot[i] < e2.Knot[j])
                {
                    newKnots2.Add(e1.Knot[i]);
                    i++;
                }
                else if (e1.Knot[i] > e2.Knot[j])
                {
                    newKnots1.Add(e2.Knot[j]);
                    j++;
                }
            }

            c1 = e1.DeepCopy();
            c1.InsertKnots(newKnots1.ToArray());

            c2 = e2.DeepCopy();
            c2.InsertKnots(newKnots2.ToArray());

            return true;
        }


    }




    public interface INurbsSurfaceEdge
    {
        bool IsRational { get; }
        int Dimension { get; }
        double[] Knot { get; }
        int Order { get; }
        int CvCount { get; }
        Interval Domain { get; }
        double[] GetCV(int i);
        double[] EvaluateNormal(double t);
        void InsertKnots(double[] newKnots);
        INurbsSurfaceEdge DeepCopy();
    }

    public class NurbsSurfaceEdge : INurbsSurfaceEdge
    {
        private NurbsSurface _surface;
        private EnumSurfaceEdge _edge;

        private int _direction => (_edge == EnumSurfaceEdge.North || _edge == EnumSurfaceEdge.South) ? 0 : 1;
               
        public bool IsRational => _surface.IsRational;

        public int Dimension => _surface.Dimension;

        public double[] Knot => _direction == 0 ? _surface.knotArray0 : _surface.knotArray1;

        public int Order => _direction == 0 ? _surface.Order0 : _surface.Order1;
        
        public int CvCount => _surface.controlPoints.CvCount[_direction];

        public Interval Domain => _direction == 0 ? _surface.Domain0() : _surface.Domain1();

        public double[] GetCV(int i)
        {
            int[] coords = GetSurfaceCVCoords(i);
            return _surface.GetCV(coords[0], coords[1]);
        }

        public double[] EvaluateNormal(double t)
        {
            double[] uv = GetSurfaceParameterCoords(t);
            return _surface.EvaluateNormal(uv[0], uv[1]);
        }


        private int[] GetSurfaceCVCoords(int i)
        {
            if (_edge == EnumSurfaceEdge.North)
                return new int[] { i, _surface.controlPoints.CvCount[1] - 1 };

            if (_edge == EnumSurfaceEdge.South)
                return new int[] { i, 0 };

            if (_edge == EnumSurfaceEdge.East)
                return new int[] { _surface.controlPoints.CvCount[0] - 1, i };

            if (_edge == EnumSurfaceEdge.West)
                return new int[] { 0, i };

            return null;
        }

        private double[] GetSurfaceParameterCoords(double t)
        {
            if (_edge == EnumSurfaceEdge.North)
                return new double[] { t, _surface.Domain1().Max };

            if (_edge == EnumSurfaceEdge.South)
                return new double[] { t, _surface.Domain1().Min };

            if (_edge == EnumSurfaceEdge.East)
                return new double[] { _surface.Domain1().Max, t };

            if (_edge == EnumSurfaceEdge.West)
                return new double[] { _surface.Domain1().Min, t };

            return null;
        }

        public void InsertKnots(double[] newKnots)
        {
            _surface = _surface.InsertKnot(_direction, newKnots);
        }

        public INurbsSurfaceEdge DeepCopy()
        {
            return new NurbsSurfaceEdge(_surface.DeepCopy(), _edge);
        }

        public NurbsSurfaceEdge(NurbsSurface surface, EnumSurfaceEdge edge)
        {
            _surface = surface;
            _edge = edge;
        }
    }

    public class NurbsCurveEdge : INurbsSurfaceEdge
    {
        private NurbsCurve _curve;

        public bool IsRational => _curve._isRational;

        public int Dimension => _curve._dimension;

        public double[] Knot => _curve.knot;

        public int Order => _curve._order;

        public int CvCount => _curve.ControlPoints.CvCount[0];

        public Interval Domain => _curve.Domain;

        public double[] GetCV(int i) => _curve.GetCV(i);

        public double[] EvaluateNormal(double t) => null;
       
        public void InsertKnots(double[] newKnots)
        {
            _curve = _curve.InsertKnot(newKnots);
        }

        public INurbsSurfaceEdge DeepCopy()
        {
            return new NurbsCurveEdge(_curve.DeepCopy());
        }
        
        public NurbsCurveEdge(NurbsCurve curve)
        {
            _curve = curve;
        }
    }
}
