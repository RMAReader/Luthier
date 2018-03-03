using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using opennurbs_CLI;

namespace Luthier.Geometry.BSpline
{
    public class NurbsSurface
    {
        private opennurbs_CLI.NurbsSurface surface;

        public NurbsSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            surface = new opennurbs_CLI.NurbsSurface(dimension, Convert.ToInt32(bIsRational), order0, order1, cv_count0, cv_count1);
        }

        public int Dimension { get{ return surface.Dimension; }}
        public int Degree(int direction) { return surface.Degree(direction); }
        public int Order(int direction) { return surface.Order(direction); }
        public int CVCount{ get { return surface.CVCount; } }
		public int KnotCount(int direction) { return surface.KnotCount(direction); }
        public Interval Domain(int direction) { return new Interval(surface.Domain(direction).Min, surface.Domain(direction).Max); }

        public double[] Evaluate(double u, double v, int nderiv)
        {
            return surface.Evaluate(u, v, nderiv);
        }

        public bool SetCV(int IU, int IV, int style, double[] point)
        {
            return Convert.ToBoolean(surface.SetCV(IU, IV, 3, point));
        }
        public double[] GetCV(int IU, int IV, int style)
        {
           return surface.GetCV(IU,IV,style);
        }
        public bool SetKnot(int direction, int IX, double knot_value) { return Convert.ToBoolean(surface.SetKnot(direction, IX, knot_value)); }
        public double GetKnot(int direction, int IX) { return surface.GetKnot(direction, IX); }

        public bool InsertKnot(int direction, double knot_value, int knot_multiplicity) { return Convert.ToBoolean(surface.InsertKnot(direction, knot_value, knot_multiplicity)); }
        public bool IncreaseDegree(int direction, int desired_degree) { return Convert.ToBoolean(surface.IncreaseDegree(direction, desired_degree)); }



    }
}
