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

        private TabPageVM _selectedTab;
        public TabPageVM SelectedTab { get => _selectedTab; set => SetField(ref _selectedTab, value); }

        public bool ActivateTabPage(TabPageVM page)
        {
            var tab = this.AllTabPages.FirstOrDefault(x => x == page);
            if (tab == null) return false;

            this.SelectedTab = tab;
            return true;
        }

        public bool ActivateTabPage(string pageId)
        {
            var tab = this.AllTabPages.FirstOrDefault(x => x.Id == pageId);
            if (tab == null) return false;

            this.SelectedTab = tab;
            return true;
        }

        public bool AddTabPage(TabPageVM page)
        {
            if (this.AllTabPages.Any(x => x.Id == page.Id))
            {
                //throw new ArgumentException("TabPager уже имеет открытую вкладку с таким Id; Id должны быть уникальными для каждой вкладки", "TabPageVM.Id");
                return false;
            }
            this.AllTabPages.Add(page);
            page.TabPager = this;
            return true;
        }

        public bool AddOrActivateTabPage(TabPageVM page)
        {
            if (this.AddTabPage(page)) return true;
            return ActivateTabPage(page);
        }

        public bool HighlightTabPage(TabPageVM page)
        {
            throw new NotImplementedException();
        }

        public bool HighlightTabPage(string pageId)
        {
            throw new NotImplementedException();
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
