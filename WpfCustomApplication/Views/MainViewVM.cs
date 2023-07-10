using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCustomApplication
{
    internal class MainViewVM : AbstractPageVM
    {
        public override object View { get; protected set; }

        private TabPagerVM _tabPager;
        public TabPagerVM TabPager => _tabPager ?? (_tabPager = new TabPagerVM());


        public MainViewVM()
        {
            this.View = new MainView() { DataContext = this };

            LoadTabPages();
        }

        private void LoadTabPages()
        {
            DefaultPageVM defaultPage = new DefaultPageVM();
            TabPager.AddTabPage(defaultPage);
            TabPager.AddTabPage(new DefaultPageVM(false));
        }

        #region Menu
        private RelayCommand _openStartPageCommand;
        public RelayCommand OpenStartPageCommand => _openStartPageCommand ?? (_openStartPageCommand = new RelayCommand(OpenStartPageExecuted, OpenStartPageCanExecuted));
        
        private void OpenStartPageExecuted(object parameter)
        {
            var t = new DefaultPageVM();
            TabPager.AddTabPage(t);
            TabPager.ActivateTabPage(t);
        }

        private bool OpenStartPageCanExecuted(object parameter)
        {
            return true;
        }

        private RelayCommand _openTabManagerCommand;
        public RelayCommand OpenTabManagerCommand => _openTabManagerCommand ?? (_openTabManagerCommand = new RelayCommand(OpenTabManagerExecuted, OpenTabManagerCanExecuted));

        private void OpenTabManagerExecuted(object parameter)
        {
            if (!TabPager.ActivateTabPage(DispatcherVM.TabId))
                TabPager.AddTabPage(new DispatcherVM());

        }
        private bool OpenTabManagerCanExecuted(object parameter)
        {
            return true;
        }
        #endregion

    }
}
