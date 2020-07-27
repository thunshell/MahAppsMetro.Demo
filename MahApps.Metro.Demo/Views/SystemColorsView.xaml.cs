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
    /// SystemColors.xaml 的互動邏輯
    /// </summary>
    public partial class SystemColorsView : UserControl
    {
        public SystemColorsView()
        {
            InitializeComponent();
            Loaded += SystemColors_Loaded;
        }

        private void SystemColors_Loaded(object sender, RoutedEventArgs e)
        {
            ShowSystemColors();
        }


        public void ShowSystemColors()
        {
            //使用映射
            foreach (System.Reflection.PropertyInfo item in typeof(System.Windows.SystemColors).GetProperties())
            {
                Rectangle rect = null;
                if (item.PropertyType == typeof(SolidColorBrush))
                {
                    rect = new Rectangle() { Width = 200, Height = 50, Fill = (SolidColorBrush)item?.GetValue(null, null) };
                }
                else if (item.PropertyType == typeof(Color))
                {
                    rect = new Rectangle() { Width = 200, Height = 50, Fill = new SolidColorBrush((Color)item?.GetValue(null, null)) };
                }
                if (rect != null)
                {
                    TextBlock text = new TextBlock() { Text = item.Name, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                    StackPanel sp = new StackPanel() { Margin = new Thickness(5) };
                    sp.Children.Add(rect);
                    sp.Children.Add(text);
                    //App.Control
                    this.wrappanel.Children.Add(sp);
                }
            }
        }
    }
}
