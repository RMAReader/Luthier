using Luthier.Geometry;
using Luthier.Model.MouseController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.Presenter
{
    
    public class Drawing2DPresenter : IDisposable
    {

        private readonly IApplicationDocumentModel model;

        public Drawing2DPresenter(IApplicationDocumentModel model)
        {
            this.model = model;
            MouseController = model.MouseControllerFactory().PointSelector(20);

         }

        private ViewMapper2D _viewMapper = new ViewMapper2D();
        public ViewMapper2D ViewMapper
        {
            get => _viewMapper;
            set => _viewMapper = value;
        }

        private IMouseController _mouseController;
        public IMouseController MouseController
        {
            get { return _mouseController; }
            set
            {
                if (_mouseController != null) _mouseController.Close();
                _mouseController = value;
                _mouseController.ViewMapper = ViewMapper;

            }
        }

        public IApplicationDocumentModel GetModel() => model;
        


        private Pen curvePen = new Pen(Color.Red) { Width = 2.0f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
        private Pen CurvePen
        {
            get => curvePen;
            set
            {
                if (curvePen != null) curvePen.Dispose();
                curvePen = value;
            }
        }

        private Pen curveControlPen = new Pen(Color.OrangeRed) { Width = 1.0f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
        private Pen CurveControlPen
        {
            get => curveControlPen;
            set
            {
                if (curveControlPen != null) curveControlPen.Dispose();
                curveControlPen = value;
            }
        }

        private Pen linePen = new Pen(Color.Navy) { Width = 0.5f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
        private Pen LinePen
        {
            get => linePen;
            set
            {
                if (linePen != null) linePen.Dispose();
                linePen = value;
            }
        }

        private Pen polygonPen = new Pen(Color.Purple) { Width = 0.5f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
        private Pen PolygonPen
        {
            get => polygonPen;
            set
            {
                if (polygonPen != null) polygonPen.Dispose();
                polygonPen = value;
            }
        }

        private Pen imageBorderPen = new Pen(Color.Red) { Width = 1.0f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
        private Pen ImageBorderPen
        {
            get => imageBorderPen;
            set
            {
                if (imageBorderPen != null) imageBorderPen.Dispose();
                imageBorderPen = value;
            }
        }

        private Pen lengthGaugePen = new Pen(Color.DarkGreen) { Width = 1.0f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round, };
        private Pen LengthGaugePen
        {
            get => lengthGaugePen;
            set
            {
                if (lengthGaugePen != null) lengthGaugePen.Dispose();
                lengthGaugePen = value;
            }
        }

        private Pen intersectionPen = new Pen(Color.Green) { Width = 3.0f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round, };
        private Pen IntersectionPen
        {
            get => intersectionPen;
            set
            {
                if (intersectionPen != null) intersectionPen.Dispose();
                intersectionPen = value;
            }
        }


        private Pen toolPathPen = new Pen(Color.HotPink) { Width = 1.0f, LineJoin = System.Drawing.Drawing2D.LineJoin.Round, };
        private Pen ToolPathPen
        {
            get => toolPathPen;
            set
            {
                if (toolPathPen != null) toolPathPen.Dispose();
                toolPathPen = value;
            }
        }

        private Brush selectedPolygonBrush = new SolidBrush(Color.LightGreen);
        private Brush SelectedPolygonBrush
        {
            get => selectedPolygonBrush;
            set
            {
                if (selectedPolygonBrush != null) selectedPolygonBrush.Dispose();
                selectedPolygonBrush = value;
            }
        }

        public void Paint(Graphics g)
        {
            var drawingAdapter = model.AdapterSystemDrawing();

            //draw images
            foreach (var data in drawingAdapter.GetImages())
            {
                if (data.image != null)
                {
                    g.DrawImage(data.image, ViewMapper.TransformModelToViewCoordinates(data.points));
                }

                var viewPoints = ViewMapper.TransformModelToViewCoordinates(data.points);
                var border = new PointF[4] {
                    viewPoints[0],
                    viewPoints[1],
                    new PointF(viewPoints[1].X+viewPoints[2].X-viewPoints[0].X,viewPoints[1].Y+viewPoints[2].Y-viewPoints[0].Y),
                    viewPoints[2],
                };
                g.DrawPolygon(ImageBorderPen, border);
            }


            //draw lines
             foreach (var line in drawingAdapter.GetLinkedLine2DPoints())
            {
                g.DrawLines(LinePen, ViewMapper.TransformModelToViewCoordinates(line));
            }


            //draw curves
            foreach (var lines in drawingAdapter.GetCurveControlPoints())
            {
                var points = ViewMapper.TransformModelToViewCoordinates(lines);
                g.DrawLines(CurveControlPen, points);
                foreach (var point in points)
                {
                    g.DrawPolygon(CurveControlPen, new PointF[] 
                    {
                        new PointF(point.X + 5, point.Y + 5),
                        new PointF(point.X + 5, point.Y - 5),
                        new PointF(point.X - 5, point.Y - 5),
                        new PointF(point.X - 5, point.Y + 5),
                    });
                }
            }
            foreach (var lines in drawingAdapter.GetCurves())
            {
                if(lines.Length > 1) g.DrawLines(CurvePen, ViewMapper.TransformModelToViewCoordinates(lines));
            }


            //draw polygons
            foreach (var polygon in drawingAdapter.GetPolygon2DPoints())
            {
                g.DrawPolygon(PolygonPen, ViewMapper.TransformModelToViewCoordinates(polygon));
            }


            //draw intersections
            foreach (var intersection in drawingAdapter.GetIntersections())
            {
                var centre = ViewMapper.TransformModelToViewCoordinates(intersection.Centre);
                var radius = ViewMapper.TransformModelToViewCoordinates(intersection.Radius);
                g.DrawLine(IntersectionPen, centre, radius);
                var r = (double) Math.Sqrt((centre.X - radius.X) * (centre.X - radius.X) + (centre.Y - radius.Y) * (centre.Y - radius.Y));
                g.DrawEllipse(IntersectionPen, (float)(centre.X - r), (float)(centre.Y - r), (float)(2 * r), (float)(2 * r));
            }

            //draw composite polygons
            foreach(var composite in drawingAdapter.GetCompositePolygons())
            {
                if (composite.Length > 2)
                {
                    g.DrawPolygon(IntersectionPen, ViewMapper.TransformModelToViewCoordinates(composite));
                }
                else if(composite.Length > 1)
                {
                    g.DrawLines(IntersectionPen, ViewMapper.TransformModelToViewCoordinates(composite));
                }
            }

            //draw length gauges
            foreach (var gauge in drawingAdapter.GetLengthGauges())
            {
                var p1 = ViewMapper.TransformModelToViewCoordinates(gauge.P1);
                var p2 = ViewMapper.TransformModelToViewCoordinates(gauge.P2);
                g.DrawLine(LengthGaugePen, p1, p2);
                g.DrawString(
                    String.Format("{0} mm", gauge.Length.ToString("0.00")),
                    new Font("Arial", 12),
                    new SolidBrush(Color.Black),
                    0.5f * (p1.X + p2.X),
                    0.5f * (p1.Y + p2.Y)
                );
            }


            //draw selected polygons
            if (MouseController is PolygonSelector)
            {
                foreach (var polygon in ((PolygonSelector) MouseController).selectedPolygons)
                {
                    var polygonPoints = polygon.ToPolygon2D(model).GetPoints().Select(p => new PointF((float)p.x, (float)p.y)).ToArray();
                    g.FillPolygon(selectedPolygonBrush, ViewMapper.TransformModelToViewCoordinates(polygonPoints));
                }
            }


            //draw surface
            foreach(var controlNet in drawingAdapter.GetSurfaceControlPoints())
            {
                var net = ViewMapper.TransformModelToViewCoordinates(controlNet);
                for (int i = 0; i < net.Length; i+=2)
                {
                    g.DrawLine(CurveControlPen, net[i], net[i+1]);
                    g.DrawPolygon(CurveControlPen, new PointF[]
                    {
                        new PointF(net[i].X + 5, net[i].Y + 5),
                        new PointF(net[i].X + 5, net[i].Y - 5),
                        new PointF(net[i].X - 5, net[i].Y - 5),
                        new PointF(net[i].X - 5, net[i].Y + 5),
                    });
                    g.DrawPolygon(CurveControlPen, new PointF[]
                    {
                        new PointF(net[i+1].X + 5, net[i+1].Y + 5),
                        new PointF(net[i+1].X + 5, net[i+1].Y - 5),
                        new PointF(net[i+1].X - 5, net[i+1].Y - 5),
                        new PointF(net[i+1].X - 5, net[i+1].Y + 5),
                    });
                }
            }
                 


            //draw toolpaths
            foreach (var path in drawingAdapter.GetToolPath())
            {
                if (path.Length > 1) g.DrawLines(ToolPathPen, ViewMapper.TransformModelToViewCoordinates(path));
            }

        }


        public void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    MouseController.MouseLeftButtonDown(e.X, e.Y);
                    break;

                case MouseButtons.Right:
                    MouseController.MouseRightButtonDown(e.X, e.Y);
                    break;

                case MouseButtons.Middle:
                    MouseController.MouseMiddleButtonDown(e.X,e.Y);
                    break;

            }
        }

        public void MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        public void MouseHover(object sender, MouseEventArgs e)
        {
        }

        public void MouseLeave(object sender, MouseEventArgs e)
        {
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            MouseController.MouseMove(e.X, e.Y);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    MouseController.MouseLeftButtonUp(e.X, e.Y);
                    break;

                case MouseButtons.Right:
                    MouseController.MouseRightButtonUp(e.X, e.Y);
                    break;

                case MouseButtons.Middle:
                    MouseController.MouseMiddleButtonUp(e.X, e.Y);
                    break;
            }
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            MouseController.MouseWheel(e.X, e.Y, e.Delta);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CurvePen.Dispose();
                    CurveControlPen.Dispose();
                    ImageBorderPen.Dispose();
                    LinePen.Dispose();
                    PolygonPen.Dispose();
                    SelectedPolygonBrush.Dispose();
                    IntersectionPen.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
