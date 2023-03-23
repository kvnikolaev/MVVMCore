using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerWithPriority
{
    public class TaskWithPriority : Task
    {
        public bool LowPriority { get; set; }
        public bool IsOnHold { get; private set; }
        internal System.Threading.Tasks.TaskScheduler _taskSheduler;

        public TaskWithPriority(Action action, bool lowPriority) : base(action)
        {
            this.LowPriority = lowPriority;
            _taskSheduler = TaskSchedulerWithPriority.Scheduler;
        }

        public static TaskWithPriority RunWithPriority(Action action, bool lowPriority)
        {
            TaskWithPriority task = new TaskWithPriority(action, lowPriority);
            task.StartWithPriority(lowPriority);
            return task;
        }

        public void StartWithPriority(bool lowPriority = false)
        {
            this.LowPriority = lowPriority;
            //!! this.IsOnHold = true;
            this.Start(_taskSheduler);
            //!! this.IsOnHold = false;
        }

        public void StartWithPriority()
        {
            this.Start(_taskSheduler);
        }

        //!!public static TaskFactory Factory
        //{
        //    get => ??
        //}
    }
}
