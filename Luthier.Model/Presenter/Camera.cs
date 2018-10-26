using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpHelper;

namespace Luthier.Model.Presenter
{

    public enum EnumProjectionMethod
    {
        Perspective,
        Orthonormal
    }

    public class Camera
    {
        public int ViewWidth;
        public int ViewHeight;

        public float ZoomFactor = 1;

        public EnumProjectionMethod ProjectionMethod;

        public Matrix InitialView;
        public Vector3 LookAt;
        public float Distance = 1000;
        public float Theta = (float)-Math.PI / 2;
        public float Phi = 0.01f;

        public Vector3 Position
        {
            get
            {
                return new Vector3(
                    (float)(LookAt.X + Math.Cos(Theta) * Math.Sin(Phi) * Distance * ZoomFactor),
                    (float)(LookAt.Y + Math.Sin(Theta) * Math.Sin(Phi) * Distance * ZoomFactor),
                    (float)(LookAt.Z + Math.Cos(Phi) * Distance * ZoomFactor)
                    );
            }
        }


        public Vector3 Direction
        {
            get
            {
                var wvT = WorldView;
                wvT.Transpose();
                Vector3 direction = Vector3.UnitZ;
                Vector3.Transform(ref direction, ref wvT, out Vector3 directionWV);
                return directionWV;
            }
        }
        public Vector3 CameraUp
        {
            get
            {
                var wvT = WorldView;
                wvT.Transpose();
                Vector3 direction = Vector3.UnitY;
                Vector3.Transform(ref direction, ref wvT, out Vector3 directionWV);
                return directionWV;
            }
        }
        public Vector3 CameraRight
        {
            get
            {
                var wvT = WorldView;
                wvT.Transpose();
                Vector3 direction = Vector3.UnitX;
                Vector3.Transform(ref direction, ref wvT, out Vector3 directionWV);
                return directionWV;
            }
        }


        public Matrix Projection
        {
            get
            {
                switch (ProjectionMethod)
                {
                    case EnumProjectionMethod.Perspective:
                        float ratio = (float)ViewWidth / ViewHeight;
                        return Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 10000);

                    case EnumProjectionMethod.Orthonormal:
                        return Matrix.OrthoLH(ViewWidth * ZoomFactor, ViewHeight * ZoomFactor, 1, 10000);

                    default:
                        return Matrix.Identity;
                }
            }
        }

        public Matrix View => Matrix.LookAtLH(Position, LookAt, Vector3.UnitZ);
            
        public Matrix World;
        public Matrix WorldView => World * View;
        public Matrix WorldViewProjection => World * View * Projection;


        public double[] ConvertFromWorldToScreen(double[] v)
        {
            var p = SharpUtilities.Mul(WorldViewProjection, v);
            return new double[] {
                (1 + p[0] / p[3]) * ViewWidth / 2,
                (1 - p[1] / p[3]) * ViewHeight / 2,
                (p[2] / p[3])
            };
        }

        public void ConvertFromScreenToWorld(float screenX, float screenY, out double[] from, out double[] to)
        {
            var x = screenX * 2 / ViewWidth - 1;
            var y = 1 - screenY * 2 / ViewHeight;

            var m = WorldViewProjection;
            m.Invert();

            var fromH = SharpUtilities.Mul(m, new double[] { x, y, 0});
            var toH = SharpUtilities.Mul(m, new double[] { x, y, 1 });

            from = new double[] { fromH[0] / fromH[3], fromH[1] / fromH[3], fromH[2] / fromH[3] };
            to = new double[] { toH[0] / toH[3], toH[1] / toH[3], toH[2] / toH[3] };
        }

    }

}
