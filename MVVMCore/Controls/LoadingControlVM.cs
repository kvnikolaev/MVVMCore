using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TaskSchedulerWithPriority;

namespace MVVMCore
{
    public class LoadingControlVM : CoreVM
    {
        public int Index { get; set; }

        private bool _isOnHold; //!!
        public bool IsOnHold { get => _isOnHold; set => SetField(ref _isOnHold, value); }

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { SetField(ref _isBusy, value); IsOnHold = false; } }

        private bool _resultOk; //!!
        public bool ResultOg { get => _resultOk; set => SetField(ref _resultOk, value); }

        private bool _resultFail; //!!
        public bool ResultFail { get => _resultFail; set => SetField(ref _resultFail, value); }

        private bool _connectionError; //!!

        private string _text;
        public string Text { get => _text; set => SetField(ref _text, value); }

        private Action<LoadingControlVM> _action;
        public Action<LoadingControlVM> Action { get; set; }
        //public Action<LoadingControlVM> Action { get => _action; set { _action = value; _task = new TaskWithPriority(_action, true); var t = new Task(_action); } }

        private TaskWithPriority _task;
        public TaskWithPriority Task { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}
