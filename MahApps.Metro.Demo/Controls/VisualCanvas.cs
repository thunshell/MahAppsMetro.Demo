using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MahAppsMetro.Demo.Controls
{
    public class VisualCanvas: FrameworkElement
    {
        Brush brush = null;
        Pen pen = null;
        VisualCollection collection = null;

        public VisualCanvas()
        {
            brush = Brushes.White;
            pen = new Pen(Brushes.Black, 1);
            collection = new VisualCollection(this);
        }

        protected override int VisualChildrenCount => collection.Count;

        protected override Visual GetVisualChild(int index)
        {
            return collection[index];
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            CreateRectangle(Brushes.White, null, new Rect(0, 0, sizeInfo.NewSize.Width, sizeInfo.NewSize.Height));
        }

        public void CreateRectangle(Point p)
        {
            CreateRectangle(brush, pen, new Rect(p, new Size(50, 50)));
        }

        public void CreateRectangle(Brush brush, Pen pen,Rect rect)
        {
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext drawing = visual.RenderOpen())
            {
                drawing.DrawRectangle(brush, pen, rect);
            }
            collection.Add(visual);
        }
    }
}
