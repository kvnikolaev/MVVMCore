using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MVVMCore
{
    internal class MainWindowVM : CoreVM
    {
        MainWindow _mainWindow;

        public string Title { get; set; } = "";

        private string _titleText = "";
        public string TitleButtonText { get => _titleText; set { _titleText = value; OnPropertyChanged(); } }

        private bool _isBusy = false;
        public bool IsBusy { get => _isBusy; set => SetField(ref _isBusy, value); }

        public MainWindowVM(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _mainWindow.DataContext = this;
            // ещё какие-то настройки

            InitializeTaskOverload();
        }

        public void Show()
        {
            _mainWindow.Show();
            Title = _titleTemplate;
            OnPropertyChanged(nameof(Title));
        }

        private RelayCommand _changeTitleCommand;
        public RelayCommand ChangeTitleCommand => _changeTitleCommand ?? (_changeTitleCommand = new RelayCommand(ChangeTitle_Execute, ChangeTitle_CanExecute));

        int _titleCounter = 1;
        string _titleTemplate = "Изменный заголовок";


        private void ChangeTitle_Execute(object parameter)
        {
            Task.Run(() =>
            {
                IsBusy = true;
                var t = Application.Current.Dispatcher.CheckAccess();
                Application.Current.Dispatcher.Invoke(() => CommandManager.InvalidateRequerySuggested())
                ;
                Thread.Sleep(1500);
                _titleCounter++;
                Title = _titleTemplate + " x" + _titleCounter;
                OnPropertyChanged(nameof(Title));

                TitleButtonText = _titleTemplate + " x" + _titleCounter;
                IsBusy = false;
            });
        }

        private bool ChangeTitle_CanExecute(object parameter)
        {
            return true;// !_isBusy;
        }


        #region Task Limit

        private int _taskAmount = 100;

        public LoadingControlVM[] LoadingControls { get; set; }

        private void InitializeTaskOverload()
        {
            LoadingControls = new LoadingControlVM[_taskAmount];
            for (int i = 0; i < LoadingControls.Length; i++)
            {
                LoadingControls[i] = new LoadingControlVM() { Text = i.ToString(), IsBusy = false };
            }
        }


        private RelayCommand _refreshAllCommand;
        public RelayCommand RefreshAllCommand => _refreshAllCommand ?? (_refreshAllCommand = new RelayCommand(RefreshAll_Execute, RefreshAll_CanExecute));

        private void RefreshAll_Execute(object parameter)
        {
             var loadButtons = FindVisualChildren<Button>(_mainWindow.TaskContainer).ToList();

            //if (loadButtons != null)
            //{
            //    foreach (var button in loadButtons)
            //    {
            //        //button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            //        button.Command.Execute(null);
            //    }
            //}

            for (int i = 0; i < loadButtons.Count(); i++)
            {
                loadButtons[i].Command.Execute(LoadingControls[i]);
            }
        }

        private bool RefreshAll_CanExecute(object parameter)
        {
            return true;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                var count1 = VisualTreeHelper.GetChildrenCount(dependencyObject);
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child is T) // != null
                    {
                        yield return (T)child;
                    }

                    foreach (T subChild in FindVisualChildren<T>(child))
                    {
                        yield return subChild;
                    }

                }
            }
        }


        private RelayCommand _loadEmulationCommand;
        public RelayCommand LoadEmulationCommand => _loadEmulationCommand ?? (_loadEmulationCommand = new RelayCommand(LoadEmulation_Execute, LoadEmulation_CanExecute));

        private void LoadEmulation_Execute(object parameter)
        {
            if (parameter is LoadingControlVM lc)
            {
                Task.Run(() =>
                {
                    lc.IsBusy = true;
                    Thread.Sleep(2000);
                    lc.IsBusy = false;
                });
            }

        }

        private bool LoadEmulation_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region Child Window

        private RelayCommand _openChildWindowCommand;
        public RelayCommand OpenChildWindowCommand => _openChildWindowCommand ?? (_changeTitleCommand = new RelayCommand(OpenChildWindow_Execute, OpenChildWindow_CanExecute));

        private void OpenChildWindow_Execute(object parameter)
        {
            Task.Run(() =>
            {

                var t = Application.Current.Dispatcher.CheckAccess();
                var t1 = _mainWindow.Dispatcher.CheckAccess();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var t = Application.Current.Dispatcher.CheckAccess();
                    var t1 = _mainWindow.Dispatcher.CheckAccess();

                    var wq = Application.Current.Dispatcher.Equals(_mainWindow.Dispatcher);

                    Window window = new Window()
                    {
                        Width = 300,
                        Height = 300,
                        WindowStartupLocation = WindowStartupLocation.Manual,
                        Left = (_mainWindow.Left + _mainWindow.Width),
                        Top = (_mainWindow.Top)
                    };
                    window.Show();
                });
            });
        }

        private bool OpenChildWindow_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

    }
}
