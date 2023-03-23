using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using TaskSchedulerWithPriority;
using System.Timers;

namespace MSTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TimeOutDefaulScheduler()
        {
            var timer = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 250; i++)
            {
                tasks.Add(Task.Run(() => { Thread.Sleep(500); }));
            }

            tasks.Add(Task.Run(() => 
            { 
                Assert.IsTrue(true); 
                System.Diagnostics.Debug.WriteLine("Last task reached in " + timer.ElapsedMilliseconds + "ms"); 
                /*Environment.Exit(0);*/ 
            }));

            Task.WaitAll(tasks.ToArray());
        }

        [TestMethod]
        public void TimeOutPriorityScheduler_Run()
        {
            var timer = Stopwatch.StartNew();
            List<TaskWithPriority> tasks = new List<TaskWithPriority>();
            for (int i = 0; i < 250; i++)
            {
                tasks.Add(TaskWithPriority.RunWithPriority(() => Thread.Sleep(500), lowPriority: true));
            }

            tasks.Add(TaskWithPriority.RunWithPriority(() => 
            { 
                Assert.IsTrue(true); 
                System.Diagnostics.Debug.WriteLine("Last task reached in " + timer.ElapsedMilliseconds + "ms");  
                /*Environment.Exit(0);*/ 
            }, lowPriority: false));

            Task.WaitAll(tasks.ToArray());
        }

        [TestMethod]
        public void TimeOutPriorityScheduler_Start()
        {
            var timer = Stopwatch.StartNew();
            List<TaskWithPriority> tasks = new List<TaskWithPriority>();
            for (int i = 0; i < 250; i++)
            {
                var t = new TaskWithPriority(() => Thread.Sleep(500), lowPriority: true);
                t.StartWithPriority(true);
                tasks.Add(t);
            }

            var t2 = new TaskWithPriority(() => 
            { 
                Assert.IsTrue(true);
                System.Diagnostics.Debug.WriteLine("Last task reached in " + timer.ElapsedMilliseconds + "ms");
                /*Environment.Exit(0);*/
            }, lowPriority: false);
            t2.StartWithPriority(false);
            tasks.Add(t2);
            Task.WaitAll(tasks.ToArray());
        }

        volatile int actualValue = 0;
        //volatile bool result = false;
        [TestMethod]
        public void OnQueueGotEmpty_Event()
        {
            // Arrange
            int expectedValue = 10;
            bool result = false;
            
            TaskSchedulerWithPriority.TaskSchedulerWithPriority.ScheduleHandler assertMethod = () =>
            {
                // Assert
                result = expectedValue == actualValue;
            };
            TaskSchedulerWithPriority.TaskSchedulerWithPriority.QueueGotEmpty += assertMethod;

            // Act
            var timer = Stopwatch.StartNew();
            List<TaskWithPriority> tasks = new List<TaskWithPriority>();
            for (int i = 0; i < expectedValue - 1; i++)
            {
                var t = new TaskWithPriority(() => 
                { 
                    Thread.Sleep(500); 
                    actualValue++; 
                }, lowPriority: true);
                t.StartWithPriority();
                tasks.Add(t);
            }
            var t2 = new TaskWithPriority(() => { Thread.Sleep(1500); actualValue++; }, lowPriority: true);
            var taskId = t2.Id;
            t2.StartWithPriority();
            tasks.Add(t2);
            Task.WaitAll(tasks.ToArray());
            Assert.AreEqual(result, true);
        }

    }
}