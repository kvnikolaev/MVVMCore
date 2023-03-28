using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskSchedulerWithPriority;

namespace MVVMCore.Controls
{
    public class LoadingControls : CoreVM, IEnumerable<LoadingControlVM>
    {
        List<LoadingControlVM> _loadingList = new List<LoadingControlVM>();

        List<TaskWithPriority> _loadingTasks = new List<TaskWithPriority>();

        #region IEnumerable
        public IEnumerator<LoadingControlVM> GetEnumerator()
        {
            return _loadingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        public void AddLoadingControl(LoadingControlVM lc)
        {
            this._loadingList.Add(lc);
        }

        public void RunAll()
        {
            // создаем список таск
            this._loadingTasks.Clear();
            foreach(var loading in _loadingList)
            {
                loading.Status = LoadingStatus.Unknown;
                var t = new TaskWithPriority(() => loading.Action(loading), lowPriority: true);
                t.ContinueWith((task) => OnPropertyChanged(nameof(DoneTasks)));
                _loadingTasks.Add(t);

                loading.IsOnHold = true;
            }

            // запускаем таски
            foreach(var task in _loadingTasks)
            {
                task.StartWithPriority(); 
            }
            
            // после всех задач обновляем интерфейс
            var endTask = Task.WhenAll(_loadingTasks);
            endTask.ContinueWith((task) => System.Windows.Application.Current.Dispatcher.Invoke(() => CommandManager.InvalidateRequerySuggested()));
        }

        public void Run(int loadingControlIndex)
        {
            if (loadingControlIndex < 0 || loadingControlIndex >= _loadingList.Count) throw new IndexOutOfRangeException("Индекс вне границ массива");

            _loadingList[loadingControlIndex].Status = LoadingStatus.Unknown;
            TaskWithPriority.Run(() => _loadingList[loadingControlIndex].Action(_loadingList[loadingControlIndex]))
                .ContinueWith((task) => OnPropertyChanged(nameof(DoneTasks)));
        }

        private bool _allDone;
        public bool AllDone { get { SetField(ref _allDone, _loadingTasks.All(task => TaskIsNotInAction(task))); return _allDone; } }

        private bool TaskIsNotInAction(Task task)
        {
            return task.Status != TaskStatus.WaitingToRun &&
                task.Status != TaskStatus.WaitingForChildrenToComplete &&
                task.Status != TaskStatus.WaitingForActivation &&
                task.Status != TaskStatus.Running;

        }
        

        //public int DoneTasks { get => _loadingTasks.Count(task => task.Status == TaskStatus.RanToCompletion); }
        ///
        public int DoneTasks { get => _loadingList.Count(load => load.Status == LoadingStatus.OK); }

        public int ContainTasks { get => _loadingList.Count; }
    }
}
