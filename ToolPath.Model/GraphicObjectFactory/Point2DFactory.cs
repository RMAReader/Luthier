using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjects;
using Luthier.Core;

namespace Luthier.Model.GraphicObjectFactory
{
    public class Point2DFactory : IPoint2DFactory
    {
        private ApplicationDocumentModel data;

        public Point2DFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public void Delete(GraphicPoint2D point)
        {
            if (point.parentObjectKeys.Count == 0) data.objects.Remove(point);

            Log.Instance().Append(string.Format("Deleted Point2D({0},{1}). Key = {2}", point.X, point.Y, point.Key));

        }

        public GraphicPoint2D New(double x, double y)
        {
            var obj = new GraphicPoint2D(x, y);
            data.objects.Add(obj);

            Log.Instance().Append(string.Format("Created Point2D({0},{1}). Key = {2}",x,y,obj.Key));

            return obj;
        }
    }

}
