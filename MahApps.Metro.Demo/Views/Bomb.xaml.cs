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
using System.Windows.Threading;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// Bomb.xaml 的互動邏輯
    /// </summary>
    public partial class Bomb : UserControl
    {

        public Bomb()
        {
            InitializeComponent();
        }

        public bool IsFalling { get; set; }
    }
}
