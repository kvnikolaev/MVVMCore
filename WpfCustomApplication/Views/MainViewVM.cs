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

    }
}
