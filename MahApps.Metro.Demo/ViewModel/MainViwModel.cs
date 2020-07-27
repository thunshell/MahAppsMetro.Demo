using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MahAppsMetro.Demo.ViewModel
{
    public class MainViwModel
    {
        public ObservableCollection<TreeA> Trees { get; set; }

        public MainViwModel()
        {
            Trees = new ObservableCollection<TreeA>();
            for (int i = 0; i < 10; i++)
            {
                Trees.Add(new TreeA {Index = "Item "+i.ToString(),
                    Trees = new ObservableCollection<TreeB>(new List<TreeB>(new TreeB[1] { new TreeB {Index = "Item " + i.ToString(), Topics = new ObservableCollection<Topic>(new List<Topic>(new Topic[1] { new Topic { Title = "TabControl.Header " + i.ToString(), Content = "TabControl.Conent "+i.ToString() } })) } })) });
            }
        }
    }

    public class TreeA
    {
        public string Index { get; set; }
        public ObservableCollection<TreeB> Trees { get; set; }
    }
    public class TreeB
    {
        public string Index { get; set; }
        public ObservableCollection<Topic> Topics { get; set; }
    }
    public class Topic
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
