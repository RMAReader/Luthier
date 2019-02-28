using Luthier.Geometry.Intersections;
using Luthier.Geometry.Nurbs;
using Luthier.Model.GraphicObjects.Interfaces;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicNurbsSurface : 
        GraphicObjectBase, 
        IDrawablePhongSurface, 
        IDrawableStaticColouredSurface,
        IDrawableLines,
        IScalable,
        ISelectable
    {
        public NurbsSurface Surface;
       
        public bool DrawControlNet { get; set; }
        public bool DrawKnotSpans { get; set; }
        public bool DrawSurface { get; set; }
        public SurfaceDrawingStyle SurfaceDrawingStyle { get; set; }
        public bool IsSelected { get; set; }

        public GraphicNurbsSurface()  { SurfaceDrawingStyle = SurfaceDrawingStyle.PhongShadedColour; }
        public GraphicNurbsSurface(int dimension, bool bIsRational, int order0, int order1, int cv_count0, int cv_count1)
        {
            Surface = new NurbsSurface(dimension, bIsRational, order0, order1, cv_count0, cv_count1);
            DrawControlNet = false;
            DrawKnotSpans = true;
            DrawSurface = true;
            SurfaceDrawingStyle = SurfaceDrawingStyle.PhongShadedColour;
        }
        public GraphicNurbsSurface(NurbsSurface surface)
        {
            Surface = surface;
            DrawControlNet = false;
            DrawKnotSpans = true;
            DrawSurface = true;
            SurfaceDrawingStyle = SurfaceDrawingStyle.PhongShadedColour;
        }

        public IEnumerable<IDraggable2d> GetDraggableObjects2d()
        {
            if (!DrawControlNet) yield break;
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
            if (!DrawControlNet) yield break;
            for (int i = 0; i < Surface.CvCount0; i++)
            {
                for (int j = 0; j < Surface.CvCount1; j++)
                {
                    yield return new DraggableSurfaceCV(this, i, j);
                }
            }
        }

        public IEnumerable<SelectableKnot> GetEdgeKnots()
        {
            if (!DrawKnotSpans) yield break;

            int minI = Knot.MinIndex(Surface.knotArray0, Surface.Order0);
            int maxI = Knot.MaxIndex(Surface.knotArray0, Surface.Order0);
            int minJ = Knot.MinIndex(Surface.knotArray1, Surface.Order1);
            int maxJ = Knot.MaxIndex(Surface.knotArray1, Surface.Order1);

            for (int i = minI; i <= maxI; i++)
            {
                yield return new SelectableKnot(this, i, minJ);
                yield return new SelectableKnot(this, i, maxJ);
            }

            for (int j = minJ + 1; j < maxJ; j++)
            {
                yield return new SelectableKnot(this, minI, j);
                yield return new SelectableKnot(this, maxI, j);
            }
        }

        public RayIntersection GetRayIntersection(double[] from, double[] to)
        {
            var i = IntersectionCalculatorRayNurbsSurface.GetIntersect(from, to, Surface);

            return new RayIntersection {
                Object = this,
                IntersectInWorldCoords = i.Coordinates,
                ObjectParameters = i.SurfaceParameters,
                RayParameter = i.RayParameter,
                ObjectHit = i.IsHit };
        }



        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            throw new NotImplementedException();
        }


        public void ScaleObject(double scaleFactor)
        {
            Surface = Surface.ScaleSurface(scaleFactor);
        }


        #region "IDrawableXXX implementations"

        void IDrawablePhongSurface.GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices)
        {
            if (SurfaceDrawingStyle == SurfaceDrawingStyle.PhongShadedColour)
                GetVertexAndIndexListsFullSurfaceDomain(ref vertices, ref indices);
        }

        private void GetVertexAndIndexListsFullSurfaceDomain(ref List<TangentVertex> vertices, ref List<int> indices)
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

        void IDrawableStaticColouredSurface.GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            if(SurfaceDrawingStyle == SurfaceDrawingStyle.HeatmapColour)
                GetVertexAndIndexListsFullSurfaceDomainHeatMap(ref vertices, ref indices);
        }

        private void GetVertexAndIndexListsFullSurfaceDomainHeatMap(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            //find range of surface
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
            double[] cv = new double[3];
            for (int i = 0; i < Surface.controlPoints.CvCount[0]; i++)
            {
                for (int j = 0; j < Surface.controlPoints.CvCount[1]; j++)
                {
                    Surface.controlPoints.GetCV(cv, i, j);
                    if (cv[2] < minZ) minZ = cv[2];
                    if (cv[2] > maxZ) maxZ = cv[2];
                }
            }

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
                    //var color = (maxZ - minZ > 0) ?
                    //    new SharpDX.Vector4((float)((position[2] - minZ) / (maxZ - minZ)), 0, (float)((maxZ - position[2]) / (maxZ - minZ)), 0) :
                    //    new SharpDX.Vector4(0, 0, 1, 0);

                    var c = (position[2] - minZ) / (maxZ - minZ);

                    var color = (c < 0.1) ? SharpDX.Color.DarkBlue :
                        (c < 0.2) ? SharpDX.Color.Blue :
                        (c < 0.3) ? SharpDX.Color.LightBlue :
                        (c < 0.4) ? SharpDX.Color.Turquoise :
                        (c < 0.5) ? SharpDX.Color.Green :
                        (c < 0.6) ? SharpDX.Color.YellowGreen :
                        (c < 0.7) ? SharpDX.Color.Yellow :
                        (c < 0.8) ? SharpDX.Color.Orange :
                        (c < 0.9) ? SharpDX.Color.OrangeRed :
                        (c <= 1.0) ? SharpDX.Color.Red :
                        SharpDX.Color.DarkBlue;


                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3((float)normal[0], (float)normal[1], (float)normal[2]),
                        Color = color.ToVector4(),
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


        void IDrawableLines.GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            if(DrawControlNet) AddControlNetEdges(ref vertices, ref indices);
            if(DrawKnotSpans) AddKnotSpanLines(ref vertices, ref indices);
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

        public void AddKnotSpanLines(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            int nU = 200;
            int nV = 200;

            //knot lines in U direction
            for (int j = Surface.Order1 - 1; j <= Surface.knotArray1.Length - Surface.Order1; j++)
            {
                var indexOffset = vertices.Count;

                double v = Surface.knotArray1[j];
                for (int i = 0; i < nU; i++)
                {
                    double u = (1 - (double)i / (nU - 1)) * Surface.Domain0().Min + (double)i / (nU - 1) * Surface.Domain0().Max;
                
                    var position = Surface.Evaluate(u, v);

                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3(0, 0, 1),
                        Color = new SharpDX.Vector4(1, 0, 1, 1)
                    });
                }

                for (int i = 0; i < nU - 1; i++)
                {
                    var from = indexOffset + i;
                    var to = from + 1;
                    indices.AddRange(new int[] { from, to });
                }
            }

            //knot lines in V direction
            for (int i = Surface.Order0 - 1; i <= Surface.knotArray0.Length - Surface.Order0; i++)
            {
                var indexOffset = vertices.Count;

                double u = Surface.knotArray0[i];
                for (int j = 0; j < nV; j++)
                {
                    double v = (1 - (double)j / (nV - 1)) * Surface.Domain1().Min + (double)j / (nV - 1) * Surface.Domain1().Max;

                    var position = Surface.Evaluate(u, v);

                    vertices.Add(new StaticColouredVertex
                    {
                        Position = new SharpDX.Vector3((float)position[0], (float)position[1], (float)position[2]),
                        Normal = new SharpDX.Vector3(0, 0, 1),
                        Color = new SharpDX.Vector4(1, 0, 1, 1)
                    });
                }

                for (int j = 0; j < nV - 1; j++)
                {
                    var from = indexOffset + j;
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

    public class SelectableKnot
    {
        public GraphicNurbsSurface Surface { get; private set; }
        public int[] KnotIndices { get; private set; }
        public double[] Parameters => new double[] { Surface.Surface.knotArray0[KnotIndices[0]], Surface.Surface.knotArray1[KnotIndices[1]] };
        public double[] Coords => Surface.Surface.Evaluate(Parameters[0], Parameters[1]);

        public SelectableKnot(GraphicNurbsSurface surface, int i, int j)
        {
            Surface = surface;
            KnotIndices = new int[] { i, j };
        }
    }

    public enum SurfaceDrawingStyle
    {
        PhongShadedColour,
        HeatmapColour
    }
}
