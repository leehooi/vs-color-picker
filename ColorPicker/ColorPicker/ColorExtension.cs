﻿using System;

namespace ColorPicker
{
    public static class ColorExtension
    {
        public static System.Windows.Media.Color ToColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
        }

        public static string ToHexString(this System.Windows.Media.Color color)
        {
            return String.Format("{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
        }

        public static System.Windows.Media.Brush ToBrush(this System.Windows.Media.Color color)
        {
            return new System.Windows.Media.SolidColorBrush(color);
        }

        public static System.Windows.Media.Color ToColor(this string value)
        {
            int color = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber);
            return color.ToColorReverse();
        }

        public static System.Windows.Media.Color ToColor(this int value)
        {
            var r = (byte)(value % 256);
            var g = (byte)((value / 256) % 256);
            var b = (byte)(value / 256 / 256);
            return System.Windows.Media.Color.FromRgb(r, g, b);
        }

        public static System.Windows.Media.Color ToColorReverse(this int value)
        {
            var r = (byte)(value / 256 / 256);
            var g = (byte)((value / 256) % 256);
            var b = (byte)(value % 256);
            return System.Windows.Media.Color.FromRgb(r, g, b);
        }
    }
}
