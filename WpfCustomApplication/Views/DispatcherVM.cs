using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfCustomApplication
{
    internal class DispatcherVM : TabPageVM
    {
        public DispatcherVM()
        {
            this.View = new Dispatcher() { DataContext = this };
            
        }


        #region TabPageVM Implementation

        private string _title = "Диспетчер вкладок";
        public override string Title { get => _title; set => SetField(ref _title, value); }

        public override object View { get; protected set; }

        protected override bool CloseCanExecuted(object parameter)
        {
            return true;
        }

        protected override void CloseExecuted(object parameter)
        {
            TabPager?.CloseTabPage(this);
        }
        #endregion

        #region Menu Commands

        private RelayCommand _openTabCommand;
        public RelayCommand OpenTabCommand => _openTabCommand ?? (_openTabCommand = new RelayCommand(OpenTabExecute, OpenTabCanExecute));

        private void OpenTabExecute(object parameter)
        {
            if (parameter is TabPageVM tab)
            {
                this.TabPager?.ActivateTabPage(tab);
            }
        }

        private bool OpenTabCanExecute(object parameter)
        {
            return true;
        }

        private RelayCommand _closeTabCommand;
        public RelayCommand CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new RelayCommand(CloseTabExecute, CloseTabCanExecute));

        private void CloseTabExecute(object parameter)
        {
            if (parameter is TabPageVM tab)
            {
                this.TabPager?.CloseTabPage(tab);
            }
        }
        
        private bool CloseTabCanExecute(object parameter)
        {
            if (parameter is TabPageVM tab)
            {
                return tab.CanClose /*&& !tab.IsBusy*/;
            }
            return false;
        }
        #endregion
    }
}
