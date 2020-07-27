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
    /// Interaction logic for TreeViews.xaml
    /// </summary>
    public partial class TreeViews : UserControl
    {
        public TreeViews()
        {
            this.DataContext = new ViewModel.MainViwModel();
            InitializeComponent();
        }
    }
}
