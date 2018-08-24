﻿using SharpDX;
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
using Luthier.Model.MouseController3D;
using System.Windows.Forms;
using Luthier.Model.KeyController3D;
using Luthier.Model.UIForms;
using SharpDX.Direct3D;
using Luthier.Geometry.BSpline;
using Luthier.Model.GraphicObjects;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Luthier.Model.Presenter
{



    public class ViewPort3DPresenter
    {
        private readonly IApplicationDocumentModel model;
        private RenderForm3d form;
        private IMouseController3D mouseController;
        private IKeyController3D keyController;
        private Camera _camera ;
        private Vector3 lightDirection;
        private LightData _lightData;

        public ViewPort3DPresenter(IApplicationDocumentModel model)
        {
            this.model = model;

            _camera = new Camera();

            InitialiseViewPort();

            //set camera position and target
            Vector3 from = new Vector3(0, 0, -1000);
            Vector3 to = new Vector3(0, 0, 0);
            _camera.InitialView = Matrix.LookAtLH(from, to, Vector3.UnitY);
            _camera.World = Matrix.Identity;
            _camera.ProjectionMethod = EnumProjectionMethod.Orthonormal;
            
            //light direction
            lightDirection = new Vector3(-0.25f, 0, 1);
            lightDirection.Normalize();
        }


        public void InitialiseViewPort()
        {
            form = new RenderForm3d();
            form.Text = "3D Viewport";

            SetMouseController(new ControlPointDraggerBase());
            SetKeyController(new OrbitZoom());

            form.DoCurveToolStripItem_Click = DoCurveToolStripItem;
            form.DoPlaneToolStripMenuItem_Click = DoPlaneToolStripMenuItem;
            form.DoSurfaceToolStripMenuItem_Click = DoSurfaceToolStripMenuItem;
            form.DoOrthonormalToolStripMenuItem_Click = DoOrthonormalToolStripMenuItem_Click;
            form.DoPerspectiveToolStripMenuItem_Click = DoPerspectiveToolStripMenuItem_Click;
            form.DoDragParallelToXYPlaneToolStripMenuItem_Click = DoDragParallelToXYPlaneToolStripMenuItem_Click;
            form.DoDragParallelToYZPlaneToolStripMenuItem_Click = DoDragParallelToYZPlaneToolStripMenuItem_Click;
            form.DoDragParallelToZXPlaneToolStripMenuItem_Click = DoDragParallelToZXPlaneToolStripMenuItem_Click;
            form.DoDragNormalToPlaneToolStripMenuItem_Click = DoDragNormalToPlaneToolStripMenuItem_Click;
            form.DoLightingOptionsToolStripMenuItem_Click = DoLightingOptionsToolStripMenuItem_Click;

            _camera.ViewWidth = form.ClientSize.Width;
            _camera.ViewHeight = form.ClientSize.Height;

            _lightData = new LightData()
            {
                SurfaceColor = new Vector3(1.0f, 1.0f, 1.0f),//Color.AntiqueWhite.ToVector3(),
                LampColor = Color.Red.ToVector3(),
                AmbiColor = new Vector3(0.2f,0.2f,0.2f),
                Kd = 1.0f,
                Kr = 0.5f,
                SpecExpon = 1.0f
            };
        }


        public void SetMouseController(IMouseController3D controller)
        {
            if (mouseController != null)
            {
                mouseController.Close();
                form.MouseClick -= mouseController.MouseClick;
                form.MouseDoubleClick -= mouseController.MouseDoubleClick;
                form.MouseDown -= mouseController.MouseDown;
                form.MouseMove -= mouseController.MouseMove;
                form.MouseUp -= mouseController.MouseUp;
                form.MouseWheel -= mouseController.MouseWheel;
            }
            mouseController = controller;

            mouseController.Bind(model);
            mouseController.Bind(_camera);

            form.MouseClick += mouseController.MouseClick;
            form.MouseDoubleClick += mouseController.MouseDoubleClick;
            form.MouseDown += mouseController.MouseDown;
            form.MouseMove += mouseController.MouseMove;
            form.MouseUp += mouseController.MouseUp;
            form.MouseWheel += mouseController.MouseWheel;
        }

        public void SetKeyController(IKeyController3D controller)
        {
            if (keyController != null) keyController.Close();
            keyController = controller;

            keyController.Bind(model);
            keyController.Bind(_camera);

            form.KeyDown += keyController.KeyDown;
            form.KeyUp += keyController.KeyUp;
            form.KeyPress += keyController.KeyPress;
            
        }


        private void DoCurveToolStripItem(object sender, EventArgs e)
        {
            SetMouseController(new SketchNurbsCurve());
        }

        private void DoDragParallelToXYPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var controller = new ControlPointDraggerParallelToPlane();
            controller.referencePlane = GraphicObjects.Plane.CreateRightHandedXY(origin: new double[3]);
            SetMouseController(controller);
        }

        private void DoDragParallelToYZPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var controller = new ControlPointDraggerParallelToPlane();
            controller.referencePlane = GraphicObjects.Plane.CreateRightHandedYZ(origin: new double[3]);
            SetMouseController(controller);
        }

        private void DoDragParallelToZXPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var controller = new ControlPointDraggerParallelToPlane();
            controller.referencePlane = GraphicObjects.Plane.CreateRightHandedZX(origin: new double[3]);
            SetMouseController(controller);
        }

        private void DoDragNormalToPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMouseController(new ControlPointDraggerNormalToPlane());
        }

        private void DoSurfaceToolStripMenuItem(object sender, EventArgs e)
        {
            var dialog = new NewBSplineSurface();
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.ShowDialog();
            if(dialog.DialogResult == DialogResult.OK)
            {
                var controller = new SketchSurface(dialog.NumberOfControlPointsU, dialog.NumberOfControlPointsV);
                SetMouseController(controller);
            }
        }


        private void DoPlaneToolStripMenuItem(object sender, EventArgs e)
        {
            var plane = GraphicObjects.Plane.CreateRightHandedXY(new double[] { 0, 0, -1000});
            model.Model.Add(plane);
        }



        private void DoPerspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _camera.ProjectionMethod = EnumProjectionMethod.Perspective;
        }

        private void DoOrthonormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _camera.ProjectionMethod = EnumProjectionMethod.Orthonormal;
        }


        private LightingOptions _lightingOptionsForm;
        private void DoLightingOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_lightingOptionsForm == null || _lightingOptionsForm.IsDisposed)
            {
                _lightingOptionsForm = new LightingOptions(_lightData);
                _lightingOptionsForm.Show();
            }
            else
            {
                _lightingOptionsForm.Focus();
            }
            
        }



        public void ShowRenderForm()
        {
            if (!SharpDevice.IsDirectX11Supported())
            {
                System.Windows.Forms.MessageBox.Show("DirectX11 Not Supported");
                return;
            }


            //Help to count Frame Per Seconds
            SharpFPS fpsCounter = new SharpFPS();


            var indices = new List<int>();
            var vertices = new List<Vector3d>();
            var normals = new List<Vector3d>();

            model.CreateMesh_NurbsSurface(vertices, normals, indices);
           
            using (SharpDevice device = new SharpDevice(form))
            {
                //device.SetWireframeRasterState();

                SharpMesh mesh_NurbsSurface = null;
                SharpMesh mesh_NurbsControl = null;
                SharpMesh mesh_NurbsCurve = null;

                SharpMesh mesh_XYPlane = CreatePlaneXY(device);

                //Create Shader From File and Create Input Layout
                SharpShader shader = new SharpShader(device, "../../HLSL.fx",
                    new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
                    new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0)
                    });

                SharpShader shaderLines = new SharpShader(device, "../../HLSL_Lines.fx",
                    new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
                    new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0)
                    });

                SharpShader shaderMetal = new SharpShader(device, "../../HLSL_AmbientDiffuseSpecular.fx",
                new SharpShaderDescription() { VertexShaderFunction = "VS", PixelShaderFunction = "PS" },
                new InputElement[] {
                        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                        new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                        new InputElement("TANGENT", 0, Format.R32G32B32_Float, 24, 0),
                        new InputElement("BINORMAL", 0, Format.R32G32B32_Float, 36, 0),
                        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 48, 0)
                });

                //create constant buffer
                //SharpDX.Direct3D11.Buffer bufferLines = shaderLines.CreateBuffer<Data>();

                SharpDX.Direct3D11.Buffer buffer = shaderMetal.CreateBuffer<Data2>();
               

                fpsCounter.Reset();

                    model.Model.HasChanged = true;

                    //main loop
                    RenderLoop.Run(form, () =>
                    {
                        //Resizing
                        if (device.MustResize)
                        {
                            device.Resize();
                            _camera.ViewWidth = form.ClientSize.Width;
                            _camera.ViewHeight = form.ClientSize.Height;
                        }

                        if (model.Model.HasChanged)
                        {

                            var sw = new Stopwatch();
                            sw.Restart();

                            model.CreateMesh_NurbsSurface(vertices, normals, indices);
                            model.Model.HasChanged = false;
                            
                            var t1 = sw.Elapsed.TotalSeconds * 1000;
                            sw.Restart();

                            //Init Mesh
                            var Sharpvertices = new TangentVertex[vertices.Count];
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                Sharpvertices[i].Position = new Vector3((float)vertices[i].x, (float)vertices[i].y, (float)vertices[i].z);
                                Sharpvertices[i].Normal = new Vector3((float)normals[i].x, (float)normals[i].y, (float)normals[i].z);
                                Sharpvertices[i].Tangent = new Vector3(1, 0, 0);
                                Sharpvertices[i].Binormal = new Vector3(0, 1, 0);
                                Sharpvertices[i].TextureCoordinate = new Vector2(0, 0);
                            }

                            if (Sharpvertices.Length > 0)
                            {
                                if (mesh_NurbsSurface == null)
                                {
                                    mesh_NurbsSurface = SharpMesh.Create<TangentVertex>(device, Sharpvertices, indices.ToArray());
                                }
                                else
                                {
                                    device.DeviceContext.UpdateSubresource(Sharpvertices, mesh_NurbsSurface.VertexBuffer);
                                }
                            }
                            var t2 = sw.Elapsed.TotalSeconds * 1000;
                            model.CreateMesh_NurbsControl(vertices, normals, indices);

                            //Init Mesh
                            var Sharpvertices2 = new StaticColouredVertex[vertices.Count];
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                Sharpvertices2[i].Position = new Vector3((float)vertices[i].x, (float)vertices[i].y, (float)vertices[i].z);
                                Sharpvertices2[i].Normal = new Vector3((float)normals[i].x, (float)normals[i].y, (float)normals[i].z);
                                Sharpvertices2[i].Color = new Vector4(0,1,1,0);
                            }

                            if (Sharpvertices2.Length > 0)
                            {
                                if (mesh_NurbsControl == null)
                                {
                                    mesh_NurbsControl = SharpMesh.Create<StaticColouredVertex>(device, Sharpvertices2, indices.ToArray());
                                }
                                else
                                {
                                    device.DeviceContext.UpdateSubresource(Sharpvertices2, mesh_NurbsControl.VertexBuffer);
                                }
                            }


                            model.CreateMesh_NurbsCurve(vertices, normals, indices);

                            //Init Mesh
                            Sharpvertices = new TangentVertex[vertices.Count];
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                Sharpvertices[i].Position = new Vector3((float)vertices[i].x, (float)vertices[i].y, (float)vertices[i].z);
                                Sharpvertices[i].Normal = new Vector3((float)normals[i].x, (float)normals[i].y, (float)normals[i].z);
                            }
                            if (Sharpvertices.Length > 0)
                            {
                                if (mesh_NurbsCurve != null) mesh_NurbsCurve.Dispose();
                                
                                mesh_NurbsCurve = SharpMesh.Create<TangentVertex>(device, Sharpvertices, indices.ToArray());
                                
                            }
                        }


                        //apply states
                        device.UpdateAllStates();

                        //clear color
                        device.Clear(SharpDX.Color.CornflowerBlue);
                        Data sceneInformation = new Data()
                        {
                            world = _camera.World,
                            worldViewProjection = _camera.WorldViewProjection,
                            lightDirection = new Vector4(lightDirection, 1),
                            //viewDirection = new Vector4(Vector3.Normalize(from - to), 1),
                        };

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
                            SurfaceColor = _lightData.SurfaceColor,//new Vector3(0.6f, 0.6f, 0.6f),//Color.AntiqueWhite.ToVector3(),
                            LampColor = _lightData.LampColor,// Color.Red.ToVector3(),
                            AmbiColor = _lightData.AmbiColor,// new Vector3(0.6f,0.6f,0.6f),
                            Kd = _lightData.Kd,//1.0f,
                            Kr = _lightData.Kr,//0.5f,
                            SpecExpon = _lightData.SpecExpon //1f
                        };


                        //apply shader
                        shaderMetal.Apply();

                        //update constant buffer
                        device.UpdateData<Data2>(buffer, sceneInformation2);

                        ////set shaders to use
                        //device.DeviceContext.VertexShader.Set(shaderMetal.VertexShader);
                        //device.DeviceContext.PixelShader.Set(shaderMetal.PixelShader);

                        //pass constant buffer to shader
                        device.DeviceContext.VertexShader.SetConstantBuffer(0, buffer);
                        device.DeviceContext.PixelShader.SetConstantBuffer(0, buffer);

                        //draw mesh
                        device.SetDefaultRasterState();
                        if (mesh_NurbsSurface != null) mesh_NurbsSurface.Draw();


                        shaderLines.Apply();
                        //device.UpdateData<Data>(bufferLines, sceneInformation);

                        device.SetWireframeRasterState();
                        //device.DeviceContext.VertexShader.Set(shaderLines.VertexShader);
                        //device.DeviceContext.PixelShader.Set(shaderLines.PixelShader);
                        //device.DeviceContext.VertexShader.SetConstantBuffer(0, bufferLines);
                        //device.DeviceContext.PixelShader.SetConstantBuffer(0, bufferLines);

                        if (mesh_NurbsControl != null) mesh_NurbsControl.DrawPatch(PrimitiveTopology.LineList);
                        if (mesh_NurbsCurve != null) mesh_NurbsCurve.DrawPatch(PrimitiveTopology.LineList);

                        device.SetDefaultRasterState();
                        mesh_XYPlane.Draw();

                        //begin drawing text
                        device.Font.Begin();

                        //draw string
                        fpsCounter.Update();
                        device.Font.DrawString("FPS: " + fpsCounter.FPS, 0, 30);
                        device.Font.DrawString($"X = {mouseController.X}, Y = {mouseController.Y}", 0, 45);

                        //flush text to view
                        device.Font.End();
                        //present
                        device.Present();
                    });

                //release resources
                if (mesh_NurbsSurface != null) mesh_NurbsSurface.Dispose();
                if (mesh_NurbsControl != null) mesh_NurbsControl.Dispose();
                if (mesh_NurbsCurve != null) mesh_NurbsCurve.Dispose();
                mesh_XYPlane.Dispose();
                buffer.Dispose();
                
            }
          
        }


        


        public SharpMesh CreatePlaneXY(SharpDevice device)
        {
            var topColor = new Vector4(0, 0.5f, 0.5f, 0);
            var topNormal = new Vector3(0, 0, 1);
            var undersideColor = new Vector4(0.5f, 0.5f, 0.5f, 0);
            var undersideNormal = new Vector3(0, 0, -1);

            var vertices = new StaticColouredVertex[]
            {
                new StaticColouredVertex{ Position = new Vector3(-1000, -1000, -100), Normal =  topNormal,Color = topColor},
                new StaticColouredVertex{ Position = new Vector3(1000, -1000, -100), Normal =  topNormal,Color = topColor},
                new StaticColouredVertex{ Position = new Vector3(1000, 1000, -100), Normal =  topNormal,Color = topColor},
                new StaticColouredVertex{ Position = new Vector3(-1000, 1000, -100), Normal =  topNormal,Color = topColor},

                new StaticColouredVertex{ Position = new Vector3(-1000, -1000, -100), Normal = undersideNormal,Color = undersideColor},
                new StaticColouredVertex{ Position = new Vector3(1000, -1000, -100), Normal = undersideNormal,Color = undersideColor},
                new StaticColouredVertex{ Position = new Vector3(1000, 1000, -100), Normal = undersideNormal,Color = undersideColor},
                new StaticColouredVertex{ Position = new Vector3(-1000, 1000, -100), Normal = undersideNormal,Color = undersideColor},
            };
            var indices = new int[]{0,2,1,2,0,3,4,5,6,6,7,4};

            return SharpMesh.Create<StaticColouredVertex>(device, vertices, indices);
        }

    }

    struct Data
    {
        public Matrix world;
        public Matrix worldViewProjection;
        public Vector4 lightDirection;
        //public Vector4 viewDirection;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Data2
    {
        public Matrix World;
        public Matrix WorldInverseTranspose;
        public Matrix WorldView;
        public Matrix WorldViewProj;
        public Vector4 LightDirection;

        public Vector3 SurfaceColor;
        public float Kd;
        public Vector3 LampColor;
        public float SpecExpon;
        public Vector3 AmbiColor;
        public float Kr;
        ////samplerCUBE EnvSampler;
        
    }

    public class LightData
    {
        public Vector3 SurfaceColor;
        public float Kd;
        public Vector3 LampColor;
        public float SpecExpon;
        public Vector3 AmbiColor;
        public float Kr;
    }

}
