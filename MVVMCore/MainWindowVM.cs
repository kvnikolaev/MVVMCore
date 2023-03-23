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
using MVVMCore.Controls;

using TaskSchedulerWithPriority;

namespace MVVMCore
{
    internal class MainWindowVM : CoreVM
    {
        MainWindow _mainWindow;

        public string Title { get; set; } = "Priority Task Container Demo";


        
        private string _titleButtonText = "";
        public string TitleButtonText { get => _titleButtonText; set { _titleButtonText = value; OnPropertyChanged(); } }

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
            TaskWithPriority.RunWithPriority(() =>
            {
                IsBusy = true;
                Thread.Sleep(1500);
                _titleCounter++;
                Title = _titleTemplate + " x" + _titleCounter;
                OnPropertyChanged(nameof(Title));

                TitleButtonText = _titleTemplate + " x" + _titleCounter;
                IsBusy = false;
            }, lowPriority: false);
        }

        private bool ChangeTitle_CanExecute(object parameter)
        {
            return !_isBusy;
        }


        #region Task Limit

        private int _taskAmount = 100;

        public LoadingControls LoadingControls { get; set; }

        private void InitializeTaskOverload()
        {
            // Все таски будут выполнять одну работы
            Action<LoadingControlVM> anonymMethod = (lc) =>
            {
                lc.IsBusy = true;
                Thread.Sleep(1200);
                lc.IsBusy = false;
            };

            LoadingControls = new LoadingControls();
            for (int i = 0; i < _taskAmount; i++) 
            {
                LoadingControls.AddLoadingControl(new LoadingControlVM { Index = i, Text = i.ToString(), Action = anonymMethod });
            }

            
        }


        private RelayCommand _refreshAllCommand;
        public RelayCommand RefreshAllCommand => _refreshAllCommand ?? (_refreshAllCommand = new RelayCommand(RefreshAll_Execute, RefreshAll_CanExecute));

        private void RefreshAll_Execute(object parameter)
        {
            LoadingControls.RunAll();
        }

        private bool RefreshAll_CanExecute(object parameter)
        {
            return LoadingControls.AllDone;
        }


        private RelayCommand _loadEmulationCommand;
        public RelayCommand LoadEmulationCommand => _loadEmulationCommand ?? (_loadEmulationCommand = new RelayCommand(LoadEmulation_Execute, LoadEmulation_CanExecute));

        private void LoadEmulation_Execute(object parameter)
        {
            if (parameter is LoadingControlVM lc)
            {
                LoadingControls.Run(lc.Index);


                //lc.IsOnHold = true;
                //TaskWithPriority.RunWithPriority(() =>
                //{
                //    lc.IsBusy = true;
                //    Thread.Sleep(2000);
                //    lc.IsBusy = false;
                //},
                //lowPriority: true);
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
            TaskWithPriority.RunWithPriority(() =>
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
            }, lowPriority: false);
        }

        private bool OpenChildWindow_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

    }
}
