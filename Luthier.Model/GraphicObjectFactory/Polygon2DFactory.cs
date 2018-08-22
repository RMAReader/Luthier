using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjects;
using Luthier.Core;

namespace Luthier.Model.GraphicObjectFactory
{
    public class Polygon2DFactory : IPolygon2DFactory
    {
        private ApplicationDocumentModel data;

        public Polygon2DFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public GraphicPolygon2D New()
        {
            var obj = new GraphicPolygon2D();
            data.Model.Add(obj);

            Log.Instance().Append(string.Format("Created Polygon2D. Key = {0}", obj.Key));

            return obj;
        }

        public GraphicPoint2D AppendPoint(GraphicPolygon2D line, double x, double y)
        {
            var point = data.Point2DFactory().New(x, y);
            line.AddPoint(point);

            Log.Instance().Append(string.Format("Polygon2D.AppendPoint: Polygon2D.Key = {0}, Point2D.Key = {1}", line.Key, point.Key));

            return point;
        }

        public void DeletePoint(GraphicPolygon2D line, GraphicPoint2D point)
        {
            if (!data.Model.ContainsObject(line)) throw new ArgumentException("Line does not exist in data model");
            if (!data.Model.ContainsObject(point)) throw new ArgumentException("Point does not exist in data model");

            line.RemovePoint(point);
            point.parentObjectKeys.Remove(line.Key);

            Log.Instance().Append(string.Format("Polygon2D.RemovePoint: Polygon2D.Key = {0}, Point2D.Key = {1}", line.Key, point.Key));

            if (point.parentObjectKeys.Count == 0) data.Model.Remove(point);
        }

        public void Delete(GraphicPolygon2D polygon)
        {
            if (!data.Model.ContainsObject(polygon)) throw new ArgumentException("Line does not exist in data model");

            foreach (var key in polygon.pointsKeys)
            {
                var point = (GraphicPoint2D)data.Model[key];
                point.parentObjectKeys.Remove(polygon.Key);
                if (point.parentObjectKeys.Count == 0) data.Model.Remove(point);
            }
            data.Model.Remove(polygon);

            Log.Instance().Append(string.Format("Polygon2D.Delete: Polygon2D.Key = {0}", polygon.Key));
        }
    }

}
