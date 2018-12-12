using Luthier.Geometry;
using Luthier.Model.GraphicObjects.Interfaces;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    class GraphicPointCloud : GraphicObjectBase, IDrawablePoints
    {
        public PointCloud Cloud { get; set; } 


        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }

        #region "IDrawable imlementations"

        public void GetVertexAndIndexLists(ref List<StaticColouredVertex> vertices, ref List<int> indices)
        {
            double[] pos = new double[3];

            for (int i = 0; i < Cloud.PointCount; i++ )
            {
                Cloud.GetPointFast(i, pos);

                indices.Add(vertices.Count);

                vertices.Add(new StaticColouredVertex
                {
                    Position = new SharpDX.Vector3((float)pos[0], (float)pos[1], (float)pos[2]),
                    Color = new SharpDX.Vector4(1, 0, 1, 1)
                });
                
            }
            
        }

#endregion

    }
}
