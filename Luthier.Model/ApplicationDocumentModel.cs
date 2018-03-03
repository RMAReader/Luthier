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
        public List<GraphicObjectBase> objects { get => model.Objects; set => model.Objects = value; }


        public ApplicationDocumentModel()
        {
            model = new GraphicModel();
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
        public Dictionary<UniqueKey, GraphicObjectBase> Objects() => objects.ToDictionary(x => x.Key);

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



        public byte[] SerialiseToBytes()
        {
            return Luthier.Core.Serializer<GraphicModel>.Serialize(model);
        }

        public void DeserialiseFromBytes(byte[] bytes)
        {
            model = Luthier.Core.Serializer<GraphicModel>.Deserialize(bytes);
        }

        public void New()
        {
            objects = new List<GraphicObjectBase>();
        }

        public override bool Equals(object obj)
        {
            if (obj is ApplicationDocumentModel == false) return false;
            var m = (ApplicationDocumentModel)obj;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Equals(m.objects[i]) == false) return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return objects.GetHashCode();
        }

        public DMesh3 CreateMesh()
        {
            var vertices = new List<Vector3d>();
            var indices = new List<int>();
            foreach (GraphicNurbSurface surface in model.Objects.Where(x => x is GraphicNurbSurface))
            {
                var points2d = surface.cvArray.Select(x => (GraphicPoint2D)Objects()[x]);
                var maxx = points2d.Max(p => p.X);
                var maxy = points2d.Max(p => p.Y);
                var minx = points2d.Min(p => p.X);
                var miny = points2d.Min(p => p.Y);

                foreach (var p in points2d)
                {
                    double u = (p.X - minx) / (maxx - minx);
                    double v = (p.Y - miny) / (maxy - miny);
                    vertices.Add(new Vector3d(p.X, p.Y, (u*u+v*v)*(maxx-minx)/5));
                }
                for (int i = 0; i < surface.cv_count0 - 1; i++)
                {
                    for (int j = 0; j < surface.cv_count1 - 1; j++)
                    {
                        int sw = i * surface.cv_count0 + j;
                        int se = sw + 1;
                        int nw = sw + surface.cv_count0;
                        int ne = nw + 1;
                        indices.AddRange(new int[] {sw,se,ne,ne,nw,sw });
                    }
                }
            }
            return DMesh3Builder.Build<Vector3d, int, int>(vertices, indices);
        }
    }







}
