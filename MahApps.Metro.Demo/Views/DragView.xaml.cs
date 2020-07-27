using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// GragView.xaml 的互動邏輯
    /// </summary>
    public partial class DragView : UserControl
    {
        List<string> myList = new List<string>() { "1111", "2222", "3333", "4444", "5555" };

        public DragView()
        {
            InitializeComponent();
            this.thumb1.DragDelta += Thumb1_DragDelta;
            this.listbox1.ItemsSource = myList;
        }

        private void Thumb1_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Canvas.SetLeft(thumb1, Canvas.GetLeft(thumb1) + e.HorizontalChange);
            Canvas.SetTop(thumb1, Canvas.GetTop(thumb1) + e.VerticalChange);
        }

        private void listbox1_Drop(object sender, DragEventArgs e)
        {
            //var data = e.Data.GetData(listbox1.SelectedItem.GetType());
            var pos = e.GetPosition(listbox1);
            var result = VisualTreeHelper.HitTest(listbox1, pos);
            if (result == null) return;
            //查找目标数据
            var listBoxItem = Helper.WpfHelper.FirstVisualParent<ListBoxItem>(result.VisualHit);
            if (listBoxItem == null) return;
            int index = myList.IndexOf(listBoxItem.Content.ToString());
            int selectIndex = listbox1.SelectedIndex;
            if (selectIndex < 0) return;
            var item = listbox1.SelectedItem;

            myList.Remove(item.ToString());
            myList.Insert(index, item.ToString());
            this.listbox1.ItemsSource = null;
            this.listbox1.ItemsSource = myList;
            //var pos = e.GetPosition(listbox1);
            //var result = VisualTreeHelper.HitTest(listbox1, pos);
            //if (result == null)
            //{
            //    return;
            //}
            ////查找元数据
            //var sourcePerson = e.Data.GetData(typeof(TextBlock)) as TextBlock;
            //if (sourcePerson == null)
            //{
            //    return;
            //}
            ////查找目标数据
            //var listBoxItem = Helper.WpfHelper.FindVisualParent<ListBoxItem>(result.VisualHit);
            //if (listBoxItem == null)
            //{
            //    return;
            //}
            //var targetPerson = listBoxItem.Content as TextBlock;
            //if (ReferenceEquals(targetPerson, sourcePerson))
            //{
            //    return;
            //}
            //listbox1.Items.Remove(sourcePerson);
            //listbox1.Items.Insert(listbox1.Items.IndexOf(targetPerson), sourcePerson);
        }

        private void listbox1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(listbox1);
                HitTestResult result = VisualTreeHelper.HitTest(listbox1, pos);
                if (result == null)
                {
                    return;
                }
                var listBoxItem = Helper.WpfHelper.FirstVisualParent<ListBoxItem>(result.VisualHit);
                if (listBoxItem == null || listBoxItem.Content != listbox1.SelectedItem)
                {
                    return;
                }
                DataObject dataObj = new DataObject(listBoxItem.Content);
                DragDrop.DoDragDrop(listbox1, dataObj, DragDropEffects.Move);
            }

        }
    }
}
