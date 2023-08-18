using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfCustomApplication
{
    internal abstract class TabPageVM : AbstractPageVM, IDisposable
    {
        public TabPageVM(bool canClose = true)
        {
            this.CanClose = canClose;
        }

        public abstract string Title { get; set; }

        private TabPagerVM _tabPager;
        public TabPagerVM TabPager { get => _tabPager; set { _tabPager = value; TabPagerSetBehavior(); } }

        /// <summary>
        /// Occurs when TabPager property get value
        /// </summary>
        protected virtual void TabPagerSetBehavior()
        {

        }

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

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // dispose managed resources
                    this.TabPager = null;
                }
                // dispose unmanaged resources

                // disposing done
                this.disposed = true;
            }
        }

        ~TabPageVM()
        {
            Dispose(disposing: false);
        }

        public enum TabPageClosing //!!
        {
            Closable,
            NotClosable
        }
    }
}
