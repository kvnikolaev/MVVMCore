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

            LoadStartTabPages();
        }

        private void LoadStartTabPages()
        {
            DefaultPageVM defaultPage = new DefaultPageVM();
            TabPager.AddTabPage(defaultPage, GetDefaultPageId());
            TabPager.AddTabPage(new DefaultPageVM(false), GetDefaultPageId());
        }

        private static int _defaultPageCounter = -1;
        private string GetDefaultPageId()
        {
            _defaultPageCounter++;
            return "default" + _defaultPageCounter;
        }

        #region Menu
        private RelayCommand _openStartPageCommand;
        public RelayCommand OpenStartPageCommand => _openStartPageCommand ?? (_openStartPageCommand = new RelayCommand(OpenStartPageExecuted, OpenStartPageCanExecuted));
        
        private void OpenStartPageExecuted(object parameter)
        {
            var t = new DefaultPageVM();
            TabPager.AddTabPage(t, GetDefaultPageId());
            TabPager.ActivateTabPage(t);
        }

        private bool OpenStartPageCanExecuted(object parameter)
        {
            return true;
        }

        #region Открытие вкладки Диспетчера вкладок
        private RelayCommand _openTabManagerCommand;
        public RelayCommand OpenTabManagerCommand => _openTabManagerCommand ?? (_openTabManagerCommand = new RelayCommand(OpenTabManagerExecuted, OpenTabManagerCanExecuted));

        private const string _tabDispatcherId = "TabPagerDispatcher";
        private void OpenTabManagerExecuted(object parameter)
        {
            if (!TabPager.ActivateTabPage(_tabDispatcherId))
            {
                TabPager.AddTabPage(new DispatcherVM(), _tabDispatcherId);
                TabPager.ActivateTabPage(_tabDispatcherId);
            }
        }
        private bool OpenTabManagerCanExecuted(object parameter)
        {
            return true;
        }
        #endregion

        #endregion

    }
}
