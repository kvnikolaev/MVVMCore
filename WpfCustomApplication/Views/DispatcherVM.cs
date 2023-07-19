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
    }
}
