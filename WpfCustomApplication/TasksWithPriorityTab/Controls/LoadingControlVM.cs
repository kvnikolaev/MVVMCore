using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TaskSchedulerWithPriority;
using MVVMCore;

namespace WpfCustomApplication
{
    public class LoadingControlVM : CoreVM
    {
        public int Index { get; set; }

        private LoadingStatus _status;
        public LoadingStatus Status { get => _status; internal set => SetField(ref _status, value); }

        private bool _isOnHold; //!!
        public bool IsOnHold { get => _isOnHold; set => SetField(ref _isOnHold, value); }

        private bool _isBusy;
        /// <summary>
        /// Indicates when Action is on going
        /// </summary>
        public bool IsBusy { get => _isBusy; private set { SetField(ref _isBusy, value); IsOnHold = false; } }

        private string _text;
        public string Text { get => _text; set => SetField(ref _text, value); }

        private Action<LoadingControlVM> _action;
        public Action<LoadingControlVM> Action { set { _action = value; } get { return GetAction(_action); } }

        public override string ToString()
        {
            return Text;
        }

        private Action<LoadingControlVM> GetAction(Action<LoadingControlVM> action)
        {
            return (lc) =>
            {
                lc.IsBusy = true;
                try
                {
                    action(lc);
                }
                catch
                {
                    lc.Status = LoadingStatus.Exception;
                    throw;
                }
                lc.IsBusy = false;
            };
        }
    }

    public enum LoadingStatus
    {
        Unknown,
        OK,
        NegativeResult,
        Exception,
        ConnectionError
    }
}
