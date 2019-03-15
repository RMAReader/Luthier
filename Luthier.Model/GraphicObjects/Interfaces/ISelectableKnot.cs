using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects.Interfaces
{
    public interface ISelectableKnot
    {
        GraphicObjectBase Object { get;  }
        int[] KnotIndices { get; }
        double[] Parameters { get; }
        double[] Coords { get; }

    }

    public class SelectableCurveKnot : ISelectableKnot
    {
        private GraphicNurbsCurve _curve;
        public GraphicNurbsCurve Curve => _curve;
        public GraphicObjectBase Object => _curve;
        public int[] KnotIndices { get; private set; }
        public double[] Parameters => new double[] { _curve.Curve.knot[KnotIndices[0]]};
        public double[] Coords => _curve.Curve.Evaluate(Parameters[0]);

        public SelectableCurveKnot(GraphicNurbsCurve curve, int i)
        {
            _curve = curve;
            KnotIndices = new int[] { i };
        }
    }

    public class SelectableSurfaceKnot : ISelectableKnot
    {
        private GraphicNurbsSurface _surface;
        public GraphicNurbsSurface Surface => _surface;
        public GraphicObjectBase Object => _surface;
        public int[] KnotIndices { get; private set; }
        public double[] Parameters => new double[] { _surface.Surface.knotArray0[KnotIndices[0]], _surface.Surface.knotArray1[KnotIndices[1]] };
        public double[] Coords => _surface.Surface.Evaluate(Parameters[0], Parameters[1]);

        public SelectableSurfaceKnot(GraphicNurbsSurface surface, int i, int j)
        {
            _surface = surface;
            KnotIndices = new int[] { i, j };
        }
    }
}
