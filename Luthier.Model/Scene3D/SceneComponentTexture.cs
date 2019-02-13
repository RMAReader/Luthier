using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Scene3D
{
    class SceneComponentTexture : SceneComponentBase
    {

        //private ShaderResourceView tex1;

        public Dictionary<string, ShaderResourceView> Textures;
      

        public SceneComponentTexture(SharpDevice device, string shaderPath, IApplicationDocumentModel model, Camera camera, LightData lightData) : base(device, model, camera, lightData)
        {
            Textures = new Dictionary<string, ShaderResourceView>();

            Shader = new SharpShader(device, shaderPath,
               new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
               new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("TANGENT", 0, Format.R32G32B32_Float, 24, 0),
                        new InputElement("BINORMAL", 0, Format.R32G32B32_Float, 36, 0),
                        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 48, 0)
               });

            //tex1 = Device.LoadTextureFromFile(@"C:\Users\Richard\Documents\Development\Luthier\TestData\TitianStradScroll\scroll from back.bmp");
        }


        public override void Draw()
        {
            if (Mesh != null)
            {
                UpdateConstantBuffers();

                Shader.Apply();

                Device.SetDefaultBlendState();
                Device.SetDefaultRasterState();
                Device.SetBlend(BlendOperation.Add, BlendOption.SourceAlpha, BlendOption.InverseSourceAlpha);
                Device.UpdateAllStates();

                Device.DeviceContext.VertexShader.SetConstantBuffer(0, _constantBuffer);
                Device.DeviceContext.PixelShader.SetConstantBuffer(0, _constantBuffer);
                            
                Device.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                Device.DeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(Mesh.VertexBuffer, Mesh.VertexSize, 0));
                Device.DeviceContext.InputAssembler.SetIndexBuffer(Mesh.IndexBuffer, Format.R32_UInt, 0);

                foreach (var subset in Mesh.SubSets)
                {
                    var texture = Textures[subset.TextureName];

                    Device.DeviceContext.PixelShader.SetShaderResource(0, texture);
                    Mesh?.Draw(subset);
                }
            }
      
        }

        public override void UpdateMesh()
        {
            var subSets = new List<SharpSubSet>();
            var vertices = new List<TangentVertex>();
            var indices = new List<int>();
           
            foreach (IDrawableTextured obj in _model.Model.VisibleObjects().Where(x => x is IDrawableTextured))
            {
                if (!Textures.ContainsKey(obj.TextureName))
                {
                    var bitmap = new System.Drawing.Bitmap(obj.Texture);
                    var resourceView = Device.LoadTextureFromBitMap(bitmap);
                    Textures.Add(obj.TextureName, resourceView);
                }

                int startIndex = indices.Count;

                obj.GetVertexAndIndexLists(ref vertices, ref indices);

                int indexCount = indices.Count - startIndex;

                if (indexCount > 0)
                {
                    subSets.Add(new SharpSubSet
                    {
                        StartIndex = startIndex,
                        IndexCount = indexCount,
                        TextureName = obj.TextureName
                    });
                }
            }
                   
            if (Mesh != null) Mesh.Dispose();
            if (vertices.Count > 0)
            {
                Mesh = SharpMesh.Create<TangentVertex>(Device, vertices.ToArray(), indices.ToArray());
                Mesh.SubSets = subSets;
            }
        }
    }
}
