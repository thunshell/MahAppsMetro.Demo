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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// ConnectionView.xaml 的互動邏輯
    /// </summary>
    public partial class ConnectionView : UserControl
    {
        public ConnectionView()
        {
            InitializeComponent();
            Loaded += ConnectionView_Loaded;
            MouseMove += UserControl_MouseMove;
            MouseLeave += UserControl_MouseLeave;
            MouseUp += UserControl_MouseUp;
        }

        private void ConnectionView_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        bool IsConnection { get; set; }
        public Polyline ConnectLine { get; set; }
        Point TempPoint { get; set; }
        bool IsLastPointInPoint { get; set; }
        List<Point> Points = new List<Point>();
        List<string> Keys = new List<string>();

        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            Point p = new Point(Canvas.GetLeft(ellipse) + ellipse.Width / 2, Canvas.GetTop(ellipse) + ellipse.Height / 2);
            StartPaint(p);
        }

        private void ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsConnection) return;
            Ellipse ellipse = sender as Ellipse;
            Point p = new Point(Canvas.GetLeft(ellipse) + ellipse.Width / 2, Canvas.GetTop(ellipse) + ellipse.Height / 2);
            IsLastPointInPoint = true;
            Paint(p);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsConnection) return;
            CheckKeys();
            Clear();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsConnection) return;
            Point p = e.GetPosition(canvas);
            Paint(p);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsConnection) return;
            Clear();
        }

        void Init()
        {
            //初始化锁点
            for (int r = 1; r <= 3; r++)
            {
                for (int c = 1; c <= 3; c++)
                {
                    Ellipse e = new Ellipse()
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                        Tag = r.ToString() + c.ToString()
                    };
                    e.MouseLeftButtonDown += ellipse_MouseLeftButtonDown;
                    e.MouseEnter += ellipse_MouseEnter;
                    int unit = 100;
                    Canvas.SetLeft(e, c * unit);
                    Canvas.SetTop(e, r * unit);
                    canvas.Children.Add(e);
                }
                CraeteLine();
            }
        }

        void CraeteLine()
        {
            //初始化线
            ConnectLine = new Polyline()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3.0,
                StrokeEndLineCap = PenLineCap.Flat,
                Points = new PointCollection(Points)
            };
            canvas.Children.Add(ConnectLine);
        }

        void StartPaint(Point p)
        {
            //开始画线
            IsConnection = true;
            Points.Add(p);
            ConnectLine.Points.Add(Points[0]);
            ConnectLine.Opacity = 1;
        }

         void Paint(Point p)
        {
            if (!IsConnection) return;

            //删除最后一个点
            if (TempPoint != new Point())                       //如果零时点在0,0说明点在点上
            {
                Points.Remove(Points.Last());
                ConnectLine.Points.Remove(TempPoint);
            }
            
            if (!IsLastPointInPoint)           //如果最后一个点不再点上则算作临时点
                TempPoint = p;
            else
            {
                TempPoint = new Point();
                IsLastPointInPoint = false;
            }

            //重画最后一个点
            Points.Add(p);
            ConnectLine.Points.Add(p);
        }

        void Clear()
        {
            //清空线和点
            IsConnection = false;
            Keys.Clear();
            Points.Clear();
            ConnectLine.Points.Clear();
        }

        bool CheckKeys()
        {
            bool isConrrect = true;
            if (isConrrect)
            {
                ConnectLine.Stroke = Brushes.Green;
            }
            else
                ConnectLine.Stroke = Brushes.Red;
            return false;
        }
    }
}
