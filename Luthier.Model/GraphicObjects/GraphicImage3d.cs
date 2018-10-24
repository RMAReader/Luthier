using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luthier.Model.Extensions;
using SharpDX;
using SharpHelper;

namespace Luthier.Model.GraphicObjects
{
    public class GraphicImage3d : GraphicObjectBase, IDrawableTextured
    {
        public double[] UpperLeft;
        public double[] LowerLeft;
        public double[] LowerRight;
        public string FileName;
        
        private System.Drawing.Image _image;
        public System.Drawing.Image Image
        {
            get
            {
                if(_image == null) _image = System.Drawing.Image.FromFile(FileName);
                return _image;
            }
        }

        public double AspectRatio => (double)Image.Width / Image.Height;

        public bool IsValid => _image != null;

        public GraphicImage3d(string fileName)
        {
            FileName = fileName;
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }


        #region "IDrawableTextured implementation"

        public Image Texture => Image;

        public string TextureName => FileName;

        public void GetVertexAndIndexLists(ref List<TangentVertex> vertices, ref List<int> indices)
        {
            
            var indexOffset = vertices.Count;
            var upperRight = UpperLeft.Add(LowerRight).Subtract(LowerLeft);
            var normal = (LowerRight.Subtract(LowerLeft)).VectorProduct(UpperLeft.Subtract(LowerLeft));

            var sw = new Vector3((float)LowerLeft[0], (float)LowerLeft[1], (float)LowerLeft[2]);
            var nw = new Vector3((float)UpperLeft[0], (float)UpperLeft[1], (float)UpperLeft[2]);
            var ne = new Vector3((float)upperRight[0], (float)upperRight[1], (float)upperRight[2]);
            var se = new Vector3((float)LowerRight[0], (float)LowerRight[1], (float)LowerRight[2]);
            var n = new Vector3((float)normal[0], (float)normal[1], (float)normal[2]);

            vertices.AddRange(new TangentVertex[]
            {
                new TangentVertex{ Position = sw, Normal = n, TextureCoordinate = new Vector2(0, 1)},
                new TangentVertex{ Position = se, Normal = n, TextureCoordinate = new Vector2(1, 1)},
                new TangentVertex{ Position = ne, Normal = n, TextureCoordinate = new Vector2(1, 0)},
                new TangentVertex{ Position = nw, Normal = n, TextureCoordinate = new Vector2(0, 0)},

                new TangentVertex{ Position = sw, Normal = -n, TextureCoordinate = new Vector2(0, 1)},
                new TangentVertex{ Position = se, Normal = -n, TextureCoordinate = new Vector2(1, 1)},
                new TangentVertex{ Position = ne, Normal = -n, TextureCoordinate = new Vector2(1, 0)},
                new TangentVertex{ Position = nw, Normal = -n, TextureCoordinate = new Vector2(0, 0)},
            });

            var rawIndices = new int[] { 0, 2, 1, 2, 0, 3, 4, 5, 6, 6, 7, 4 };
            foreach(var index in rawIndices)
            {
                indices.Add(index + indexOffset);
            }

            
        }

        #endregion
    }
}
