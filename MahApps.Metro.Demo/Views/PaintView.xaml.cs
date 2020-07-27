using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// PaintView.xaml 的互動邏輯
    /// </summary>
    public partial class PaintView : UserControl
    {
        public PaintView()
        {
            InitializeComponent();
            Loaded += PaintView_Loaded;
        }

        private void PaintView_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.canvas2.Children.Clear();
            double x = this.canvas2.ActualWidth;
            double y = this.canvas2.ActualHeight;
            Random r = new Random();
            List<Point> points = new List<Point>();
            for (int i = 1; i <= 10; i++)
            {
                Point p = new Point(i * 100, r.Next(100, 300));
                points.Add(p);
                AddPoint(p);
            }
            //PaintLine(points);
            UpdateRoad(points);
        }

        Path path2 = null;
        private void PaintLine(List<Point> points)
        {
            List<Point> controls = GetControlPoints(points);

            StringBuilder data = new StringBuilder("M");
            data.AppendFormat("{0},{1}", points[0].X, points[0].Y);
            for (int i = 1; i < points.Count - 1; i++)
            {
                //BezierSegment bs = new BezierSegment(controls[i * 2 - 1], controls[i * 2], points[i], true);
                Point pre = controls[i * 2 - 1];
                Point next = controls[i * 2];
                data.AppendFormat(" C {0},{1} {2},{3} {4},{5}", pre.X, pre.Y, next.X, next.Y, points[i].X, points[i].Y);
            }

            path2 = new Path { Stroke = Brushes.DodgerBlue, StrokeThickness = 1, Data = Geometry.Parse(data.ToString()) };
            this.canvas2.Children.Add(path2);
        }

        void AddPoint(Point p, Brush brush = null)
        {
            var e1 = new Ellipse { Width = 10, Height = 10, Fill = brush ?? Brushes.Orange };
            this.canvas2.Children.Add(e1);
            Canvas.SetLeft(e1, p.X - 5);
            Canvas.SetTop(e1, p.Y - 5);
        }

        void AddLine(Point p1, Point p2, Brush brush = null)
        {
            Line path = new Line { Stroke = brush ?? Brushes.Red, StrokeThickness = 1, X1 = p1.X, Y1 = p1.Y, X2 = p2.X, Y2 = p2.Y};
            this.canvas2.Children.Add(path);
            AddPoint(p1);
            AddPoint(p2);
        }

        List<Point> GetControlPoints(List<Point> points)
        {
            List<Point> controls = new List<Point>();
            controls.Add(points[0]);
            controls.Add(new Point());
            for (int i = 1; i < points.Count - 2; i++)
            {
                Point center1 = GetCenterPoint(points[i], points[i+1]);
                Point center2 = GetCenterPoint(points[i + 1], points[i + 2]);
                Point offset = GetCenterPoint(center1, center2);
                Point control1 = GetControlPont(offset, center1);
                controls.Add(control1);
                Point control2 = GetControlPont(offset, center2);
                controls.Add(control2);
            }
            controls.Add(new Point());
            controls.Add(points[points.Count - 1]);
            return controls;
        }

        private Point GetControlPont(Point offset, Point center)
        {
            return new Point(center.X + offset.X, center.Y + offset.Y);
        }

        Point GetCenterPoint(Point pnt1, Point pnt2)
        {
            return new Point(GetCenter(pnt1.X, pnt2.X), GetCenter(pnt1.Y, pnt2.Y));
        }

        double GetCenter(double a, double b)
        {
            return (a + b) / 2;
        }

        Path path;
        public void UpdateRoad(List<Point> list)
        {
            double x = this.canvas2.ActualWidth;
            double y = this.canvas2.ActualHeight;
            PathFigure pf = new PathFigure();

            pf.StartPoint = list[0];
            List<Point> controls = new List<Point>();
            for (int i = 0; i < list.Count; i++)
            {
                controls.AddRange(Control1(list, i));
            }
            for (int i = 1; i < list.Count; i++)
            {
                BezierSegment bs = new BezierSegment(controls[i * 2 - 1], controls[i * 2], list[i], true);
                bs.IsSmoothJoin = true;

                pf.Segments.Add(bs);
            }
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            PathGeometry pg = new PathGeometry(pfc);

            path = new Path();
            path.Stroke = Brushes.Black;
            path.Data = pg;
            canvas2.Children.Add(path);
        }

        public List<Point> Control1(List<Point> list, int n)
        {
            List<Point> point = new List<Point>();
            point.Add(new Point());
            point.Add(new Point());
            if (n == 0)
            {
                point[0] = list[0];
            }
            else
            {
                point[0] = Average(list[n - 1], list[n]);
                AddPoint(point[0], Brushes.DarkGreen);
            }
            if (n == list.Count - 1)
            {
                point[1] = list[list.Count - 1];
            }
            else
            {
                point[1] = Average(list[n], list[n + 1]);
                AddPoint(point[1], Brushes.DarkGreen);
            }
            Point footPoint = Average(point[0], point[1]);
            Point sh = Sub(list[n], footPoint);
            point[0] = Mul(Add(point[0], sh), list[n]);
            point[1] = Mul(Add(point[1], sh), list[n]);
            AddPoint(point[0]);
            AddPoint(point[1]);
            return point;
        }
        public Point Average(Point x, Point y)
        {
            return new Point((x.X + y.X) / 2, (x.Y + y.Y) / 2);
        }
        public Point Add(Point x, Point y)
        {
            return new Point(x.X + y.X, x.Y + y.Y);
        }
        public Point Sub(Point x, Point y)
        {
            return new Point(x.X - y.X, x.Y - y.Y);
        }
        public Point Mul(Point x, Point y, double d = 0.6)
        {
            Point temp = Sub(x, y);
            temp = new Point(temp.X * d, temp.Y * d);
            temp = Add(y, temp);
            return temp;
        }
    }
}
