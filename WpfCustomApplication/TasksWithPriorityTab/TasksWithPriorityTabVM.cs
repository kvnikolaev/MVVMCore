using MVVMCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using TaskSchedulerWithPriority;

namespace WpfCustomApplication
{
    internal class TasksWithPriorityTabVM : TabPageVM
    {
        public TasksWithPriorityTabVM()
        {
            this.View = new TasksWithPriorityTab();
            InitializeTaskOverload();
        }

        #region TabPageVM Implementation

        public override object View { get; protected set; }

        private string _title = "Задачи с приоритетом";
        public override string Title { get => _title; set { _title = value; } }

        protected override void CloseExecuted(object parameter)
        {
            TabPager?.CloseTabPage(this);
        }

        protected override bool CloseCanExecuted(object parameter)
        {
            //!!if (IsBusy)
            //{
            //    TabPager.ActivatePage(this);
            //    return false;
            //}
            //return true;

            return true ;
        }
        #endregion

        #region Task Limit

        private int taskAmount = 100;

        public LoadingControls LoadingControls { get; set; }

        private void InitializeTaskOverload()
        {
            // Все таски будут выполнять одну работу
            Action<LoadingControlVM> anonymMethod = (lc) =>
            {
                Thread.Sleep(1200);
                int rnd = Random.Shared.Next(1, 4);
                if (rnd % 3 < 2)
                    lc.Status = LoadingStatus.OK;
                else
                    lc.Status = LoadingStatus.NegativeResult;
            };

            LoadingControls = new LoadingControls();
            for (int i = 0; i < taskAmount; i++)
            {
                LoadingControls.AddLoadingControl(new LoadingControlVM { Index = i, Text = i.ToString(), Action = anonymMethod });
            }
        }

        private RelayCommand _refreshAllCommand;
        public RelayCommand RefreshAllCommand => _refreshAllCommand ?? (_refreshAllCommand = new RelayCommand(RefreshAll_Executed, RefreshAll_CanExecuted));

        private void RefreshAll_Executed(object parameter)
        {
            LoadingControls.RunAll();
        }

        private bool RefreshAll_CanExecuted(object parameter)
        {
            return LoadingControls.AllDone;
        }

        private RelayCommand _emulateLoadingCommand;
        public RelayCommand EmulateLoadingCommand => _emulateLoadingCommand ?? (_emulateLoadingCommand = new RelayCommand(LoadEmulation_Executed, LoadEmulation_CanExecuted));

        private void LoadEmulation_Executed(object parameter)
        {
            if (parameter is LoadingControlVM lc)
            {
                LoadingControls.Run(lc.Index);
            }
        }

        private bool LoadEmulation_CanExecuted(object parameter)
        {
            return true;
        }
        #endregion

        #region Child Window

        private Window _childWindow;

        private string _actionText = "Open";
        public string ActionText { get => _actionText; set => SetField(ref _actionText, value); }

        private RelayCommand _openChildWindowCommand;
        public RelayCommand OpenChildWindowCommand => _openChildWindowCommand ?? (_openChildWindowCommand = new RelayCommand(OpenChildWindow_Executed, OpenChildWindow_CanExecuted));

        private void OpenChildWindow_Executed(object parameter)
        {
            TaskWithPriority.RunWithPriority(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var t = Application.Current.Dispatcher.CheckAccess();
                    var t1 = Application.Current.MainWindow.Dispatcher.CheckAccess();

                    var wq = Application.Current.Dispatcher.Equals(Application.Current.MainWindow.Dispatcher);

                    if (_childWindow == null)
                    {
                        _childWindow = new Window()
                        {
                            Width = 300,
                            Height = 300,
                            WindowStartupLocation = WindowStartupLocation.Manual,
                            Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width),
                            Top = (Application.Current.MainWindow.Top)
                        };
                        _childWindow.Owner = Application.Current.MainWindow;
                        _childWindow.Closing += _childWindow_CanceledClosing; ;
                    }
                    if (_childWindow.IsVisible)
                    {
                        _childWindow.Hide();
                        ActionText = "Open";
                    }
                    else
                    {
                        _childWindow.Show();
                        ActionText = "Close";
                    }
                });
            }, lowPriority: false);
        }

        private bool OpenChildWindow_CanExecuted(object parameter)
        {
            return true;
        }


        private void _childWindow_CanceledClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _childWindow.Hide();
            ActionText = "Open";
        }


        #endregion
    }
}
