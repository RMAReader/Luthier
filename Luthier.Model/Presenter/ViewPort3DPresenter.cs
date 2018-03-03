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

            DMesh3 g3mesh = model.CreateMesh();
            if (g3mesh.TriangleCount == 0)
            {
                form.Show();
            }
            else
            {
                using (SharpDevice device = new SharpDevice(form))
                {
                    //device.SetWireframeRasterState();

                    //Init Mesh
                    var indices = g3mesh.Triangles().SelectMany(x => x.array).ToList();
                    var vertices = g3mesh.Vertices().Select(v => new ColoredVertex(new Vector3((float)v.x, (float)v.y, (float)v.z), new Vector4(0, 1, 0, 0))).ToList();
                    int nvertices = vertices.Count;

                    //create reverse side mesh
                    var indicesRev = g3mesh.Triangles().SelectMany(t => new int[] { t.a, t.c, t.b }).ToArray();
                    var verticesRev = g3mesh.Vertices().Select(v => new ColoredVertex(new Vector3((float)v.x, (float)v.y, (float)v.z), new Vector4(1, 0, 0, 0)));

                    indices.AddRange(indicesRev.Select(x => x + nvertices));
                    vertices.AddRange(verticesRev);

                    SharpMesh mesh = SharpMesh.Create<ColoredVertex>(device, vertices.ToArray(), indices.ToArray());

                    //Create Shader From File and Create Input Layout
                    SharpShader shader = new SharpShader(device, "../../HLSL.txt",
                        new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
                        new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0)
                        });

                    //create constant buffer
                    SharpDX.Direct3D11.Buffer buffer = shader.CreateBuffer<Matrix>();

                    fpsCounter.Reset();

                    //main loop
                    RenderLoop.Run(form, () =>
                    {
                        //Resizing
                        if (device.MustResize)
                            {
                                device.Resize();
                            }

                        //apply states
                        device.UpdateAllStates();

                        //clear color
                        device.Clear(SharpDX.Color.CornflowerBlue);

                        //Set matrices
                        float ratio = (float)form.ClientRectangle.Width / (float)form.ClientRectangle.Height;
                            Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 10000);
                            Matrix view = Matrix.LookAtLH(new Vector3(0, 0, 2000), new Vector3(), Vector3.UnitY);
                            Matrix world = Matrix.RotationY(Environment.TickCount / 10000.0F);
                            Matrix WVP = world * view * projection;

                        //update constant buffer
                        device.UpdateData<Matrix>(buffer, WVP);

                        //pass constant buffer to shader
                        device.DeviceContext.VertexShader.SetConstantBuffer(0, buffer);

                        //apply shader
                        shader.Apply();

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


}
