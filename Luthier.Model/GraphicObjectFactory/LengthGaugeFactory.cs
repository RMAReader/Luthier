﻿using Luthier.Core;
using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjectFactory
{
    public class LengthGaugeFactory
    {
        private ApplicationDocumentModel data;

        public LengthGaugeFactory(ApplicationDocumentModel data)
        {
            this.data = data;
        }

        public GraphicLengthGauge New(double x, double y)
        {
            var p1 = data.Point2DFactory().New(x, y);
            var p2 = data.Point2DFactory().New(x, y);
            var obj = new GraphicLengthGauge(p1.Key, p2.Key);

            data.Model.Add(obj);
            Log.Instance().Append(string.Format("Created LengthGauge. Key = {0}", obj.Key));

            return obj;
        }


        public void SetEndPoint(GraphicLengthGauge gauge, double x, double y)
        {
            var point = data.Model[gauge.toPoint] as GraphicPoint2D;
            if (point != null)
            {
                point.X = x;
                point.Y = y;
            }
        }


        public void Delete(GraphicLengthGauge gauge)
        {
            if (!data.Model.ContainsObject(gauge)) throw new ArgumentException("gauge does not exist in data model");

            foreach (var key in new UniqueKey[] { gauge.fromPoint, gauge.toPoint })
            {
                var point = (GraphicPoint2D)data.Model.VisibleObjects().Where(x => x.Key == key).First();
                point.parentObjectKeys.Remove(gauge.Key);
                if (point.parentObjectKeys.Count == 0) data.Model.Remove(point);
            }
            data.Model.Remove(gauge);

            Log.Instance().Append(string.Format("gauge.Delete: gauge.Key = {0}", gauge.Key));
        }

    }
}
