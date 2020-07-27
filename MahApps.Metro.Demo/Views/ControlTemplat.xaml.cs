using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace MahAppsMetro.Demo.Views
{
    /// <summary>
    /// ControlTemplat.xaml 的互動邏輯
    /// </summary>
    public partial class ControlTemplat : UserControl
    {
        public ControlTemplat()
        {
            InitializeComponent();
            Loaded += ControlsTemplateWindow_Loaded;
        }

        private void ControlsTemplateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Type controlType = typeof(Control);
            List<Type> types = new List<Type>();
            Assembly assembly = Assembly.GetAssembly(typeof(Control));
            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(controlType) && !t.IsAbstract && t.IsPublic)
                    types.Add(t);
            }
            assembly = Assembly.GetAssembly(typeof(MetroWindow));
            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(controlType) && !t.IsAbstract && t.IsPublic)
                    types.Add(t);
            }
            types.Sort(new TypeComparer());

            this.lbControls.ItemsSource = types;
        }


        private void lstControls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Type type = (Type)lbControls.SelectedItem;
                ConstructorInfo info = type.GetConstructor(System.Type.EmptyTypes);
                Control control = (Control)info.Invoke(null);
                control.Visibility = Visibility.Collapsed;
                grid.Children.Add(control);
                ControlTemplate template = control.Template;
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb, settings);
                XamlWriter.Save(template, writer);
                
                edit.Document = new ICSharpCode.AvalonEdit.Document.TextDocument(sb.ToString());
                grid.Children.Remove(control);
            }
            catch { }
        }
    }

    internal class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            int res = 0;
            if ((x == null) && (y == null))
            {
                return 0;
            }
            else if ((x != null) && (y == null))
            {
                return 1;
            }
            else if ((x == null) && (y != null))
            {
                return -1;
            }

            res = x.ToString().CompareTo(y.ToString());  //如果x大于y返回-1,如果x小于y返回1,升序abcdABCD
            return res;
        }
    }
}
