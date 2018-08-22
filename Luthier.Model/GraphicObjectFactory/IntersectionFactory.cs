using Luthier.Core;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjectFactory
{
    public class IntersectionFactory
    {

        private ApplicationDocumentModel model;

        public IntersectionFactory(ApplicationDocumentModel model)
        {
            this.model = model;
        }

        public ApplicationDocumentModel GetModel() => model;


        public GraphicIntersection New(double x, double y)
        {
            var p1 = model.Point2DFactory().New(x, y);
            var p2 = model.Point2DFactory().New(x, y);
            var obj = new GraphicIntersection(p1.Key, p2.Key);
            model.Model.Add(obj);

            Log.Instance().Append(string.Format("Created Intersection. Key = {0}", obj.Key));
            return obj;
        }


        public GraphicPoint2D SetCentre(GraphicIntersection intersection, double x, double y)
        {
            var centre = model.Model[intersection.Centre] as GraphicPoint2D;
            centre.X = x;
            centre.Y = y;
            return centre;
        }


        public GraphicPoint2D SetRadius(GraphicIntersection intersection, double x, double y)
        {
            var radius = model.Model[intersection.Radius] as GraphicPoint2D;
            radius.X = x;
            radius.Y = y;
            return radius;
        }

        public void SetObject1(GraphicIntersection intersection, GraphicObjectBase obj)
        {
            intersection.Object1 = obj.Key;
        }

        public void SetObject2(GraphicIntersection intersection, GraphicObjectBase obj)
        {
            intersection.Object2 = obj.Key;
        }

        public void Delete(GraphicIntersection intersection)
        {
            if (!model.Model.ContainsObject(intersection)) throw new ArgumentException("intersection does not exist in data model");

            foreach (var key in new UniqueKey[] { intersection.Centre, intersection.Radius })
            {
                var point = (GraphicPoint2D)model.Model[key];
                point.parentObjectKeys.Remove(intersection.Key);
                if (point.parentObjectKeys.Count == 0) model.Model.Remove(point);
            }
            model.Model.Remove(intersection);

            Log.Instance().Append(string.Format("gauge.Delete: gauge.Key = {0}", intersection.Key));
        }


    }
}
