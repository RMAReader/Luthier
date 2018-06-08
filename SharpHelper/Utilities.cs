﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHelper
{
    
    /// <summary>
    /// Utilities
    /// </summary>
    public static class SharpUtilities
    {
        /// <summary>
        /// Degree to Radiant
        /// </summary>
        /// <param name="degree">Degree</param>
        /// <returns>Radiant</returns>
        public static float AngleToRad(float degree)
        {
            //3.14 : 180 = ? : degree
            return (degree * (float)Math.PI) / 180.0F;
        }

        /// <summary>
        /// Radiant to Degree
        /// </summary>
        /// <param name="radiant">Radiant</param>
        /// <returns>Degree</returns>
        public static float RadToGrad(float radiant)
        {
            //3.14 : 180 = radiant : ?
            return (radiant * 180.0F) / (float)Math.PI;
        }

        /// <summary>
        /// String to float array
        /// </summary>
        /// <param name="text">String</param>
        /// <returns>Array</returns>
        public static float[] StringToFloat(string text)
        {
            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
            info.NumberDecimalSeparator = ".";
            var parts = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return (from p in parts select float.Parse(p, info)).ToArray();
        }

        /// <summary>
        /// Convert array of float in a string
        /// </summary>
        /// <param name="values">Values</param>
        /// <returns>String</returns>
        public static string FloatToText(params float[] values)
        {
            if (values.Length == 0)
                return "";

            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
            info.NumberDecimalSeparator = ".";
            string temp = values.First().ToString(info);

            for (int i = 1; i < values.Length; i++)
            {
                temp += " " + values[i].ToString(info);
            }
            return temp;
        }

        /// <summary>
        /// Convert array of integer in a string
        /// </summary>
        /// <param name="values">Values</param>
        /// <returns>String</returns>
        public static string IntToText(params int[] values)
        {
            if (values.Length == 0)
                return "";

            System.Globalization.NumberFormatInfo info = new System.Globalization.NumberFormatInfo();
            info.NumberDecimalSeparator = ".";
            string temp = values.First().ToString(info);

            for (int i = 1; i < values.Length; i++)
            {
                temp += " " + values[i].ToString(info);
            }
            return temp;
        }


      
        public static double[] Mul(Matrix m, double[] v)
        {
            return new double[] {
                m.M11 * v[0] + m.M21 * v[1] + m.M31 * v[2] + m.M41,
                m.M12 * v[0] + m.M22 * v[1] + m.M32 * v[2] + m.M42,
                m.M13 * v[0] + m.M23 * v[1] + m.M33 * v[2] + m.M43,
                m.M14 * v[0] + m.M24 * v[1] + m.M34 * v[2] + m.M44};
        }

    }
}
