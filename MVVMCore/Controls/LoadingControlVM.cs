using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MVVMCore
{
    public class LoadingControlVM : CoreVM
    {
        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set => SetField(ref _isBusy, value); }

        private string _text;
        public string Text { get => _text; set => SetField(ref _text, value); }

        public override string ToString()
        {
            return Text;
        }
    }
}
