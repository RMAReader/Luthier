using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using SharpDX;
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
    class SceneComponentPhongShaded : SceneComponentBase
    {
      
        public SceneComponentPhongShaded(SharpDevice device, string shaderPath, IApplicationDocumentModel model, Camera camera, LightData lightData) : base(device, model, camera, lightData)
        {
         
            Shader = new SharpShader(device, shaderPath,
               new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
               new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("TANGENT", 0, Format.R32G32B32_Float, 24, 0),
                        new InputElement("BINORMAL", 0, Format.R32G32B32_Float, 36, 0),
                        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 48, 0)
               });

            
        }

        public override void Draw()
        {
            UpdateConstantBuffers();

            Shader.Apply();

            Device.UpdateAllStates();
            Device.SetDefaultBlendState();
            Device.SetDefaultRasterState();

            Device.DeviceContext.VertexShader.SetConstantBuffer(0, _constantBuffer);
            Device.DeviceContext.PixelShader.SetConstantBuffer(0, _constantBuffer);
            
            Mesh?.Draw();
        }


        public override void UpdateMesh()
        {
            var vertices = new List<TangentVertex>();
            var indices = new List<int>();

            foreach (IDrawablePhongSurface obj in _model.Model.VisibleObjects().Where(x => x is IDrawablePhongSurface))
            {
                obj.GetVertexAndIndexLists(ref vertices, ref indices);
            }

            if (Mesh != null) Mesh.Dispose();
            if (vertices.Count > 0)
            {
                Mesh = SharpMesh.Create<TangentVertex>(Device, vertices.ToArray(), indices.ToArray());
            }
        }

    }
}
