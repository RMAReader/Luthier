using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace Luthier.Model.Presenter
{
    public class ViewPort3DPresenter
    {
        private readonly IApplicationDocumentModel model;

        private bool modelChanged;

        public ViewPort3DPresenter(IApplicationDocumentModel model)
        {
            this.model = model;
        }




        public void ShowRenderForm()
        {
            if (!SharpDevice.IsDirectX11Supported())
            {
                System.Windows.Forms.MessageBox.Show("DirectX11 Not Supported");
                return;
            }

            //render form
            RenderForm form = new RenderForm();
            form.Text = "3D Viewport";

            //Help to count Frame Per Seconds
            SharpFPS fpsCounter = new SharpFPS();


            var indices = new List<int>();
            var vertices = new List<Vector3d>();
            var normals = new List<Vector3d>();

            model.CreateMesh(vertices, normals, indices);
            if (vertices.Count == 0)
            {
                form.Show();
            }
            else
            {
                using (SharpDevice device = new SharpDevice(form))
                {
                    //device.SetWireframeRasterState();

                    SharpMesh mesh = null;

                    //Create Shader From File and Create Input Layout
                    SharpShader shader = new SharpShader(device, "../../HLSL.txt",
                        new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
                        new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0)
                        });

                    //create constant buffer
                    SharpDX.Direct3D11.Buffer buffer = shader.CreateBuffer<Data>();

                    fpsCounter.Reset();

                    model.Model.HasChanged = true;

                    //main loop
                    RenderLoop.Run(form, () =>
                    {
                        //Resizing
                        if (device.MustResize)
                        {
                            device.Resize();
                        }

                        if (model.Model.HasChanged)
                        {
                            if (mesh != null) mesh.Dispose();

                            model.CreateMesh(vertices, normals, indices);
                            model.Model.HasChanged = false;
                            
                            //Init Mesh
                            var Sharpvertices = new StaticColouredVertex[vertices.Count];
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                Sharpvertices[i].Position = new Vector3((float)vertices[i].x, (float)vertices[i].y, (float)vertices[i].z);
                                Sharpvertices[i].Normal = new Vector3((float)normals[i].x, (float)normals[i].y, (float)normals[i].z);
                                Sharpvertices[i].Color = new Vector4(1, 0, 0, 0);
                            }
                            mesh = SharpMesh.Create<StaticColouredVertex>(device, Sharpvertices, indices.ToArray());
                        }


                        //apply states
                        device.UpdateAllStates();

                        //clear color
                        device.Clear(SharpDX.Color.CornflowerBlue);

                        //Set matrices
                        float ratio = (float)form.ClientRectangle.Width / (float)form.ClientRectangle.Height;
                        Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 10000);
                        
                        //set camera position and target
                        Vector3 from = new Vector3(0, 0, -1000);
                        Vector3 to = new Vector3(0, 0, 0);
                        Matrix view = Matrix.LookAtLH(from, to, Vector3.UnitY);

                        Matrix world = Matrix.RotationY(Environment.TickCount / 1000.0F);
                        Matrix WVP = world * view * projection;

                        //light direction
                        Vector3 lightDirection = new Vector3(0.25f, 0, 1);
                        lightDirection.Normalize();


                        Data sceneInformation = new Data()
                        {
                            world = world,
                            worldViewProjection = world * view * projection,
                            lightDirection = new Vector4(lightDirection, 1),
                            //viewDirection = new Vector4(Vector3.Normalize(from - to), 1),
                        };


                        //apply shader
                        shader.Apply();

                        //update constant buffer
                        device.UpdateData<Data>(buffer, sceneInformation);

                        //pass constant buffer to shader
                        device.DeviceContext.VertexShader.SetConstantBuffer(0, buffer);
                        device.DeviceContext.PixelShader.SetConstantBuffer(0, buffer);



                        //draw mesh
                        mesh.Draw();

                        //begin drawing text
                        device.Font.Begin();

                        //draw string
                        fpsCounter.Update();
                            device.Font.DrawString("FPS: " + fpsCounter.FPS, 0, 0);

                        //flush text to view
                        device.Font.End();
                        //present
                        device.Present();
                    });

                    //release resources
                    mesh.Dispose();
                    buffer.Dispose();
                }
            }



        }



    }

    struct Data
    {
        public Matrix world;
        public Matrix worldViewProjection;
        public Vector4 lightDirection;
        //public Vector4 viewDirection;
    }
}
