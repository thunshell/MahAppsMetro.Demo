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
    /// AnimationView1.xaml 的互動邏輯
    /// </summary>
    public partial class AnimationView1 : UserControl
    {
        double Radian = Math.PI / 180;
        bool hasSetBinding = false;
        int R = 150;

        public AnimationView1()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder xaml = new StringBuilder();
            canvas.Children.Clear();
            storyboard.Children.Clear();
            try
            {
                for (int angle = 0, i = 0; angle < 360; angle += 15)
                {
                    int tempAngle = angle;
                    Line line = new Line() { Stroke = Brushes.Red, StrokeThickness = 3, X2 = R, Y2 = R, RenderTransform = new RotateTransform(), SnapsToDevicePixels = true };
                    
                    switch (i++)
                    {
                        case 1: line.Stroke = Brushes.Orange;break;
                        case 2:line.Stroke = Brushes.Yellow;break;
                        case 3: line.Stroke = Brushes.Green; break;
                        case 4: line.Stroke = Brushes.Aqua; break;
                        case 5: line.Stroke = Brushes.Blue; break;
                        case 6: line.Stroke = Brushes.Purple; i = 0; break;
                        default:
                            break;
                    }

                    if(angle == 0)
                    {
                        line.X1 = R;
                        line.Y1 = 0;
                        line.RenderTransformOrigin = new Point(1,0);
                    }
                    else if(angle == 90)
                    {
                        line.X1 = 2 * R;
                        line.Y1 = R;
                        line.RenderTransformOrigin = new Point(1, 1);
                    }
                    else if (angle == 180)
                    {
                        line.X1 = R;
                        line.Y1 = 2 * R;
                        line.RenderTransformOrigin = new Point(1, 1);
                    }
                    else if (angle == 270)
                    {
                        line.X1 = 0;
                        line.Y1 = R;
                        line.RenderTransformOrigin = new Point(0, 1);
                    }
                    else if(angle > 0 && angle < 90)   //第一象限
                    {
                        line.X1 = R + R * Math.Sin(tempAngle * Radian);
                        line.Y1 = R - R * Math.Cos(tempAngle * Radian);
                        line.RenderTransformOrigin = new Point(1, line.Y1 / R);
                    }
                    else if(angle > 90 && angle < 180)  //第四象限
                    {
                        while (tempAngle > 90)
                            tempAngle = tempAngle - 90;
                        tempAngle = 90 - tempAngle;
                        line.X1 = R + R * Math.Sin(tempAngle * Radian);
                        line.Y1 = R + R * Math.Cos(tempAngle * Radian);
                        line.RenderTransformOrigin = new Point(1, 1);
                    }
                    else if(angle > 180 && angle < 270)      //第三象限
                    {
                        while (tempAngle > 90)
                            tempAngle = tempAngle - 90;
                        line.X1 = R - R * Math.Sin(tempAngle * Radian);
                        line.Y1 = R + R * Math.Cos(tempAngle * Radian);
                        line.RenderTransformOrigin = new Point(line.X1 / R, 1);
                    }
                    else      //第二象限
                    {
                        while (tempAngle > 90)
                            tempAngle = tempAngle - 90;
                        tempAngle = 90 - tempAngle;

                        line.X1 = R - R * Math.Sin(tempAngle * Radian);
                        line.Y1 = R - R * Math.Cos(tempAngle * Radian);
                        line.RenderTransformOrigin = new Point(line.X1 / R, line.Y1 / R);
                    }
                    SetAnimation(line);
                    xaml.Append($"<Line Stroke=\"Red\" StrokeThickness=\"3\" X1=\"{line.X1}\" X2=\"150\" Y1=\"{line.Y1}\" Y2=\"150\" />");
                    canvas.Children.Add(line);
                }
                if (!HasSetAnimation)
                    SetCanvasAnimation();
                if (!hasSetBinding)
                {
                    hasSetBinding = true;
                    tbAngle.SetBinding(TextBlock.TextProperty, new Binding("Angle") { Source = canvas.Children[0].RenderTransform });
                }
                storyboard.Begin();
                //Helper.NotepadHelper.NewNotePad(xaml.ToString());
            }
            catch(Exception ex)
            {

            }
        }

        Storyboard storyboard = new Storyboard();
        void SetAnimation(Line line, double angle = 360, int second = 10)
        {
            //DoubleAnimation doubleAnimation = new DoubleAnimation(angle, TimeSpan.FromSeconds(second));
            ////doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //doubleAnimation.Completed += (s, e) =>
            //{
            //    angle = angle == 0 ? 360 : 0;
            //    second = second == 10 ? 3 : 10;
            //    SetAnimation(line, angle, second);
            //};
            DoubleAnimationUsingKeyFrames doubleAnimation = new DoubleAnimationUsingKeyFrames() { RepeatBehavior = RepeatBehavior.Forever, Duration = TimeSpan.FromSeconds(16), AutoReverse = true };
            doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));
            doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(8))));
            doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(60, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(11))));
            doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(60, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(16))));
            //doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(330, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(9))));
            //doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(360, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));
            //doubleAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(5))));
            line.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
        }

        bool HasSetAnimation = false;
        void SetCanvasAnimation(double angle = 360, int second = 10)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(angle, TimeSpan.FromSeconds(second));
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Completed += (s, e) =>
            {
                angle = angle == 0 ? 360 : 0;
                second = second == 10 ? 3 : 10;
                SetCanvasAnimation(angle, second);
            };
            canvas.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
            HasSetAnimation = true;
        }
    }
}
