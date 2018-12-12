using Luthier.Model.GraphicObjects;
using Luthier.Model.Presenter;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Scene3D
{
    class Scene
    {
        
        public SharpDevice Device { get; set; }
        public List<SceneComponentBase> Components { get; set; }

        public Camera Camera;
        public LightData LightData;
        public IApplicationDocumentModel Model;

        public Scene(SharpDevice device, IApplicationDocumentModel model, Camera camera, LightData lightData)
        {
            this.Device = device;
            this.Camera = camera;
            this.LightData = lightData;
            this.Model = model;

            InitialiseComponents();
        }


        public void InitialiseComponents()
        {
            Components = new List<SceneComponentBase>()
            {
                new SceneComponentPhongShaded(Device, "../../HLSL_AmbientDiffuseSpecular.fx", Model, Camera, LightData),
                new SceneComponentTexture(Device, "../../HLSL_Textured_Unshaded.fx", Model, Camera, LightData),
                new SceneComponentLines(Device, "../../HLSL_Lines.fx", Model, Camera, LightData),
                new SceneComponentPoints(Device, "../../HLSL_Lines.fx", Model, Camera, LightData)
            };

        }


        public void Draw()
        {

            Device.UpdateAllStates();
            Device.SetDefaultBlendState();
            Device.Clear(SharpDX.Color.WhiteSmoke);

            foreach (var component in Components)
            {
                component.Draw();
            }
        }


        public void Update()
        {
            foreach (var component in Components)
            {
                component.UpdateMesh();
            }
        }


        
      


    }
}
