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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// TabControlsView.xaml 的互動邏輯
    /// </summary>
    public partial class TabControlsView : UserControl
    {
        public TabControlsView()
        {
            InitializeComponent();
            Loaded += TabControlsView_Loaded;
        }

        int index = 0;
        bool isLoaded = false;
        private void TabControlsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (isLoaded) return;
            isLoaded = true;
            Task.Factory.StartNew(new Action(() =>
            {
                while (true)
                {
                    int count = 0;
                    Dispatcher.Invoke(new Action(() =>
                    {
                        count = tabControl1.Items.Count;
                        tabControl1.SelectedIndex = index;
                    }));
                    Thread.Sleep(3000);
                    index++;
                    if (index >= count) index = 0;
                }
            }));
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = tabControl1.SelectedIndex;
        }
    }
}
