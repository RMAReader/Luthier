using Luthier.Geometry.Image;
using Luthier.Geometry.Nurbs;
using Luthier.Geometry.Optimization;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.ObjectiveFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Geometry.Test
{
    [TestClass]
    public class PointCloudTests
    {

        [TestMethod]
        public void BuildPointCloudFromImage()
        {
            string filename = @"C:\Users\Richard\Documents\Development\Luthier\TestData\Messiah Belly Arch B Fitting Curve.bmp";
            var image = new Bitmap(System.Drawing.Image.FromFile(filename));

            var builder = new PointCloudBuilder();

            var cloud = builder.CreateFromImage(image, Color.Black);

        }


        [Ignore()]
        [TestMethod]
        public void BuildPointCloudFromImageAndFit()
        {
            string filename = @"C:\Users\Richard\Documents\Development\Luthier\TestData\Messiah Belly Arch B Fitting Curve.bmp";
            var image = new Bitmap(System.Drawing.Image.FromFile(filename));

            var builder = new PointCloudBuilder();

            var cloud = builder.CreateFromImage(image, Color.Black);

            cloud.GetBounds(out double minX, out double maxX, out double minY, out double maxY);
            double y = 0.5 * (minY + maxY);

            var curve = new NurbsCurve(dimension: 2, isRational: false, order: 3, cvCount: 11);
            int divisor = curve.NumberOfPoints - 1;
            for ( int i = 0; i < curve.NumberOfPoints; i++)
            {
                var x = ((double)i / divisor) * maxX + (1 - (double)i / divisor) * minX;
                curve.SetCV(i, new double[] { x, y });
            }

            var fitter = new NurbsCurveFitterConjugateGradient();
            fitter.IterationCompleteEvent += (object sender, IterationCompleteEventArgs e) =>
            {
                var newImage = (Bitmap)image.Clone();
                DrawFittedCurve(curve, newImage, Color.Magenta);

                newImage.Save($"{filename}_fitted{e.NumberOfIterations}.bmp");
            };

            fitter.Fit(curve, cloud);

            var lowerBound = DenseVector.Build.Dense(curve.cvDataBlock.Length, -5000);
            var upperBound = DenseVector.Build.Dense(curve.cvDataBlock.Length, 5000);

            var objFunc = new NurbsCurveSquaredDistance(curve, cloud, EndConstraint.VariablePositionVariableTangent);
            //var objvalue = ObjectiveFunction.Value(objFunc.Value);
            //var fdgof = new ForwardDifferenceGradientObjectiveFunction(objvalue, lowerBound, upperBound);

            int iteration = 0;

            while (iteration < 20)
            {
                try
                {
                    var initialGuess = new DenseVector(curve.cvDataBlock);
                    //var cgresult = ConjugateGradientMinimizer.Minimum(fdgof, initialGuess, gradientTolerance: 1e-2, maxIterations: 1);
                }
                catch
                {

                }
                iteration++;
               
            }
           
            
        }



        private void DrawFittedCurve(NurbsCurve curve, Bitmap source, Color fittedColor)
        {
            var image = new LockBitmap(source);

            image.LockBits();

            var points = curve.ToLines(source.Width);

            foreach(var p in points)
            {
                int i = (int)p[0];
                int j = source.Height - (int)p[1];

                for(int u = -1; u<=1; u++)
                {
                    for(int v = -1; v<=1; v++)
                    {
                        if (0 <= i + u && i + u < image.Width && 0 <= j + v && j + v< image.Height)
                            image.SetPixel(i + u, j + v, fittedColor);
                    }
                }
                
            }

            for (int p =0; p < curve.NumberOfPoints; p++)
            {
                double[] cp = curve.GetCV(p);

                int i = (int)cp[0];
                int j = source.Height - (int)cp[1];

                for (int u = -5; u <= 5; u++)
                {
                    for (int v = -5; v <= 5; v++)
                    {
                        if (0 <= i + u && i + u < image.Width && 0 <= j + v && j + v < image.Height)
                            image.SetPixel(i + u, j + v, Color.Green);
                    }
                }

            }

            image.UnlockBits();
            
        }

    }
}
