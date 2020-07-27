using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// Interaction logic for DataPager.xaml
    /// </summary>
    public partial class DataPager : UserControl
    {
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPager), new PropertyMetadata(1, PageCountChanged));

        private static void PageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int oldValue = Convert.ToInt32(e.OldValue);
            int newValue = Convert.ToInt32(e.NewValue);
            DataPager pager = (DataPager)d;

            if (newValue <= 0) return;
            pager.ResetList();
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(DataPager), new PropertyMetadata(1, OnValueChanged));


        public ICommand NavigateCommand
        {
            get { return (ICommand)GetValue(NavigateCommandProperty); }
            set { SetValue(NavigateCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavigateCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigateCommandProperty =
            DependencyProperty.Register("NavigateCommand", typeof(ICommand), typeof(DataPager));

        public DataPager()
        {
            InitializeComponent();
            this.PART_FirstPage.Click += (s, e) => Value = 1;
            this.PART_LastPage.Click += (s, e) => Value = PageCount;
            this.PART_PreviePage.Click += (s, e) => Value--;
            this.PART_NextPage.Click += (s, e) => Value++;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int oldValue = Convert.ToInt32(e.OldValue);
            int newValue = Convert.ToInt32(e.NewValue);
            DataPager pager = (DataPager)d;

            pager.ResetList();
            pager.NavigateCommand?.Execute(newValue);
        }

        private void ResetList()
        {
            PART_FirstPage.IsEnabled = true;
            PART_PreviePage.IsEnabled = true;
            PART_LastPage.IsEnabled = true;
            PART_NextPage.IsEnabled = true;
            if (Value == 1)
            {
                PART_FirstPage.IsEnabled = false;
                PART_PreviePage.IsEnabled = false;
            }
            if (Value >= PageCount)
            {
                PART_LastPage.IsEnabled = false;
                PART_NextPage.IsEnabled = false;
            }

            List<FrameworkElement> list = new List<FrameworkElement>();
            if(Value > 10)
                list.Add(new TextBlock(new Run("...")) { Margin = new Thickness(5) });

            int startIndex = Value -  10 >= 0 ? Value - 10 + 1 : 1;
            for (int i = startIndex; i <= PageCount && i < startIndex + 10; i++)
            {
                if (Value == i)
                    list.Add(new TextBlock(new Run(i.ToString())) { Margin = new Thickness(5) });
                else
                {
                    Hyperlink link = new Hyperlink(new Run(i.ToString())) { Tag = i };
                    link.Click += (s,e)=> Value = Convert.ToInt32(link.Tag);
                    list.Add(new TextBlock(link) { Margin = new Thickness(5) });
                }
            }

            if (PageCount > 10 && Value < PageCount)
                list.Add(new TextBlock(new Run("...")) { Margin = new Thickness(5) });
            this.PART_PageCodes.ItemsSource = list;
        }
    }
}
