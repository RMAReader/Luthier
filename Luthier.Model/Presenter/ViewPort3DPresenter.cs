using SharpDX;
using SharpDX.Windows;
using SharpHelper;
using System;
using Luthier.Model.MouseController3D;
using System.Windows.Forms;
using Luthier.Model.KeyController3D;
using Luthier.Model.UIForms;
using System.Runtime.InteropServices;
using Luthier.Model.Scene3D;
using Luthier.Model.GraphicObjects;
using System.Linq;
using Luthier.Model.ToolPathSpecification;
using System.IO;
using Luthier.Model.GraphicObjects.Events;
using System.Collections.Generic;

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
        private ObjectExplorerForm _objectExplorerForm;

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

            _selectPlaneController = new SelectPlaneController();

            form.DoCurveDegree1ToolStripItem_Click = DoCurveDegree1ToolStripItem;
            form.DoCurveDegree2ToolStripItem_Click = DoCurveDegree2ToolStripItem;
            form.DoPlaneToolStripMenuItem_Click = DoPlaneToolStripMenuItem;
            form.DoSurfaceToolStripMenuItem_Click = DoSurfaceToolStripMenuItem;
            form.DoOrthonormalToolStripMenuItem_Click = DoOrthonormalToolStripMenuItem_Click;
            form.DoPerspectiveToolStripMenuItem_Click = DoPerspectiveToolStripMenuItem_Click;
            form.DoDragParallelToPlaneToolStripMenuItem_Click = DoDragParallelToPlaneToolStripMenuItem_Click;
            form.DoDragNormalToPlaneToolStripMenuItem_Click = DoDragNormalToPlaneToolStripMenuItem_Click;
            form.DoLightingOptionsToolStripMenuItem_Click = DoLightingOptionsToolStripMenuItem_Click;
            form.DoInsertImageToolStripMenuItem_Click = DoInsertImageToolStripMenuItem_Click;
            form.DoSelectCanvasToolStripMenuItem_Click = DoSelectCanvasToolStripMenuItem_Click;
            form.DoObjectExplorerToolStripMenuItem_Click = DoObjectExplorerToolStripMenuItem_Click;
            form.DoPanToolStripMenuItem_Click = DoPanToolStripMenuItem_Click;
            form.DoScaleModelStripMenuItem_Click = DoScaleModelStripMenuItem_Click;
            form.DoSurfaceDrawingStyleToolStripMenuItem_Click = DoSurfaceDrawingStyleToolStripMenuItem_Click;
            form.DoCreateJoiningSurfaceToolStripMenuItem_Click = DoCreateJoiningSurfaceToolStripMenuItem_Click;
            form.DoCreateOffsetCurveToolStripMenuItem_Click = DoCreateOffsetCurveToolStripMenuItem_Click;
            form.DoCreateAdjCurvatureCurveToolStripMenuItem_Click = DoCreateAdjCurvatureCurveToolStripMenuItem_Click;
            form.DoDiscToolStripMenuItem_Click = DoDiscToolStripMenuItem_Click;
            form.DoCompositeCurveToolStripMenuItem_Click = DoCompositeCurveToolStripMenuItem_Click;
            form.DoMouldOutlineToolStripMenuItem_Click = DoMouldOutlineToolStripMenuItem_Click;
            form.DoRecalculateAllToolStripMenuItem_Click = DoRecalculateAllToolStripMenuItem_Click;
            form.DoExportGcodeToolStripMenuItem_Click = DoExportAllGcodeToolStripMenuItem_Click;
            

            _camera.ViewWidth = form.ClientSize.Width;
            _camera.ViewHeight = form.ClientSize.Height;

            _lightData = new LightData()
            {
                AmbientColor = Color.White.ToVector3(),
                DiffuseColor = Color.Red.ToVector3(),
                SpecularColor = Color.White.ToVector3(),
                AmbientCoefficient = 0.2f,
                DiffuseCoefficient = 0.9f,
                SpecularCoefficient = 0.9f,
                ShininessCoefficient = 300
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

        private void DoCurveDegree1ToolStripItem(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new SketchNurbsCurve(degree: 1);
                controller.Canvas = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set canvas before sketching curve.");
            }

        }

        private void DoCurveDegree2ToolStripItem(object sender, EventArgs e)
        {
            if(_selectPlaneController.Plane != null)
            {
                var controller = new SketchNurbsCurve(degree: 2);
                controller.Canvas = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set canvas before sketching curve.");
            }
            
        }

        private void DoDiscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new SketchDiscController();
                controller.Canvas = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set canvas before sketching curve.");
            }
        }

        public void DoDragParallelToPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new ControlPointDraggerParallelToPlane();
                controller.ReferencePlane = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set reference plane before dragging objects.");
            }
        }

        public void DoDragNormalToPlaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new ControlPointDraggerNormalToPlane();
                controller.ReferencePlane = _selectPlaneController.Plane;
                SetMouseController(controller);
                
            }
            else
            {
                MessageBox.Show("Must set reference plane before dragging objects.");
            }
        }

     
        private void DoSurfaceToolStripMenuItem(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var dialog = new NewBSplineSurface();
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ShowDialog();
                if(dialog.DialogResult == DialogResult.OK)
                {
                    var controller = new SketchSurface(dialog.NumberOfControlPointsU, dialog.NumberOfControlPointsV);
                    controller.Canvas = _selectPlaneController.Plane;
                    SetMouseController(controller);
                }
            }
            else
            {
                MessageBox.Show("Must set reference plane before creating surface.");
            }
        }


        private void DoPlaneToolStripMenuItem(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new InsertPlane();
                controller.Canvas = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set reference plane before creating plane.");
            }
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

        //private NewImageForm _newImageForm;
        private void DoInsertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var controller = new InsertImage();
            controller.Canvas = _selectPlaneController.Plane;
            SetMouseController(controller);
            var _newImageForm = new NewImageForm(this, controller);
            
            _newImageForm.Show();
        }

        private SelectPlaneController _selectPlaneController;
        private void DoSelectCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController == null)
            {
                _selectPlaneController = new SelectPlaneController();
            }
            SetMouseController(_selectPlaneController);
        }


        private void DoObjectExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_objectExplorerForm == null || _objectExplorerForm.IsDisposed)
            {
                _objectExplorerForm = new ObjectExplorerForm(model, this);
            }
            _objectExplorerForm.Show();
        }

        private void DoPanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMouseController(new PanZoomMouseWheelController());
        }

        private void DoScaleModelStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var dialog = new ScaleModelDialog();
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ShowDialog();
                if (dialog.DialogResult == DialogResult.OK)
                {
                    SetMouseController(new CalibrateDistanceController3D(dialog.GetResult()));
                }
            }
            else
            {
                MessageBox.Show("Must set reference plane before calibrating scale of model.");
            }
        }


        private SurfaceDrawingStyle _surfaceDrawingStyle = SurfaceDrawingStyle.PhongShadedColour;
        private void DoSurfaceDrawingStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_surfaceDrawingStyle == SurfaceDrawingStyle.PhongShadedColour)
            {
                _surfaceDrawingStyle = SurfaceDrawingStyle.HeatmapColour;
            }
            else if (_surfaceDrawingStyle == SurfaceDrawingStyle.HeatmapColour)
            {
                _surfaceDrawingStyle = SurfaceDrawingStyle.PhongShadedColour;
            }
            
            foreach (GraphicNurbsSurface s in model.Model.Where(x => x is GraphicNurbsSurface))
            {
                s.SurfaceDrawingStyle = _surfaceDrawingStyle;
            }
            model.Model.HasChanged = true;
        }


        private KnotSelectorController _knotSelectorController;
        private void DoCreateJoiningSurfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_knotSelectorController == null)
            {
                _knotSelectorController = new KnotSelectorController();
            }
            SetMouseController(_knotSelectorController);
        }


        private void DoCreateOffsetCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new CreateOffsetCurveController();
                controller.ReferencePlane = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set reference plane before creating offset curve.");
            }
        }

        private void DoCreateAdjCurvatureCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new CreateAdjustedCurvatureCurveController();
                controller.ReferencePlane = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set reference plane before creating adjusted curvature curve.");
            }
        }

        private void DoCompositeCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var controller = new CreateNurbsCurveCompositeController();
                controller.ReferencePlane = _selectPlaneController.Plane;
                SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set reference plane before creating composite curve.");
            }
        }

        private void DoMouldOutlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectPlaneController.Plane != null)
            {
                var mouldOutline = new MouldEdgeSpecification
                {
                    ReferencePlaneKey = _selectPlaneController.Plane.Key,
                    BoundaryPolygonKey = new List<UniqueKey>(),
                    SafeHeight = 20,
                    TopHeight = 20,
                    BottomHeight = 0,
                    MaximumCutDepth = 5,
                    CutHeights = new List<double> { -4, -8, -12, -16, -20, },
                    SpindleState = CncOperation.EnumSpindleState.OnClockwise,
                    SpindleSpeed = 22000,
                    CuttingHorizontalFeedRate = 500,
                    CuttingVerticalFeedRate = 500,
                    FreeHorizontalFeedRate = 3000,
                    FreeVerticalFeedRate = 1500,
                    Tool = new CncTool.EndMill { Diameter = 5.40 },
                    IsInsideCut = true,
                    IsCutFromUnderneath = true,
                };

                var form = new NewEdges2DToolPathForm(mouldOutline);
                model.Model.Add(mouldOutline);
                form.Show();

                //var controller = new SelectToolPathBoundaryCurvesController();
                //controller.ReferencePlane = _selectPlaneController.Plane;
                //SetMouseController(controller);
            }
            else
            {
                MessageBox.Show("Must set reference plane before creating toolpath.");
            }
        }

        private void DoRecalculateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ToolPathSpecificationBase toolPath in model.Model.Where(x => x is ToolPathSpecificationBase))
            {
                toolPath.Calculate();

                if(toolPath.IsVisible)
                    model.Model.HasChanged = true;
            }
        }

        public void DoRecalculateToolpathToolStripMenuItem_Click(object sender, ExportToolPathsToGCodeEventArgs e)
        {
            foreach (ToolPathSpecificationBase toolPath in e.ToolPaths)
            {
                toolPath.Calculate();

                if (toolPath.IsVisible)
                    model.Model.HasChanged = true;
            }
        }

        private void DoExportAllGcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog chooseFolderDialog = new FolderBrowserDialog())
            {
                chooseFolderDialog.ShowNewFolderButton = true;

                if (chooseFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (ToolPathSpecificationBase toolPath in model.Model.Where(x => x is ToolPathSpecificationBase))
                    {
                        var path = $"{chooseFolderDialog.SelectedPath}\\{toolPath.Name}_{toolPath.Key}.txt";

                        if(path != null)
                            File.WriteAllText(path, toolPath.ToolPath.ToGCode());
                    }
                }
            }
        }

        public void DoExportChosenGcodeToolStripMenuItem_Click(object sender, ExportToolPathsToGCodeEventArgs e)
        {
            using (FolderBrowserDialog chooseFolderDialog = new FolderBrowserDialog())
            {
                chooseFolderDialog.ShowNewFolderButton = true;

                if (chooseFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (ToolPathSpecificationBase toolPath in e.ToolPaths)
                    {
                        var path = $"{chooseFolderDialog.SelectedPath}\\{toolPath.Name}_{toolPath.Key}.txt";

                        if (path != null)
                            File.WriteAllText(path, toolPath.ToolPath.ToGCode());
                    }
                }
            }
        }

        public void DoExtendFrontToolStripMenuItem_Click(object sender, GraphicNurbsCurveEditEventArgs e)
        {
            //e.Curve.Curve.ExtendFrontStraight();
            e.Curve.Curve.ExtendFrontConstantCurvature();
            model.Model.HasChanged = true;
        }

        public void DoExtendBackToolStripMenuItem_Click(object sender, GraphicNurbsCurveEditEventArgs e)
        {
            //e.Curve.Curve.ExtendBackStraight();
            e.Curve.Curve.ExtendBackConstantCurvature();
            model.Model.HasChanged = true;
        }


        public void DoReverseToolStripMenuItem_Click(object sender, GraphicNurbsCurveEditEventArgs e)
        {
            e.Curve.Curve = e.Curve.Curve.Reverse();
            model.Model.HasChanged = true;
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

            using (SharpDevice device = new SharpDevice(form))
            {

                var scene = new Scene(device, model, _camera, _lightData);

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

                    if (_camera.ViewWidth > 10)
                    {

                        if (model.Model.HasChanged)
                        {
                            scene.Update();
                            model.Model.HasChanged = false;
                        }

                        scene.Draw();

                        //begin drawing text
                        device.Font.Begin();

                        //draw string
                        fpsCounter.Update();
                        device.Font.SetFont(Color.Black,"Calibri", 14);
                        device.Font.DrawString("FPS: " + fpsCounter.FPS, 0, 30);
                        device.Font.DrawString($"X = {mouseController.X}, Y = {mouseController.Y}", 0, 45);

                        var offsetController = mouseController as CreateOffsetCurveController;
                        if (offsetController != null && offsetController.OffsetDistance != null)
                        {
                            device.Font.DrawString($"Offset: {offsetController.OffsetDistance:0.0}mm", offsetController.X, offsetController.Y - 15);
                        }
                        
                        var sketchObjectController = mouseController as SketchObjectBase;
                        if(sketchObjectController != null && sketchObjectController.WorldIntersection != null)
                        {
                            var p = sketchObjectController.WorldIntersection;
                            device.Font.DrawString($"World Coordinates: X = {p.X:0.000}, Y = {p.Y:0.000}, Z = {p.Z:0.000}", 0, 60);
                        }

                        //flush text to view
                        device.Font.End();
                        //present
                        device.Present();
                    }
                });

                _lightingOptionsForm?.Close();
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Data2
    {
        public Matrix World;
        public Matrix WorldInverseTranspose;
        public Matrix WorldView;
        public Matrix WorldViewProj;
        public Vector4 LightDirection;

        public Vector3 AmbientColor;
        public float AmbientCoefficient;
        public Vector3 DiffuseColor;
        public float DiffuseCoefficient;
        public Vector3 SpecularColor;
        public float SpecularCoefficient;
        public Vector3 Padding1;
        public float ShininessCoefficient;

    }

    public class LightData
    {
        public Vector3 AmbientColor;
        public float AmbientCoefficient;
        public Vector3 DiffuseColor;
        public float DiffuseCoefficient;
        public Vector3 SpecularColor;
        public float SpecularCoefficient;
        public float ShininessCoefficient;
    }

}
