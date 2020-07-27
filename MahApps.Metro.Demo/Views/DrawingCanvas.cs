using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Views
{
    public class DrawingCanvas:Canvas
    {
        private List<Visual> visuals = new List<Visual>();

        protected override int VisualChildrenCount => visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void RemoveVisual(Visual visual)
        {
            visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public void RemoveVisual(int index)
        {
            RemoveVisual(visuals[index]);
        }

        public bool HadLoaded { get; set; }

        public DrawingCanvas()
        {
            this.Loaded += DrawingCanvas_Loaded;
        }

        private void DrawingCanvas_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (HadLoaded)
                AddVisuals();
            this.HadLoaded = true;
        }


        public bool HadAdded { get; set; }
        private void AddVisuals()
        {
            if (HadAdded) return;
            HadAdded = true;
            DrawingVisual drawing = new DrawingVisual();
            using (DrawingContext context = drawing.RenderOpen())
            {
                Brush brush = Brushes.Black;
                Pen pen = new Pen(Brushes.Gray, 1);
                RadialGradientBrush linearGradient = new RadialGradientBrush();
                linearGradient.Center = new System.Windows.Point(0.5, 0.5);
                linearGradient.GradientStops.Add(new GradientStop(Colors.White, 0));
                linearGradient.GradientStops.Add(new GradientStop(Colors.Black, 1));
                context.DrawEllipse(linearGradient, pen, new System.Windows.Point(300,300), 100, 100);
                context.DrawGeometry(brush, pen, GetTextGeometry("你好", "", 30));
                this.AddVisual(drawing);
            }
        }

        private string GetTextPath(string word, Typeface typeface, int fontSize)
        {
            PathGeometry path = GetTextGeometry(word, typeface, fontSize).GetFlattenedPathGeometry();
            return path.ToString();
        }

        public Geometry GetTextGeometry(string word, Typeface typeface, int fontSize)
        {
            FormattedText text = new FormattedText(word,
                new System.Globalization.CultureInfo("zh-cn"),
                System.Windows.FlowDirection.LeftToRight, typeface, fontSize,
                Brushes.Black);

            Geometry geo = text.BuildGeometry(new System.Windows.Point(0, 0));
            return geo;
        }

        public Geometry GetTextGeometry(string word, string fontfamily, int fontSize)
        {
            Typeface typeface = new Typeface(new FontFamily(fontfamily), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            return GetTextGeometry(word, typeface, fontSize);
        }
    }
}
