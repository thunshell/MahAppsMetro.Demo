using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

    public class BaseViewModel: BindingBase
{
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { base.SetProperty(ref isBusy, value); }
        }

        public Dispatcher OnMainDispatcher { get { return Application.Current.Dispatcher; } }
    }
