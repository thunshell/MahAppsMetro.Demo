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
    /// Animation2.xaml 的互動邏輯
    /// </summary>
    public partial class Animation2 : UserControl
    {
        public Animation2()
        {
            InitializeComponent();
            Loaded += Animation2_Loaded;
        }

        public new bool IsLoaded { get; set; }
        private void Animation2_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) return;
            IsLoaded = true;
            Play();
            PlayPanel1();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void Play()
        {
            ThicknessAnimationUsingKeyFrames thicknessAnimation = new ThicknessAnimationUsingKeyFrames()
            {
                Duration = TimeSpan.FromSeconds(6),
                RepeatBehavior = RepeatBehavior.Forever,
            };
            thicknessAnimation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, 20, 0, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4))));
            thicknessAnimation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, 20, 0, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(5))));
            thicknessAnimation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(0, 100, 0, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(5.5))));
            this.tbName.BeginAnimation(TextBlock.MarginProperty, thicknessAnimation);
        }

        void PlayPanel1()
        {
            int time = 1;
            for (int i = 0; i < panel1.Children.Count; i++)
            {
                DoubleAnimationUsingKeyFrames thicknessAnimation = new DoubleAnimationUsingKeyFrames()
                {
                    BeginTime = TimeSpan.FromMilliseconds(time * i),
                    Duration = TimeSpan.FromSeconds(time * panel1.Children.Count),
                    RepeatBehavior = RepeatBehavior.Forever,
                    DecelerationRatio = 0.5
                };
                thicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time * i))));
                thicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(20, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time * i + time))));
                panel1.Children[i].BeginAnimation(Canvas.TopProperty, thicknessAnimation);
            }
        }
    }
}
