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

        public GraphicModel Model { get => model; }

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

        public void CreateMesh(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        {
 
            int nU = 10;
            int nV = 10;
            foreach (GraphicNurbSurface surface in model.Objects.Where(x => x is GraphicNurbSurface))
            {

                var prim = surface.ToPrimitive(this);

                for(int i=0; i<prim.CVCount(0); i++)
                {
                    for (int j = 0; j < prim.CVCount(1); j++)
                    {
                        var p = prim.GetCV(i, j);
                        p[2] = (p[0] * p[0] + p[1] * p[1]) / 1000;
                        prim.SetCV(i, j, p);
                    }
                }

               // var points2d = new List<Vector3d>();
                for (int i = 0; i < nU; i++)
                {
                    double u = (1 - (double)i / (nU)) * prim.Domain(0).Min + (double)i / (nU) * prim.Domain(0).Max;
                    for (int j=0; j < nV; j++)
                    {
                        double v = (1 - (double)j / (nV)) * prim.Domain(1).Min + (double)j / (nV) * prim.Domain(1).Max;
                        vertices.Add(new Vector3d(prim.Evaluate(u, v)));
                        normals.Add(new Vector3d(prim.EvaluateNormal(u, v)));
                    }
                }

                

                //var maxx = points2d.Max(p => p.x);
                //var maxy = points2d.Max(p => p.y);
                //var minx = points2d.Min(p => p.x);
                //var miny = points2d.Min(p => p.y);

                //foreach (var p in points2d)
                //{
                //    double u = (p.x - minx) / (maxx - minx);
                //    double v = (p.y - miny) / (maxy - miny);
                //    vertices.Add(new Vector3d(p.x, p.y, (u*u+v*v)*(maxx-minx)/5));
                //}
                for (int i = 0; i < nU - 1; i++)
                {
                    for (int j = 0; j < nV - 1; j++)
                    {
                        int sw = i * nV + j;
                        int se = sw + 1;
                        int nw = sw + nV;
                        int ne = nw + 1;
                        indices.AddRange(new int[] {sw,se,ne,ne,nw,sw });
                    }
                }
            }
        }
    }







}
