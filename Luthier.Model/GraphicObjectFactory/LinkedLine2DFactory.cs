using System;

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
            data.Model.Add(obj);
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
            if (!data.Model.ContainsObject(line)) throw new ArgumentException("Line does not exist in data model");
            if (!data.Model.ContainsObject(point)) throw new ArgumentException("Point does not exist in data model");

            line.RemovePoint(point);
            point.parentObjectKeys.Remove(line.Key);

            if (point.parentObjectKeys.Count == 0) data.Model.Remove(point);
        }

        public void DeleteLine(GraphicLinkedLine2D line)
        {
            if (!data.Model.ContainsObject(line)) throw new ArgumentException("Line does not exist in data model");
            
            foreach(var key in line.pointsKeys)
            {
                var point = (GraphicPoint2D) data.Model[key];
                point.parentObjectKeys.Remove(line.Key);
                if (point.parentObjectKeys.Count == 0) data.Model.Remove(point);
            }
            data.Model.Remove(line);
        }
    }


 

}
