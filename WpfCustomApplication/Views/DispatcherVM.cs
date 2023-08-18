using MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var t = System.Windows.EventManager.GetRoutedEvents();

        }

        #region Collection keys editor Presentation

        protected override void TabPagerSetBehavior()
        {
            if (TabPager == null) return;
            KeysCollection = new ObservableCollection<KeyPresentator>(TabPager?.AllTabPages.Keys.Select(key => new KeyPresentator(key, this.TabPager) ));
            TabPager.AllTabPages.CollectionChanged += AllTabPages_CollectionChanged;
        }

        private void AllTabPages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (TabPager == null) return; //!! отписывать от события в dispose
            KeysCollection = new ObservableCollection<KeyPresentator>(TabPager?.AllTabPages.Keys.Select(key => new KeyPresentator(key, this.TabPager) ));
            OnPropertyChanged(nameof(KeysCollection));
        }

        private bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                // dispose managed
            }
            // dispose unmanaged
            TabPager.AllTabPages.CollectionChanged -= AllTabPages_CollectionChanged;

            disposed = true;
            base.Dispose(disposing);
        }


        public ObservableCollection<KeyPresentator> KeysCollection { get; set; }

        public class KeyPresentator : CoreVM
        {
            private TabPagerVM tabPager;

            private bool _editionMode;
            public bool EditionMode { get => _editionMode; set => SetField(ref _editionMode, value); }

            public string Key { get; set; }

            private string _changingKey;
            public string ChangingKey { get => _changingKey; set => SetField(ref _changingKey, value); }

            public KeyPresentator(string key, TabPagerVM tabPager)
            {
                this.Key = key;
                this.ChangingKey = key;
                this.tabPager = tabPager;
            }

            public override string ToString()
            {
                return Key;
            }

            private RelayCommand _editModeCommand;
            public RelayCommand EditModeCommand => _editModeCommand ?? (_editModeCommand = new RelayCommand(EditModeExecuted, (parameter) => { return true; }));

            private void EditModeExecuted(object parameter)
            {
                this.EditionMode = true;
            }

            private RelayCommand _cancelEditingCommand;
            public RelayCommand CancelEditingCommand => _cancelEditingCommand ?? (_cancelEditingCommand = new RelayCommand(CancelEditingExecuted, (parameter) => { return true; }));

            private void CancelEditingExecuted(object parameter)
            {
                if (this.EditionMode == false) return;
                this.EditionMode = false;
                this.ChangingKey = Key;
            }

            private RelayCommand _enterEditingCommand;
            public RelayCommand EnterEditingCommand => _enterEditingCommand ?? (_enterEditingCommand = new RelayCommand(EnterEditingExecuted, (parameter) => { return true; }));

            private void EnterEditingExecuted(object parameter)
            {
                this.EditionMode = false;
                if (tabPager.EditTabKey(this.Key, this.ChangingKey))
                {
                    this.Key = this.ChangingKey;
                    OnPropertyChanged(nameof(this.Key));
                }
                else this.ChangingKey = this.Key;
            }
        }

        #endregion

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

        private RelayCommand _blockClosingCommand;
        public RelayCommand BlockClosingCommand => _blockClosingCommand ?? (_blockClosingCommand = new RelayCommand(BlockClosingExecute, BlockClosingCanExecute));

        private void BlockClosingExecute(object parameter)
        {
            if (parameter is TabPageVM tab)
            {
                tab.CanClose = !tab.CanClose;
            }
        }

        private bool BlockClosingCanExecute(object parameter)
        {
            return true;
        }
        #endregion

        
    }
}
