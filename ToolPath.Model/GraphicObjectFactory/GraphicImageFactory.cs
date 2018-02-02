using Luthier.Core;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjectFactory
{
    public class GraphicImageFactory
    {
        private ApplicationDocumentModel data;

        public GraphicImageFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public GraphicImage New(double x, double y)
        {
            var obj = new GraphicImage();
            data.objects.Add(obj);

            obj.pointsKeys[0] = data.Point2DFactory().New(x, y).Key;
            obj.pointsKeys[1] = data.Point2DFactory().New(x, y).Key;
            obj.pointsKeys[2] = data.Point2DFactory().New(x, y).Key;
            
            Log.Instance().Append(string.Format("Created Image. Key = {0}", obj.Key));

            return obj;
        }

        public void SetPoint(GraphicImage image, int pointIndex, double x, double y)
        {
            var point = data.Objects()[image.pointsKeys[pointIndex]] as GraphicPoint2D;
            if (point != null)
            {
                point.X = x;
                point.Y = y;
            }
        }

        public void SetPointsFixedAspectRatio(GraphicImage image, double x0, double y0, double x1, double y1)
        {
            var p0 = data.Objects()[image.pointsKeys[0]] as GraphicPoint2D;
            var p1 = data.Objects()[image.pointsKeys[1]] as GraphicPoint2D;
            var p2 = data.Objects()[image.pointsKeys[2]] as GraphicPoint2D;
            if (p0 != null && p1 != null && p2 != null)
            {
                double width = image.GetImage().Width;
                double height = image.GetImage().Height;

                p0.X = x0;
                p0.Y = y0;
                p1.X = x1;
                p1.Y = y1;

                p2.X = p0.X - (double)(height / width * (p1.Y - p0.Y));
                p2.Y = p0.Y + (double)(height / width * (p1.X - p0.X));
            }
        }

        public void Delete(GraphicImage image)
        {
            if (data.objects.Where(x => x.Key == image.Key).ToList().Count == 0) throw new ArgumentException("image does not exist in data model");

            foreach (var key in image.pointsKeys)
            {
                var point = (GraphicPoint2D)data.objects.Where(x => x.Key == key).First();
                point.parentObjectKeys.Remove(image.Key);
                if (point.parentObjectKeys.Count == 0) data.objects.Remove(point);
            }
            data.objects.Remove(image);

            Log.Instance().Append(string.Format("Image.Delete: Image.Key = {0}", image.Key));
        }

    }
}
