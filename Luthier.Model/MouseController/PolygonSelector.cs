using Luthier.Geometry;
using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.MouseController
{
    public class PolygonSelector : PanZoomMouseWheel
    {
        private readonly ApplicationDocumentModel model;
        private double selectionRadius;
        public List<IPolygon2D> selectedPolygons;
        

        public PolygonSelector(ApplicationDocumentModel model, double selectionRadius)
        {
            this.model = model;
            this.selectionRadius = selectionRadius;
            selectedPolygons = new List<IPolygon2D>();
        }


        public override void MouseLeftButtonDown(int x, int y)
        {
            ToggleSelection(x, y);
        }

        public override void MouseRightButtonDown(int x, int y)
        {
            ToggleSelection(x, y);
        }


        private void ToggleSelection(int x, int y)
        {
            IPolygon2D nearestPolygon = NearestPolygon(x, y);
            if (nearestPolygon != null)
            {
                if (selectedPolygons.Contains(nearestPolygon) == false)
                {
                    selectedPolygons.Add(nearestPolygon);
                }
                else
                {
                    selectedPolygons.Remove(nearestPolygon);
                }
            }
        }

        private IPolygon2D NearestPolygon(int x, int y)
        {
            PointF p = ViewMapper.TransformViewToModelCoordinates(new PointF(x, y));
            var range = (double)Math.Sqrt(selectionRadius * selectionRadius * ViewMapper.Scale * ViewMapper.Scale);
            var nearestObj = model.Model
                .Where(o => o is IPolygon2D)
                .Select(o => new { Distance = o.GetDistance(model, p.X, p.Y), Object = o })
                .Where(o => o.Distance < range && (o.Object is GraphicPoint2D) == false)
                .OrderBy(o => o.Distance)
                .FirstOrDefault();

            return (nearestObj == null) ? null : (IPolygon2D)nearestObj.Object;

        }

    }
}
