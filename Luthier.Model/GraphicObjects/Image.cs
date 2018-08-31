using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.GraphicObjects
{
    public class Image : GraphicObjectBase
    {
        public double[] UpperLeft;
        public double[] LowerLeft;
        public double[] LowerRight;
        public string FileName;
        public double AspectRatio;
        public bool IsValid => AspectRatio != 0;

        public Image()
        {

        }
        public Image(string fileName)
        {
            FileName = fileName;
            var image = System.Drawing.Image.FromFile(FileName);
            AspectRatio = (double) image.Width / image.Height;
            image.Dispose();
        }

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return double.MaxValue;
        }
    }
}
