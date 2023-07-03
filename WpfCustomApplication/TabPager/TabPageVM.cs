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
        public abstract string Title { get; set; }

        public TabPagerVM TabPager { get; set; }

        /// <summary>
        /// Possibility close in UI
        /// </summary>
        public bool CanClose { get; set; }

        /// <summary>
        /// Unique Id for orientation in UI
        /// </summary>
        public abstract string Id { get; }

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(CloseExecuted, (x) => CanClose && CloseCanExecuted(x)));

        protected abstract void CloseExecuted(object parameter);
        protected abstract bool CloseCanExecuted(object parameter);
    }
}
