using Luthier.Model.Presenter;
using SharpDX;
using SharpDX.Direct3D11;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Scene3D
{
    abstract class SceneComponentBase
    {
        public SharpDevice Device;
        public SharpShader Shader;
        public SharpMesh Mesh;

        protected Camera _camera;
        protected LightData _lightData;
        protected SharpDX.Direct3D11.Buffer _constantBuffer;
        protected IApplicationDocumentModel _model;

        public SceneComponentBase(SharpDevice device, IApplicationDocumentModel model, Camera camera, LightData lightData)
        {
            this.Device = device;
            _camera = camera;
            _lightData = lightData;
            _model = model;

            _constantBuffer = new SharpDX.Direct3D11.Buffer(Device.Device, Utilities.SizeOf<Data2>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        public abstract void Draw();
        public abstract void UpdateMesh();

        public void Dispose()
        {
            Shader?.Dispose();
            Mesh?.Dispose();
        }


        protected void UpdateConstantBuffers()
        {
            var lightDirection = new Vector3(-0.25f, 0, 1);
            lightDirection.Normalize();

            var viewI = _camera.View;
            viewI.Invert();

            var worldIT = _camera.World;
            worldIT.Invert();
            worldIT.Transpose();

            Data2 sceneInformation2 = new Data2()
            {
                World = _camera.World,
                WorldInverseTranspose = worldIT,
                WorldView = _camera.WorldView,
                WorldViewProj = _camera.WorldViewProjection,
                LightDirection = new Vector4(lightDirection, 1),
                AmbientColor = _lightData.AmbientColor,
                DiffuseColor = _lightData.DiffuseColor,
                SpecularColor = _lightData.SpecularColor,
                AmbientCoefficient = _lightData.AmbientCoefficient,
                DiffuseCoefficient = _lightData.DiffuseCoefficient,
                SpecularCoefficient = _lightData.SpecularCoefficient,
                ShininessCoefficient = _lightData.ShininessCoefficient
            };

            Device.UpdateData<Data2>(_constantBuffer, sceneInformation2);
        }

    }
}
