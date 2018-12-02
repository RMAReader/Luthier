using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Luthier.Core;
using Luthier.Model.GraphicObjectFactory;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using Luthier.Model.ToolPathSpecification;
using Luthier.Model.ToolPathSpecificationFactory;
using Luthier.CncTool;
using g3;
using Luthier.Geometry.Nurbs;
using Luthier.Geometry;
using Luthier.Geometry.Optimization;

namespace Luthier.Model
{

    public class ApplicationDocumentModel : IApplicationDocumentModel
    {
        private Point2DFactory point2DFactory;
        private Polygon2DFactory polygon2DFactory;
        private LinkedLine2DFactory linkedLine2DFactory;
        private BSplineFactory bSplineFactory;
        private GraphicImageFactory imageFactory;
        private LengthGaugeFactory lengthGaugeFactory;
        private IntersectionFactory intersectionFactory;
        private CompositePolygonFactory compositePolygonFactory;

        private AdapterSystemDrawing adapterSystemDrawing;
        private MouseControllerFactory mouseControllerFactory;
        private ToolPathFactory toolPathFactory;

        public GraphicModel model;


        public ApplicationDocumentModel()
        {
            model = new GraphicModel();
            var plane = GraphicPlane.CreateRightHandedXY(new double[] { 0, 0, 0 });
            var layer = new GraphicLayer();
            layer.Objects = new List<UniqueKey> { plane.Key };
            plane.LayerKey = layer.Key;

            
            model.Add(layer);
            model.Add(plane);
            
            GetFittedSurface();
            for(int i=0; i<_fittingHistory.Count; i++)
            {
                var surface = new GraphicNurbsSurface();
                surface.Surface = _fittingHistory[i];
                surface.Name = $"surface {i}";
                layer.AddToLayer(surface);
                model.Add(surface);
            }

            point2DFactory = new Point2DFactory(this);
            polygon2DFactory =new Polygon2DFactory(this);
            linkedLine2DFactory = new LinkedLine2DFactory(this);
            bSplineFactory = new BSplineFactory(this);
            imageFactory = new GraphicImageFactory(this);
            lengthGaugeFactory = new LengthGaugeFactory(this);
            intersectionFactory = new IntersectionFactory(this);
            compositePolygonFactory = new CompositePolygonFactory(this);

            adapterSystemDrawing = new AdapterSystemDrawing(this);
            mouseControllerFactory = new MouseControllerFactory(this);
            toolPathFactory = new ToolPathFactory(this);
        }

        /* Factory methods for creating graphic objects */
        public IPoint2DFactory Point2DFactory() => point2DFactory;

        public IPolygon2DFactory Polygon2DFactory() => polygon2DFactory;

        public ILinkedLine2DFactory LinkedLine2DFactory() => linkedLine2DFactory;

        public BSplineFactory BSplineFactory() => bSplineFactory;

        public GraphicImageFactory ImageFactory() => imageFactory;

        public LengthGaugeFactory LengthGaugeFactory() => lengthGaugeFactory;

        public IntersectionFactory IntersectionFactory() => intersectionFactory;

        public CompositePolygonFactory CompositePolygonFactory() => compositePolygonFactory;

        public IAdapterSystemDrawing AdapterSystemDrawing() => adapterSystemDrawing;

        public IMouseControllerFactory MouseControllerFactory() => mouseControllerFactory;

        public IToolPathFactory ToolPathFactory() => toolPathFactory;

        public GraphicModel Model { get => model; }

        public byte[] SerialiseToBytes()
        {
            return Serializer<GraphicModelStorage>.Serialize(model.GetStorage());
        }

        public void DeserialiseFromBytes(byte[] bytes, string path)
        {
            var storage = Serializer<GraphicModelStorage>.Deserialize(bytes);
            model = new GraphicModel(storage, path);
        }

        public void New()
        {

        }



        private NurbsSurface GetFittedSurface()
        {
            //create point cloud for a spherical patch
            var cloudPoints = new List<double[]>();
            int numberOfPoints = 50;
            double minX = -10;
            double minY = -10;
            double maxX = 10;
            double maxY = 10;
            double radius = 20;
            for (int i = 0; i < numberOfPoints; i++)
            {
                double u = (double)i / (numberOfPoints - 1);
                for (int j = 0; j < numberOfPoints; j++)
                {
                    double v = (double)j / (numberOfPoints - 1);

                    double x = (1 - u) * minX + u * maxX;
                    double y = (1 - v) * minY + v * maxY;
                    double z = Math.Max(0,Math.Sqrt(radius * radius - x * x - y * y) - radius + 3);
                    cloudPoints.Add(new double[] { x, y, z });
                }
            }
            var cloud = new PointCloud(cloudPoints);

            var surface = new NurbsSurface(dimension: 3, bIsRational: false, order0: 3, order1: 3, cv_count0: 30, cv_count1: 30);
            
            for(int i=0; i< surface.knotArray0.Length; i++)
            {
                surface.knotArray0[i] = (double)i / (surface.knotArray0.Length - 1);
            }
            for (int i = 0; i < surface.knotArray1.Length; i++)
            {
                surface.knotArray1[i] = (double)i / (surface.knotArray1.Length - 1);
            }

            for (int i=0; i<surface.CvCount0; i++)
            {
                double u = (double)i / (surface.CvCount0 - 1);
                for (int j = 0; j < surface.CvCount1; j++)
                {
                    double v = (double)j / (surface.CvCount1 - 1);

                    double x = (1 - u) * minX * 2 + u * maxX * 2;
                    double y = (1 - v) * minY * 2 + v * maxY * 2;
                    double z = 0;// Math.Sqrt(radius * radius - x * x - y * y) - radius;

                    surface.SetCV(i, j, new double[] { x, y, z });
                }
            }

            _fittingHistory = new List<NurbsSurface>();
            _fittingHistory.Add(surface);
            
            var fitter = new NurbsSurfaceFitterAccordNet();

            fitter.IterationCompleteEvent += Fitter_IterationCompleteEvent;


            fitter.Fit(surface, cloud);

            return fitter.Solution;
        }

        private List<NurbsSurface> _fittingHistory;

        private void Fitter_IterationCompleteEvent(object sender, IterationCompleteEventArgs e)
        {
            var newSurface = _fittingHistory.Last().Clone();
            Array.Copy(e.Parameters, newSurface.controlPoints.Data, e.Parameters.Length);
            _fittingHistory.Add(newSurface);
        }
    }




}
