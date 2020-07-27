using MahAppsMetro.Demo.Views;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MahAppsMetro.Demo.Windows
{
    /// <summary>
    /// BombDropperWindow.xaml 的互動邏輯
    /// </summary>
    public partial class BombDropperWindow : Window
    {
        DispatcherTimer bombTimer = new DispatcherTimer();
        Dictionary<Bomb, Storyboard> storyboards = new  Dictionary<Bomb, Storyboard>();

        public BombDropperWindow()
        {
            InitializeComponent();
            bombTimer.Tick += BombTimer_Tick;
        }

        private void canvasBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(0, 0, canvasBackground.ActualWidth, canvasBackground.ActualHeight);
            canvasBackground.Clip = rect;
        }

        int droppedCount = 0;
        int savedCount = 0;
        double initialSecondsBetweenBombs = 1.3;
        double initialSecondsToFall = 3.5;
        double SecondsBetweenBombs;
        double secondsToFall;

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = false;
            //reset game
            droppedCount = 0;
            savedCount = 0;
            SecondsBetweenBombs = initialSecondsBetweenBombs;
            secondsToFall = initialSecondsToFall;

            //start the bomb-dropping game
            bombTimer.Interval = TimeSpan.FromSeconds(SecondsBetweenBombs);
            bombTimer.Start();
        }

        double secondsBetweenAdjustments = 15;
        DateTime lastAdjustmentTime = DateTime.MinValue;
        double secondsBetweenBombsReduction = 0.1;
        double secondsToFallReduction = 0.1;
        private void BombTimer_Tick(object sender, EventArgs e)
        {
            //Create bomb
            Bomb bomb = new Bomb
            {
                IsFalling = true
            };

            //Position the bomb
            Random random = new Random();
            bomb.SetValue(Canvas.LeftProperty, (double)random.Next(0, (int)canvasBackground.ActualWidth - 50));
            bomb.SetValue(Canvas.TopProperty, -100.0);

            //Add the bomb to Canvas
            canvasBackground.Children.Add(bomb);

            //
            bomb.MouseLeftButtonDown += Bomb_MouseLeftButtonDown;

            //Sotryboard
            Storyboard storyboard = new Storyboard();
            DoubleAnimation fallAnimation = new DoubleAnimation()
            {
                To = canvasBackground.ActualHeight,
                Duration = TimeSpan.FromSeconds(secondsToFall)
            };

            Storyboard.SetTarget(fallAnimation, bomb);
            Storyboard.SetTargetProperty(fallAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(fallAnimation);

            DoubleAnimation wiggleAnimation = new DoubleAnimation()
            {
                To = 30,
                Duration = TimeSpan.FromSeconds(0.2),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };

            Storyboard.SetTarget(wiggleAnimation, ((TransformGroup)bomb.RenderTransform).Children[0]);
            Storyboard.SetTargetProperty(wiggleAnimation, new PropertyPath("Angle"));
            storyboard.Children.Add(wiggleAnimation);
            storyboards.Add(bomb, storyboard);

            storyboard.Duration = fallAnimation.Duration;
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();

            //修改速度
            if((DateTime.Now.Subtract(lastAdjustmentTime).TotalSeconds > secondsBetweenAdjustments))
            {
                lastAdjustmentTime = DateTime.Now;

                SecondsBetweenBombs -= secondsBetweenBombsReduction;
                secondsToFall -= secondsToFallReduction;

                bombTimer.Interval = TimeSpan.FromSeconds(SecondsBetweenBombs);

                //更新界面
                lblRate.Text = String.Format("A bomb is released every {0} seconds.", SecondsBetweenBombs.ToString());
                lblSpeed.Text = String.Format("Each bomb takes {0} seconds to fall.", secondsToFall.ToString());
            }
        }

        int maxDropped = 5;
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            ClockGroup clockGroup = (ClockGroup)sender;

            DoubleAnimation completedAnimation = (DoubleAnimation)clockGroup.Children[0].Timeline;
            Bomb completedBomb = (Bomb)Storyboard.GetTarget(completedAnimation);

            if (completedBomb.IsFalling)
            {
                droppedCount++;
            }
            else
                savedCount++;

            lblStatus.Text = string.Format("You have dropped {0} bombs and saved {1}", droppedCount, savedCount);

            if(droppedCount >= maxDropped)
            {
                bombTimer.Stop();
                lblStatus.Text += "\r\n\r\nGame over.";
                foreach(KeyValuePair<Bomb, Storyboard> item in storyboards)
                {
                    Storyboard storyboard = item.Value;
                    Bomb bomb = item.Key;
                    storyboard.Stop();
                    canvasBackground.Children.Remove(bomb);
                }
                storyboards.Clear();
                cmdStart.IsEnabled = true;
            }
            else
            {
                Storyboard storyboard = (Storyboard)clockGroup.Timeline;
                storyboard.Stop();
                storyboards.Remove(completedBomb);
                canvasBackground.Children.Remove(completedBomb);
            }
        }

        private void Bomb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Bomb bomb = (Bomb)sender;
            bomb.IsFalling = false;

            double currentTop = Canvas.GetTop(bomb);
            Storyboard storyboard = storyboards[bomb];
            storyboard.Stop();

            //将炸弹移出界面
            DoubleAnimation riseAnimation = new DoubleAnimation()
            {
                From = currentTop,
                To = 0,
                Duration = TimeSpan.FromSeconds(2)
            };

            Storyboard.SetTarget(riseAnimation, bomb);
            Storyboard.SetTargetProperty(riseAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(riseAnimation);

            DoubleAnimation slideAnimation = new DoubleAnimation();
            double currentLeft = Canvas.GetLeft(bomb);
            
            slideAnimation.To = currentLeft < canvasBackground.ActualWidth / 2 ? - 100 : canvasBackground.ActualWidth + 100;
            slideAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTarget(slideAnimation, bomb);
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(slideAnimation);

            //start animation
            storyboard.Duration = slideAnimation.Duration;
            storyboard.Begin();
        }
    }
}
