using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Luthier.Geometry.Image;

namespace Luthier.Geometry
{
    public class PointCloud
    {
        private int _dimension;
        private int _pointCount;
        private double[] _data;

        public int PointCount => _pointCount;
        public int Dimension => _dimension;

        public PointCloud(List<double[]> points)
        {
            _dimension = points.First().Length;
            _pointCount = points.Count;
            _data = new double[_dimension * _pointCount];
            for(int i=0; i < _pointCount; i++ )
            {
                Array.Copy(points[i], 0, _data, i * _dimension, _dimension);
            }
        }

        public double[] this[int index]
        {
            get
            {
                double[] result = new double[_dimension];
                Array.Copy(_data, index * _dimension, result, 0, _dimension);
                return result;
            } 
        }

        public void GetPointFast(int index, double[] result)
        {
            Array.Copy(_data, index * _dimension, result, 0, _dimension);
        }

        public void GetBounds(out double minX, out double maxX, out double minY, out double maxY)
        {
            maxX = double.MinValue;
            maxY = double.MinValue;
            minX = double.MaxValue;
            minY = double.MaxValue;

            double[] cp = new double[2];
            for (int i = 0; i < PointCount; i++)
            {
                GetPointFast(i, cp);

                if (maxX < cp[0]) maxX = cp[0];
                if (minX > cp[0]) minX = cp[0];
                if (maxY < cp[1]) maxY = cp[1];
                if (minY > cp[1]) minY = cp[1];
            }
        }

    }



    public class PointCloudBuilder
    {

        public PointCloud CreateFromImage(Bitmap source, Color targetColor)
        {
            var points = new List<double[]>();

            var image = new LockBitmap(source);

            image.LockBits();

            //Iterate whole bitmap to findout the picked color
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    //Get the color at each pixel
                    Color now_color = image.GetPixel(j, i);
                    
                    //Compare Pixel's Color ARGB property with the picked color's ARGB property 
                    //if (now_color.ToArgb() == targetColor.ToArgb())
                    if (now_color.R == targetColor.R && now_color.G == targetColor.G && now_color.B == targetColor.B)
                    {
                        points.Add(new double[] { j, image.Height - i });
                    }
                }
            }

            image.UnlockBits();

            return new PointCloud(points);
        }


    }

}
