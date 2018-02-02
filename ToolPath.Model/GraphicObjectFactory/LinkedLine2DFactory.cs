using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.GraphicObjects;

namespace Luthier.Model.GraphicObjectFactory
{
    public class LinkedLine2DFactory : ILinkedLine2DFactory
    {
        private ApplicationDocumentModel data;

        public LinkedLine2DFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public GraphicLinkedLine2D New()
        {
            var obj = new GraphicLinkedLine2D();
            data.objects.Add(obj);
            return obj;
        }

        public GraphicPoint2D AppendPoint(GraphicLinkedLine2D line, double x, double y)
        {
            var point = data.Point2DFactory().New(x, y);
            line.AddPoint(point);
            return point;
        }

        public void DeletePoint(GraphicLinkedLine2D line, GraphicPoint2D point)
        {
            if (data.objects.Where(x => x.Key == line.Key).ToList().Count == 0) throw new ArgumentException("Line does not exist in data model");
            if (data.objects.Where(x => x.Key == point.Key).ToList().Count == 0) throw new ArgumentException("Point does not exist in data model");

            line.RemovePoint(point);
            point.parentObjectKeys.Remove(line.Key);

            if (point.parentObjectKeys.Count == 0) data.objects.Remove(point);
        }

        public void DeleteLine(GraphicLinkedLine2D line)
        {
            if (data.objects.Where(x => x.Key == line.Key).ToList().Count == 0) throw new ArgumentException("Line does not exist in data model");
            
            foreach(var key in line.pointsKeys)
            {
                var point = (GraphicPoint2D) data.objects.Where(x => x.Key == key).First();
                point.parentObjectKeys.Remove(line.Key);
                if (point.parentObjectKeys.Count == 0) data.objects.Remove(point);
            }
            data.objects.Remove(line);
        }
    }


 

}
