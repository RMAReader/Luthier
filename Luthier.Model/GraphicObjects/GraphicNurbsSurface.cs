using Luthier.Geometry.Nurbs;
using SharpHelper;
using System;
using System.Collections.Generic;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicNurbsSurface : GraphicObjectBase, IDrawablePhongSurface, IDrawableLines
    {
        public NurbsSurface Surface;
       
        public GraphicNurbsSurface()  { }
        public GraphicNurbsSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            Surface = new NurbsSurface(dimension, bIsRational, order0, order1, cv_count0, cv_count1);
        }
        
        public IEnumerable<IDraggable2d> GetDraggableObjects2d()
        {
            for (int i = 0; i < Surface.CvCount0; i++)
            {
                for (int j = 0; j < Surface.CvCount1; j++)
                {
                    yield return new DraggableSurfaceCV2d(this, i, j);
                }
            }
        }

        public IEnumerable<IDraggable> GetDraggableObjects()
        {
            for (int i = 0; i < Surface.CvCount0; i++)
            {
                for (int j = 0; j < Surface.CvCount1; j++)
                {
                    yield return new DraggableSurfaceCV(this, i, j);
                }
            }
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }

        #region "IDrawableXXX implementations"

        public void GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices)
        {
            
            int nU = 200;
            int nV = 200;

            int indexOffset = vertices.Count;

            for (int i = 0; i < nU; i++)
            {
                double u = (1 - (double)i / (nU - 1)) * Surface.Domain0().Min + (double)i / (nU - 1) * Surface.Domain0().Max;
                for (int j = 0; j < nV; j++)
                {
                    double v = (1 - (double)j / (nV - 1)) * Surface.Domain1().Min + (double)j / (nV - 1) * Surface.Domain1().Max;

                    var position = Surface.Evaluate(u, v);
                    var normal = Surface.EvaluateNormal(u, v);

                    vertices.Add(new TangentVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3((float)normal[0], (float)normal[1], (float)normal[2]),
                        Tangent = new SharpDX.Vector3(1, 0, 0),
                        Binormal = new SharpDX.Vector3(0, 1, 0),
                        TextureCoordinate = new SharpDX.Vector2(0, 0)
                    });
                }
            }
            
            for (int i = 0; i < nU - 1; i++)
            {
                for (int j = 0; j < nV - 1; j++)
                {
                    int sw = i * nV + j + indexOffset;
                    int se = sw + 1;
                    int nw = sw + nV;
                    int ne = nw + 1;
                    indices.AddRange(new int[] { sw, se, ne, ne, nw, sw });
                    indices.AddRange(new int[] { sw, ne, se, ne, sw, nw });
                }
            }
            
        }


        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            AddControlNetEdges(ref vertices, ref indices);
            
        }

        public void AddControlNetEdges(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            var indexOffset = vertices.Count;

            for (int i = 0; i < Surface.CvCount0; i++)
            {
                for (int j = 0; j < Surface.CvCount1; j++)
                {
                    var position = Surface.GetCV(i, j);

                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3(0, 0, 1),
                        Color = new SharpDX.Vector4(1, 1, 0, 1)
                    });
                }
            }

            //vertical
            for (int i = 0; i < Surface.CvCount0 - 1; i++)
            {
                for (int j = 0; j < Surface.CvCount1; j++)
                {
                    var from = i * Surface.CvCount1 + j + indexOffset;
                    var to = from + Surface.CvCount1;
                    indices.AddRange(new int[] { from, to });
                }
            }

            //horizontal
            for (int i = 0; i < Surface.CvCount0; i++)
            {
                for (int j = 0; j < Surface.CvCount1 - 1; j++)
                {
                    var from = i * Surface.CvCount1 + j + indexOffset;
                    var to = from + 1;
                    indices.AddRange(new int[] { from, to });
                }
            }
        }


        #endregion
    }


    public class DraggableSurfaceCV2d : IDraggable2d
    {
        private GraphicNurbsSurface surface;
        private int i;
        private int j;

        public DraggableSurfaceCV2d(GraphicNurbsSurface surface, int i, int j)
        {
            this.surface = surface;
            this.i = i;
            this.j = j;
        }

        public double GetDistance(double x, double y)
        {
            double cvx = surface.Surface.GetCV(i, j)[0];
            double cvy = surface.Surface.GetCV(i, j)[1];
            return Math.Sqrt((cvx - x) * (cvx - x) + (cvy - y) * (cvy - y)); ;
        }

        public void Set(double x, double y)
        {
            double[] cv = new double[surface.Surface.Dimension];
            cv[0] = x;
            cv[1] = y;
            surface.Surface.SetCV(i, j, cv);
        }
    }

    public class DraggableSurfaceCV : IDraggable
    {
        private GraphicNurbsSurface surface;
        private int i;
        private int j;

        public DraggableSurfaceCV(GraphicNurbsSurface surface, int i, int j)
        {
            this.surface = surface;
            this.i = i;
            this.j = j;
        }

        public double[] Values
        {
            get => surface.Surface.GetCV(i, j);
            set => surface.Surface.SetCV(i, j, value);
        }
    }
}
