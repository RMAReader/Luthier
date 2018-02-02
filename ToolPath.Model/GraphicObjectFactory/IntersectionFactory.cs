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
            model.objects.Add(obj);

            Log.Instance().Append(string.Format("Created Intersection. Key = {0}", obj.Key));
            return obj;
        }


        public GraphicPoint2D SetCentre(GraphicIntersection intersection, double x, double y)
        {
            var centre = model.Objects()[intersection.Centre] as GraphicPoint2D;
            centre.X = x;
            centre.Y = y;
            return centre;
        }


        public GraphicPoint2D SetRadius(GraphicIntersection intersection, double x, double y)
        {
            var radius = model.Objects()[intersection.Radius] as GraphicPoint2D;
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
            if (model.objects.Where(x => x.Key == intersection.Key).ToList().Count == 0) throw new ArgumentException("intersection does not exist in data model");

            foreach (var key in new UniqueKey[] { intersection.Centre, intersection.Radius })
            {
                var point = (GraphicPoint2D)model.objects.Where(x => x.Key == key).First();
                point.parentObjectKeys.Remove(intersection.Key);
                if (point.parentObjectKeys.Count == 0) model.objects.Remove(point);
            }
            model.objects.Remove(intersection);

            Log.Instance().Append(string.Format("gauge.Delete: gauge.Key = {0}", intersection.Key));
        }


    }
}
