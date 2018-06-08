
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.BSpline
{

    public class NurbsCurve
    {


        private List<Point2D> points;
        private Knot knot;

        public NurbsCurve(List<Point2D> points, Knot knot)
        {
            this.points = points;
            this.knot = knot;
        }


        public Point2D GetPoint(int IX)
        {
            return points[IX];
        }
        public double GetKnot(int IX)
        {
            return knot.data[IX];
        }
        public int GetDegree() => knot.p;
        public int NumberOfPoints { get => points.Count; }


        public Point2D Evaluate(double t)
        {
            if(IsValid())
            {
                return Algorithm.evaluate_curve(knot.p, knot.data, points.ToArray(), t);
            }
            return null;
        }

        public bool IsValid()
        {
            if (knot.p < 1) return false;
            if (points.Count + knot.p + 1 < knot.size) return false;
            if (points.Count < knot.p + 1) return false;
            return true;
        }


        public List<Point2D> ToLines(int numberOfLines) => ToLines(numberOfLines, double.MinValue, double.MaxValue);
        public List<Point2D> ToLines(int numberOfLines, double from, double to)
        {
            from = Math.Max(from, knot.minParam);
            to = Math.Min(to, knot.maxParam);
            var result = new List<Point2D>();

            for(int i = 0; i <= numberOfLines; i++)
            {
                var t = ((double)i / numberOfLines) * to + (1 - (double)i / numberOfLines) * from;
                var point = Evaluate(t);
                if(point != null) result.Add(point);
            }

            return result;
        }

        public void InsertKnots(List<double> knots)
        {
            var newKnot = new Knot { p = knot.p, data = new List<double>(knot.data)};
            newKnot.data.AddRange(knots);
            newKnot.data.Sort();
            points = BSpline.Algorithm.olso_insertion(points, knot.p, knot.data, newKnot.data);
            knot = newKnot;
        }

        public void CloseFront()
        {
            var newknot = new List<double>();
            var minParam = knot.minParam;
            for (int i=0; i < knot.Continuity(minParam); i++) newknot.Add(minParam);
            InsertKnots(newknot);
            points = points.GetRange(newknot.Count, points.Count - newknot.Count);
            knot.data = knot.data.GetRange(newknot.Count, knot.data.Count - newknot.Count);
        }

        public void CloseBack()
        {
            var newknots = new List<double>();
            var maxParam = knot.maxParam;
            for (int i = 0; i < knot.Continuity(maxParam); i++) newknots.Add(maxParam);
            InsertKnots(newknots);
            points = points.GetRange(0, points.Count - newknots.Count);
            knot.data = knot.data.GetRange(0, knot.data.Count - newknots.Count);
        }


        public NurbsCurve DeepCopy()
        {
            return new NurbsCurve(new List<Point2D>(points), knot.DeepCopy());
        }

    }



    public class NurbsCurve2
    {

        private int dimension;
        private int order;
        private bool isRational;
        private int cvSize;

        public double[] knot;
        public double[] cvDataBlock; // 
        private int cvCount;
      

        public NurbsCurve2(List<Point2D> points, Knot knot)
        {
            //this.points = points;
            //this.knot = knot;
        }


        public double[] GetPoint(int IX)
        {
            double[] cv = new double[cvSize];
            for (int i = 0; i < cvSize; i++) cv[i] = cvDataBlock[IX + i * cvCount];
            return cv;
        }
        public double GetKnot(int IX)
        {
            return knot[IX];
        }
        public int GetDegree() => order - 1;
        public int NumberOfPoints { get => cvCount; }

        public Interval Domain
        {
            get => new Interval(knot[order - 2], knot[knot.Length - order + 1]);
        }

        public double[] Evaluate(double t)
        {
            double[] p = new double[cvSize];
            int knotIX = Algorithm.Find_Knot_Span(order - 1, knot, t);
            int cvIX = knotIX - order + 2;
            for(int i = 0; i < cvSize; i++)
            {
                p[i] = Algorithm.Evaluate_CurveSpan_Deboor(order - 1, knotIX, ref knot, ref cvDataBlock, cvIX, cvCount, t);
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


        public NurbsCurve2 DeepCopy()
        {
            return this;          
            //return new NurbsCurve(new List<Point2D>(points), knot.DeepCopy());
        }

    }
}
