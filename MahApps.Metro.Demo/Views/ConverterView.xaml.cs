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
    /// ConverterView.xaml 的互動邏輯
    /// </summary>
    public partial class ConverterView : UserControl
    {
        public ConverterView()
        {
            InitializeComponent();
        }

        private void btnToBase64_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbString.Text)) return;

                tbBase64.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(tbString.Text));
            }
            catch { }
        }

        private void btnToString_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbBase64.Text)) return;
                tbString.Text = Encoding.UTF8.GetString(Convert.FromBase64String(tbBase64.Text));
            }
            catch { }
        }
    }
}
