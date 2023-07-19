using MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempWpfElements;

namespace WpfCustomApplication
{
    internal interface ITabPager
    {
        public bool AddTabPage(TabPageVM page, string pageId);

        public bool ActivateTabPage(TabPageVM page);

        public bool ActivateTabPage(string pageId);

        public bool HighlightTabPage(TabPageVM page);

        public bool HighlightTabPage(string pageId);

        public bool CloseTabPage(TabPageVM page);

        public bool CloseTabPage(string pageId);

        public ObservablePairCollection<string, TabPageVM> AllTabPages { get; }
    }
}
