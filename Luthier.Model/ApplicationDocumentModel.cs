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
            return Serializer<GraphicModelStorage>.Serialize(model.GetStorage);
        }

        public void DeserialiseFromBytes(byte[] bytes)
        {
            var storage = Serializer<GraphicModelStorage>.Deserialize(bytes);
            model = new GraphicModel(storage);
        }

        public void New()
        {

        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is ApplicationDocumentModel == false) return false;
        //    var m = (ApplicationDocumentModel)obj;
        //    for (int i = 0; i < objects.Count; i++)
        //    {
        //        if (objects[i].Equals(m.objects[i]) == false) return false;
        //    }
        //    return true;
        //}
        //public override int GetHashCode()
        //{
        //    return objects.GetHashCode();
        //}

        public void CreateMesh_NurbsSurface(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        {
            vertices.Clear();
            normals.Clear();
            indices.Clear();

            int nU = 20;
            int nV = 20;
            foreach (GraphicNurbSurface surface in model.Where(x => x is GraphicNurbSurface))
            {

               

                // var points2d = new List<Vector3d>();
                for (int i = 0; i < nU; i++)
                {
                    double u = (1 - (double)i / (nU)) * surface.Domain0().Min + (double)i / (nU) * surface.Domain0().Max;
                    for (int j=0; j < nV; j++)
                    {
                        double v = (1 - (double)j / (nV)) * surface.Domain1().Min + (double)j / (nV) * surface.Domain1().Max;
                        vertices.Add(new Vector3d(surface.Evaluate(u, v)));
                        normals.Add(new Vector3d(surface.EvaluateNormal(u, v)));
                    }
                }

                for (int i = 0; i < nU - 1; i++)
                {
                    for (int j = 0; j < nV - 1; j++)
                    {
                        int sw = i * nV + j;
                        int se = sw + 1;
                        int nw = sw + nV;
                        int ne = nw + 1;
                        indices.AddRange(new int[] { sw, se, ne, ne, nw, sw });
                        indices.AddRange(new int[] { sw, ne, se, ne, sw, nw });
                    }
                }
            }
        }

        public void CreateMesh_NurbsControl(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        {
            vertices.Clear();
            normals.Clear();
            indices.Clear();

            foreach (GraphicNurbSurface surface in model.Where(x => x is GraphicNurbSurface))
            {
                for (int i = 0; i < surface.CvCount0; i++)
                {
                    for (int j = 0; j < surface.CvCount1; j++)
                    {
                        vertices.Add(new Vector3d(surface.GetCV(i, j)));
                        normals.Add(new Vector3d(0, 0, 1));
                    }
                }

                //vertical
                for (int i = 0; i < surface.CvCount0 - 1; i++)
                {
                    for (int j = 0; j < surface.CvCount1; j++)
                    {
                        var from = i * surface.CvCount1 + j;
                        var to = from + surface.CvCount1;
                        indices.AddRange(new int[] { from ,to });
                    }
                }

                //horizontal
                for (int i = 0; i < surface.CvCount0; i++)
                {
                    for (int j = 0; j < surface.CvCount1 - 1; j++)
                    {
                        var from = i * surface.CvCount1 + j;
                        var to = from + 1;
                        indices.AddRange(new int[] { from, to });
                    }
                }
            }
        }



        public void CreateMesh_NurbsCurve(List<Vector3d> vertices, List<Vector3d> normals, List<int> indices)
        {
            vertices.Clear();
            normals.Clear();
            indices.Clear();

            foreach (NurbsCurve curve in model.Where(x => x is NurbsCurve))
            {
                var startIndex = vertices.Count();
                for (int i = 0; i < curve.NumberOfPoints; i++)
                {
                    vertices.Add(new Vector3d(curve.GetCV(i)));
                    normals.Add(new Vector3d(0, 0, 1));
                }
                for (int i = startIndex; i < startIndex + curve.NumberOfPoints - 1; i++)
                {
                    indices.AddRange(new int[] { i, i + 1 });
                }

                if (curve.NumberOfPoints > 2)
                {
                    startIndex = vertices.Count();
                    foreach (var v in curve.ToLines(1000))
                    {
                        vertices.Add(new Vector3d(v));
                        normals.Add(new Vector3d(0, 0, 1));
                    }
                    for (int i = startIndex; i < startIndex + 1000; i++)
                    {
                        indices.AddRange(new int[] { i, i + 1 });
                    }
                }
            }

            
        }

    }







}
