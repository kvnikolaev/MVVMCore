using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MVVMCore;
using WpfCustomApplication.Views;

namespace WpfCustomApplication
{
    internal class BaseLoginVM : AbstractPageVM
    {
        public override object View { get; protected set; }

        public BaseLoginVM()
        {
            this.View = new BaseLogin() { DataContext = this };
        }

        private string _lastLoginTime;
        public string LastLoginTime { get => _lastLoginTime; set => SetField(ref _lastLoginTime, value); }

        #region Commands

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(LoginCommand_Execute, LoginCommand_CanExecute));

        private void LoginCommand_Execute(object parameter)
        {
            LastLoginTime = "Время последнего входа: " + DateTime.Now;
            OnLogin(this, new LoginEventArgs());
        }

        private bool LoginCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

        public delegate void LoginHandler(object sender, LoginEventArgs e);
        public event LoginHandler OnLogin;
    }

    public class LoginEventArgs /*: EventArgs*/
    {

    }

}
