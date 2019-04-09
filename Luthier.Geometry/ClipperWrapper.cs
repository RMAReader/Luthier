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
        private static double s = 10000000;


        public static List<Polygon2D> OffsetPolygon(Polygon2D polygon, double radius)
        {
            ClipperOffset clipper = new ClipperOffset();
            clipper.MiterLimit = 2;
            clipper.AddPath(polygon.GetPoints().Select(p => new IntPoint((long)(p.X * s),(long)(p.Y * s))).ToList(), JoinType.jtMiter, EndType.etClosedPolygon);

            var solution = new List<List<IntPoint>>();
            clipper.Execute(ref solution, radius * s);

            return solution.Select(poly => new Polygon2D(poly.Select(p => new Point2D(p.X / s, p.Y / s)).ToList())).ToList();
        }

        public static List<Polygon2D> OffsetPolygons(List<Polygon2D> polygons, double radius)
        {
            ClipperOffset clipper = new ClipperOffset();
            clipper.MiterLimit = 2;

            foreach (var polygon in polygons)
            {
                clipper.AddPath(polygon.GetPoints().Select(p => new IntPoint((long)(p.X * s), (long)(p.Y * s))).ToList(), JoinType.jtMiter, EndType.etClosedPolygon);
            }

            var solution = new List<List<IntPoint>>();
            clipper.Execute(ref solution, radius * s);

            return solution.Select(poly => new Polygon2D(poly.Select(p => new Point2D(p.X / s, p.Y / s)).ToList())).ToList();
        }

        public static List<Point2D> OffsetLine(List<Point2D> points, double radius)
        {

            //1. use Clipper to find surrounding polygon of line.  With "EndType.etOpenButt", the surrounding polygon passes through the start and end point of the line
            ClipperOffset clipper = new ClipperOffset();
            clipper.MiterLimit = 2;

            double delta = Math.Abs(radius) * s;

            clipper.AddPath(points.Select(p => new IntPoint((long)(p.X * s), (long)(p.Y * s))).ToList(), JoinType.jtMiter, EndType.etOpenButt);

            var solution = new List<List<IntPoint>>();
            clipper.Execute(ref solution, delta);

            var surroundingPolygon = solution.Select(poly => poly.Select(p => new Point2D(p.X / s, p.Y / s)).ToList()).First().ToList();

            //2. find sections of surrounding polygon that are the ends of the line
            IntPoint startIndices = new IntPoint(-1,-1);
            IntPoint endIndices = new IntPoint(-1, -1);

            for (int i=0; i< surroundingPolygon.Count; i++)
            {
                int j = (i + 1) % points.Count;

                var d1 = Algorithm.DistancePointToLine(points.First().Data, surroundingPolygon[i].Data, surroundingPolygon[j].Data);
                if(d1 < Math.Abs(radius) * 0.1)
                    startIndices = new IntPoint(i, j);

                var d2 = Algorithm.DistancePointToLine(points.Last().Data, surroundingPolygon[i].Data, surroundingPolygon[j].Data);
                if (d2 < Math.Abs(radius) * 0.1)
                    endIndices = new IntPoint(i, j);

            }

            //3. depending on sign(radius), return only left or right-hand part of surrounding polygon.
            var result = new List<Point2D>();
            if (radius > 0)
            {
                if (startIndices.Y < endIndices.X)
                {
                    for (int i = (int)startIndices.Y; i < (int)endIndices.X; i++)
                        result.Add(surroundingPolygon[i]);
                }
                else
                {
                    for (int i = (int)startIndices.Y; i < surroundingPolygon.Count; i++)
                        result.Add(surroundingPolygon[i]);

                    for (int i = 0; i < (int)endIndices.X; i++)
                        result.Add(surroundingPolygon[i]);
                }
            }
            else
            {
                if (endIndices.Y < startIndices.X)
                {
                    for (int i = (int)endIndices.Y; i < (int)startIndices.X; i++)
                        result.Add(surroundingPolygon[i]);
                }
                else
                {
                    for (int i = (int)endIndices.Y; i < surroundingPolygon.Count; i++)
                        result.Add(surroundingPolygon[i]);

                    for (int i = 0; i < (int)startIndices.X; i++)
                        result.Add(surroundingPolygon[i]);
                }
                result.Reverse();
            }

            return result;
        }

    }
}
