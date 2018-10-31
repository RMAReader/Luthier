using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Luthier.Model.Helpers
{
    
    public static class Convert
    {
        public static System.Numerics.Vector2 NumericsVector2(double[] array) => new System.Numerics.Vector2((float)array[0], (float)array[1]);
        public static System.Numerics.Vector3 NumericsVector3(double[] array) => new System.Numerics.Vector3((float)array[0], (float)array[1], (float)array[2]);
        public static System.Numerics.Vector4 NumericsVector4(double[] array) => new System.Numerics.Vector4((float)array[0], (float)array[1], (float)array[2], (float)array[3]);
        public unsafe static System.Numerics.Vector3 NumericsVector3(SharpDX.Vector3 v) => *(System.Numerics.Vector3*) &v;

        public static SharpDX.Vector2 SharpDXVector2(double[] array) => new SharpDX.Vector2((float)array[0], (float)array[1]);
        public static SharpDX.Vector3 SharpDXVector3(double[] array) => new SharpDX.Vector3((float)array[0], (float)array[1], (float)array[2]);
        public static SharpDX.Vector4 SharpDXVector4(double[] array) => new SharpDX.Vector4((float)array[0], (float)array[1], (float)array[2], (float)array[3]);
        public unsafe static SharpDX.Vector3 SharpDXVector3(System.Numerics.Vector3 v) => *(SharpDX.Vector3*)&v;

        public static double[] ToArray(System.Numerics.Vector3 v) => new double[] { v.X, v.Y, v.Z };
        public static double[] ToArray(SharpDX.Vector3 v) => new double[] { v.X, v.Y, v.Z };

    }

   


}
