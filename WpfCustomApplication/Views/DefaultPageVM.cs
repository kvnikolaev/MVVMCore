using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCustomApplication.Views;

namespace WpfCustomApplication
{
    internal class DefaultPageVM : TabPageVM
    {

        public DefaultPageVM(bool canClose = true)
        {
            this.CanClose = canClose;
            this.View = new DefaultPage();

            _defaultPageCounter++;
            _id = "default" + _defaultPageCounter;
        }

        private bool _blocked;
        public bool Blocked { get => _blocked; set => SetField(ref _blocked, value); }

        #region TabPageVM Implementation

        private static int _defaultPageCounter = -1;

        private string _id;
        public override string Id { get => _id; }
        public override object View { get; protected set; }
        
        private string _title = "Start page";
        public override string Title { get => _title; set { _title = value; } }

        protected override void CloseExecuted(object parameter)
        {
            TabPager?.CloseTabPage(this);
        }

        protected override bool CloseCanExecuted(object parameter)
        {
            //if (IsBusy)
            //{
            //    TabPager.ActivatePage(this);
            //    return false;
            //}
            //return true;

            return !Blocked;
        }
        #endregion

    }
}
