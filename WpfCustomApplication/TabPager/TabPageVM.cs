using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCustomApplication
{
    internal abstract class TabPageVM : AbstractPageVM
    {
        public TabPageVM(bool canClose = true)
        {
            this.CanClose = canClose;
        }

        public abstract string Title { get; set; }

        public TabPagerVM TabPager { get; set; }

        private bool _canClose = true;
        /// <summary>
        /// Possibility close in UI
        /// </summary>
        public bool CanClose { get => _canClose; set => SetField(ref _canClose, value); }

        /// <summary>
        /// Visual notification in UI
        /// </summary>
        public bool IsAlarm { get; set; } //!!

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(CloseExecuted, (x) => CanClose && CloseCanExecuted(x)));

        protected abstract void CloseExecuted(object parameter); //!! Close behavior
        protected abstract bool CloseCanExecuted(object parameter);

        public enum TabPageClosing //!!
        {
            Closable,
            NotClosable
        }
    }
}
