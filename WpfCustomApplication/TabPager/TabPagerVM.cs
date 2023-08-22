using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfElements;

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
        public ObservablePairCollection<string, TabPageVM> AllTabPages { get; set; } = new ObservablePairCollection<string, TabPageVM>();

        private TabPageVM _selectedTab;
        public TabPageVM SelectedTab { get => _selectedTab; set => SetField(ref _selectedTab, value); }

        public bool ActivateTabPage(TabPageVM page)
        {
            var tab = this.AllTabPages.FirstOrDefault(x => x.Value == page);
            if (tab == null) return false;

            this.SelectedTab = tab.Value;
            return true;
        }

        public bool ActivateTabPage(string pageId)
        {
            var tab = this.AllTabPages.FirstOrDefault(x => x.Key == pageId);
            if (tab == null) return false;

            this.SelectedTab = tab.Value;
            return true;
        }

        public bool AddTabPage(TabPageVM page, string pageId)
        {
            if (this.AllTabPages.Any(x => x.Key == pageId))
            {
                //throw new ArgumentException("TabPager уже имеет открытую вкладку с таким Id; Id должны быть уникальными для каждой вкладки", "TabPageVM.Id");
                return false;
            }
            this.AllTabPages.Add(pageId, page);
            page.TabPager = this;
            return true;
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
            var tab = this.AllTabPages.FirstOrDefault(x => x.Value == page);
            if (tab == null) 
                 return false;
            this.AllTabPages.Remove(tab);
            page.Dispose();
            return true;
        }

        public bool CloseTabPage(string pageId)
        {
            throw new NotImplementedException();
        }

        public bool EditTabKey(string oldKey, string newKey)
        {
            if (string.IsNullOrEmpty(newKey)) return false;
            if (newKey == oldKey) return false;
            if (AllTabPages.Keys.Contains(newKey)) return false;
            var tab = this.AllTabPages.FirstOrDefault(x => x.Key == oldKey);
            if (tab == null)
                return false;
            tab.Key = newKey;
            return true;
        }

        #endregion
    }
}
