﻿using System;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using Luthier.Model.Presenter;
using Luthier.Model.GraphicObjects;
using Luthier.Model.GraphicObjects.Interfaces;

namespace Luthier.Model.Scene3D
{
    class SceneComponentStaticColouredSurface : SceneComponentBase
    {
        public SceneComponentStaticColouredSurface(SharpDevice device, string shaderPath, IApplicationDocumentModel model, Camera camera, LightData lightData) : base(device, model, camera, lightData)
        {

            Shader = new SharpShader(device, shaderPath,
                  new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
                  new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0)
                  });

        }


        public override void Draw()
        {
            UpdateConstantBuffers();

            Shader.Apply();

            Device.SetDefaultBlendState();
            Device.SetDefaultRasterState();
            Device.SetDefaultDepthState();
            Device.UpdateAllStates();

            Device.DeviceContext.VertexShader.SetConstantBuffer(0, _constantBuffer);
            Device.DeviceContext.PixelShader.SetConstantBuffer(0, _constantBuffer);

            Mesh?.Draw();
        }

        public override void UpdateMesh()
        {
            var vertices = new List<StaticColouredVertex>();
            var indices = new List<int>();

            foreach (IDrawableStaticColouredSurface obj in _model.Model.VisibleObjects().Where(x => x is IDrawableStaticColouredSurface))
            {
                obj.GetVertexAndIndexLists(ref vertices, ref indices);
            }

            if (Mesh != null) Mesh.Dispose();
            if (vertices.Count > 0)
            {
                Mesh = SharpMesh.Create<StaticColouredVertex>(Device, vertices.ToArray(), indices.ToArray());
            }
        }
    }
}
