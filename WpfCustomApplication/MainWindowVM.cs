using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCustomApplication.Views;

namespace WpfCustomApplication
{
    public class MainWindowVM
    {
        MainWindow _window;

        private const bool FreeAccess = true;

        public void StartShow()
        {
            _window = new MainWindow() { DataContext = this };

            if (FreeAccess)
            {
                ShowContentScreen();
            }
            else 
            { 
                _window.Content = DefaultLoginVM.View;
            }

            _window.Show();
        }

        private void DefaultLogin_Login(object sender, LoginEventArgs e)
        {
            ShowContentScreen();
        }

        private void ShowContentScreen()
        {
            _window.Content = MainViewVM.View;
        }

        #region HotKeys
        
        private RelayCommand _quitCommand;
        public RelayCommand QuitCommand => _quitCommand ?? (_quitCommand = new RelayCommand(QuitCommandExecuted, QuitCommandCanExecute));

        private void QuitCommandExecuted(object parameter)
        {
            _window.Content = DefaultLoginVM.View;
        }

        private bool QuitCommandCanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region Properties and Fields
        private MainViewVM _mainViewVM;
        internal MainViewVM MainViewVM => _mainViewVM ?? (_mainViewVM = new MainViewVM());

        private BaseLoginVM _defaultLoginVM;
        private BaseLoginVM DefaultLoginVM => _defaultLoginVM ?? (_defaultLoginVM = InitLoginVM());

        private BaseLoginVM InitLoginVM()
        {
            var loginVM = new BaseLoginVM();
            loginVM.OnLogin += DefaultLogin_Login;
            return loginVM;
        }
        #endregion
    }
}
