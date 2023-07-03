using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfCustomApplication
{
    internal class TabPagerVM : AbstractPageVM, ITabPager
    {
        public override object View { get; protected set; }

        public TabPagerVM()
        {
            this.View = new TabPager() { DataContext = this };
        }


        #region ITabPager
        public ObservableCollection<TabPageVM> AllTabPages { get; set; } = new ObservableCollection<TabPageVM>();

        public bool ActivateTabPage(TabPageVM page)
        {
            throw new NotImplementedException();
        }

        public bool ActivateTabPage(string pageId)
        {
            throw new NotImplementedException();
        }

        public bool AddTabPage(TabPageVM page)
        {
            this.AllTabPages.Add(page);
            page.TabPager = this;
            return true;
        }

        public bool CloseTabPage(TabPageVM page)
        {
            this.AllTabPages.Remove(page);
            page.TabPager = null;
            return true;
        }

        public bool CloseTabPage(string pageId)
        {
            throw new NotImplementedException();
        }

        #endregion 
    }
}
