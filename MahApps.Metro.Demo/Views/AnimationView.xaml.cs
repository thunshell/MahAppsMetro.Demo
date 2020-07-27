using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// AnimationView.xaml 的互動邏輯
    /// </summary>
    public partial class AnimationView : UserControl
    {
        public AnimationView()
        {
            InitializeComponent();
        }

        #region Animation1
        bool IsStart1 { get; set; }
        private void btnStop1_Click(object sender, RoutedEventArgs e)
        {
            //(this.TryFindResource("story2") as Storyboard).Stop();
            canvas1.Children.Clear();
            IsStart1 = false;
        }
        
        private void btnStart1_Click(object sender, RoutedEventArgs e)
        {
            if (IsStart1) return;
            IsStart1 = true;
            StartSnowing(canvas1);
        }

        void StartSnowing(Canvas panel)
        {
            Random random = new Random();
            Task.Factory.StartNew(new Action(() =>
            {
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(j * 100);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        int snowCount = random.Next(0, 20);
                        for (int i = 0; i < snowCount; i++)
                        {
                            int width = random.Next(10, 20);
                            PackIconFontAwesome pack = new PackIconFontAwesome()
                            {
                                Kind = PackIconFontAwesomeKind.SnowflakeRegular,
                                Width = width,
                                Height = width,
                                Foreground = Brushes.White,
                                BorderThickness = new Thickness(0),
                                RenderTransform = new RotateTransform(),
                            };
                            int left = random.Next(0, (int)panel.ActualWidth);
                            Canvas.SetLeft(pack, left);
                            panel.Children.Add(pack);
                            int seconds = random.Next(10, 20);
                            DoubleAnimationUsingPath doubleAnimation = new DoubleAnimationUsingPath()
                            {
                                Duration = new Duration(new TimeSpan(0, 0, seconds)),
                                RepeatBehavior = RepeatBehavior.Forever,
                                PathGeometry = new PathGeometry(new List<PathFigure>() { new PathFigure(new Point(left, 0), new List<PathSegment>() { new LineSegment(new Point(left, panel.ActualHeight), false) }, false) }),
                                Source = PathAnimationSource.Y
                            };
                            pack.BeginAnimation(Canvas.TopProperty, doubleAnimation);
                            DoubleAnimation doubleAnimation1 = new DoubleAnimation(360, new Duration(new TimeSpan(0, 0, 10)))
                            {
                                RepeatBehavior = RepeatBehavior.Forever,

                            };
                            pack.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation1);
                        }
                    }));
                }
            }));
        }
        #endregion

        #region 动画2
        List<Model.EllipseInfo> ellipse2 = new List<Model.EllipseInfo>();
        double accelerationY = 0.1;
        int minStartingSpeed = 1;
        int maxStartingSpeed = 50;
        double speedRatio = 0.1;
        int minEllipses = 20;
        int maxEllipses = 100;
        int ellipseRadius = 10;
        bool isRendering = false;

        private void btnStart2_Click(object sender, RoutedEventArgs e)
        {
            if(!isRendering)
            {
                ellipse2.Clear();
                canvas2.Children.Clear();
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                isRendering = true;
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if(ellipse2.Count == 0)
            {
                int halfCanvasWidth = (int)(canvas2.ActualWidth) / 2;
                Random random = new Random();
                int ellipseCount = random.Next(minEllipses, maxEllipses);
                for (int i = 0; i < ellipseCount; i++)
                {
                    Ellipse ellipse = new Ellipse() { Fill = Brushes.LimeGreen, Width = ellipseRadius, Height = ellipseRadius };
                    Canvas.SetLeft(ellipse, halfCanvasWidth + random.Next(-halfCanvasWidth, halfCanvasWidth));
                    Canvas.SetTop(ellipse, 0);
                    canvas2.Children.Add(ellipse);
                    Model.EllipseInfo ellipseInfo = new Model.EllipseInfo(ellipse, speedRatio * random.Next(minStartingSpeed, maxStartingSpeed));
                    ellipse2.Add(ellipseInfo);
                }
            }
            else
            {
                ellipse2.ForEach(ellipse =>
                {
                    double top = Canvas.GetTop(ellipse.Ellipse);
                    Canvas.SetTop(ellipse.Ellipse, top + ellipse.VelocityY);
                    if (top > canvas2.ActualHeight - ellipseRadius * 2)
                    {
                        ellipse2.Remove(ellipse);
                    }
                    else
                    {
                        ellipse.VelocityY += accelerationY;
                    }
                });
                if (ellipse2.Count == 0)
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                    isRendering = false;
                }
            }
        }
        #endregion

        private void btnSnowWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.BombDropperWindow window = new Windows.BombDropperWindow();
            window.Show();
        }
        
        private void btnWaveWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.WaveGridWindow window = new Windows.WaveGridWindow();
            window.Show();
        }

    }
}
