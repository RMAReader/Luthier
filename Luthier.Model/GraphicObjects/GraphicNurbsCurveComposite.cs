using Luthier.Core;
using Luthier.Geometry;
using Luthier.Geometry.Nurbs;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Luthier.Geometry.Intersection;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicNurbsCurveComposite : 
        GraphicObjectBase,
        IPolygon2D,
        IDrawableLines
    {
        public UniqueKey ReferencePlaneKey { get; set; }
        public List<NurbsCurveCompositeJoin> Components { get; set; }



        #region "IDrawableLines implementation"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            var curveColour = new SharpDX.Vector4(1, 0, 1, 1);

            foreach(var section in GetCurveSections())
            {
                var curveSection = new GraphicNurbsCurve(section.Curve);
                curveSection.GetVertexAndIndexListsForCurve(ref vertices, ref indices, curveColour, section.Interval.Min, section.Interval.Max);
            }
        }

        #endregion



        public List<Point3D> ToLines()
        {
            var result = new List<Point3D>();
            foreach (var section in GetCurveSections())
            {
                var sectionLines = section.Curve
                    .ToLines(1000, section.Interval.Min, section.Interval.Max)
                    .Select(x => new Point3D(x));

                result.AddRange(sectionLines);
            }
            return result;
        }

        private IEnumerable<NurbsCurveSection> GetCurveSections()
        {
            var plane = (GraphicPlane)Model[ReferencePlaneKey];

            //project all curves into plane,and calculate intersections using projected curves
            double[][] map = new double[][] { plane._unitU, plane._unitV, plane._normal };

            var intersects = new List<NurbsCurveIntersection>();
            foreach(var component in Components)
            {
                //Todo: must map curves and centre point to in-plane coordinates
                GraphicNurbsCurve c1 = (GraphicNurbsCurve)Model[component.Curve1Key];
                GraphicNurbsCurve c2 = (GraphicNurbsCurve)Model[component.Curve2Key];
                Point2D centre = new Point2D(map.DotProduct(component.Centre.Data));
                var intersection = Intersection.GetIntersection(c1.Curve.Apply((double[] cv) => map.DotProduct(cv)), c2.Curve.Apply((double[] cv) => map.DotProduct(cv)), centre, 0.0001);

                intersects.Add(new NurbsCurveIntersection {
                    curve1 = c1.Curve,
                    curve2 = c2.Curve,
                    Parameter1 = intersection.Parameter1,
                    Parameter2 = intersection.Parameter2,
                    Point = intersection.Point
                });
            }

            return intersects.EnumeratePairsClosed().Select(x => GetCurveSection(x.Item1, x.Item2));
        }


        private NurbsCurveSection GetCurveSection(NurbsCurveIntersection i1, NurbsCurveIntersection i2)
        {
            NurbsCurve curve = null;
            Interval interval = null;
            if (i1.curve1 == i2.curve1)
            {
                curve = i1.curve1;
                interval = new Interval(i1.Parameter1, i2.Parameter1);
            }
            else if (i1.curve1 == i2.curve2)
            {
                curve = i1.curve1;
                interval = new Interval(i1.Parameter1, i2.Parameter2);
            }
            else if (i1.curve2 == i2.curve1)
            {
                curve = i1.curve2;
                interval = new Interval(i1.Parameter2, i2.Parameter1);
            }
            else
            {
                curve = i1.curve2;
                interval = new Interval(i1.Parameter2, i2.Parameter2);
            }
            return new NurbsCurveSection { Curve = curve, Interval = interval };
        }




        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }


        #region "IPolygon2D implementation"

        UniqueKey IPolygon2D.Key()
        {
            return Key;
        }

        public Polygon2D ToPolygon2D(IApplicationDocumentModel model)
        {
            throw new NotSupportedException();
        }

        public Polygon2D ToPolygon2D()
        {
            GraphicPlane referencePlane = Model[ReferencePlaneKey] as GraphicPlane;
            return new Polygon2D(ToLines().Select(p => referencePlane.MapWorldToPlaneCoordinates(p)).Select(p => new Point2D(p.X, p.Y)).ToList());
        }

        #endregion
    }

    [Serializable]
    public class NurbsCurveCompositeJoin
    {
        public Point3D Centre { get; set; }
        public double Radius { get; set; }
        public UniqueKey Curve1Key { get; set; }
        public UniqueKey Curve2Key { get; set; }
    }

    public class NurbsCurveSection
    {
        public NurbsCurve Curve;
        public Interval Interval;
    }

}
