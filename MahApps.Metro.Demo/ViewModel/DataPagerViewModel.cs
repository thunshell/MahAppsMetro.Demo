using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahAppsMetro.Demo.ViewModel
{
    public class DataPagerViewModel:BindingBase
    {
        private int pageCount;

        public int PageCount
        {
            get { return pageCount; }
            set { base.SetProperty(ref pageCount, value); }
        }

        int row = 100;
        int count = 50000;
        //public ObservableCollection<int> DataSource { get; set; }
        public ObservableCollection<int> CurrentSource { get; set; }

        public SimpleCommand NavigateCommand { get; set; }

        public DataPagerViewModel()
        {
            NavigateCommand = new SimpleCommand(OnNavigate);
            //DataSource = new ObservableCollection<int>();
            CurrentSource = new ObservableCollection<int>();

            //for (int i = 0; i < 50000; i++)
            //{
            //    DataSource.Add(i);
            //}
            PageCount = count / row;
            OnNavigate(1);
        }

        private void OnNavigate(object obj)
        {
            int page = Convert.ToInt32(obj);
            int r = row * page - row + 1;
            CurrentSource.Clear();
            for (int i = r; i < r + row && i < count; i++)
            {
                CurrentSource.Add(i - 1);
            }
        }
    }
}
