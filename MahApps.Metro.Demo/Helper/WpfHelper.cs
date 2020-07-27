using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MahAppsMetro.Demo.Helper
{
    public class WpfHelper
    {
        public static T FirstVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                obj = VisualTreeHelper.GetParent(obj);
                if (obj is T)
                    return obj as T;
            }
            return null;
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return child as T;
                }
                else
                {
                    T childOfChildren = FindVisualChild<T>(child);
                    if (childOfChildren != null)
                    {
                        return childOfChildren;
                    }
                }
            }
            return null;
        }


        public static Bitmap BitmapSourceToBitmap(BitmapSource source)
        {
            using (var stream = new MemoryStream())
            {
                var e = new BmpBitmapEncoder();
                e.Frames.Add(BitmapFrame.Create(source));
                e.Save(stream);

                var bmp = new Bitmap(stream);

                return bmp;
            }
        }

        public static RenderTargetBitmap RenderVisaulToBitmap(Visual vsual, int width, int height)
        {
            var rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            rtb.Render(vsual);

            return rtb;
        }
    }
}
