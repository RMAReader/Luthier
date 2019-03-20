using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClipperLib;

namespace Luthier.Geometry
{
    /*
     wrapper class around the open source Clipper library for clipping and offsetting polygons and lines
     http://www.angusj.com/delphi/clipper.php
         */

    public class ClipperWrapper
    {

        public static List<Polygon2D> OffsetPolygon(Polygon2D polygon, double radius)
        {
            ClipperOffset clipper = new ClipperOffset();
            double s = 10000000;
            clipper.MiterLimit = 2;
            clipper.AddPath(polygon.GetPoints().Select(p => new IntPoint((long)(p.X * s),(long)(p.Y * s))).ToList(), JoinType.jtMiter, EndType.etClosedPolygon);

            var solution = new List<List<IntPoint>>();
            clipper.Execute(ref solution, radius * s);

            return solution.Select(poly => new Polygon2D(poly.Select(p => new Point2D(p.X / s, p.Y / s)).ToList())).ToList();
        }

        public static List<Polygon2D> OffsetPolygons(List<Polygon2D> polygons, double radius)
        {
            ClipperOffset clipper = new ClipperOffset();
            double s = 10000000;
            clipper.MiterLimit = 2;

            foreach (var polygon in polygons)
            {
                clipper.AddPath(polygon.GetPoints().Select(p => new IntPoint((long)(p.X * s), (long)(p.Y * s))).ToList(), JoinType.jtMiter, EndType.etClosedPolygon);
            }

            var solution = new List<List<IntPoint>>();
            clipper.Execute(ref solution, radius * s);

            return solution.Select(poly => new Polygon2D(poly.Select(p => new Point2D(p.X / s, p.Y / s)).ToList())).ToList();
        }

    }
}
