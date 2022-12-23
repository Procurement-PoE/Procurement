using POEApi.Infrastructure.Events;
using System;
using System.Collections.Generic;

namespace POEApi.Transport
{
    public class TaskThrottle
    {
        protected Queue<DateTime> CurrentTasks { get; set; }
        public TimeSpan WindowSize { get; set; }
        public int WindowLimit { get; protected set; }
        public int SimultaneousTasksLimit { get; protected set; }

        private int _numberOfOutstandingTasks;
        public int NumberOfOutstandingTasks
        {
            get
            {
                return _numberOfOutstandingTasks;
            }
            protected set
            {
                _numberOfOutstandingTasks = value;
            }
        }

        public event ThottledEventHandler Throttled;

        private Object _lockObject = new Object();

        public TaskThrottle(TimeSpan windowSize, int windowLimit, int simultaneousTasksLimit)
        {
            CurrentTasks = new Queue<DateTime>(simultaneousTasksLimit);
            WindowSize = windowSize;
            WindowLimit = windowLimit;
            SimultaneousTasksLimit = simultaneousTasksLimit;
        }

        public void AdjustWindowLimit(int newWindowLimit, int newSimultaneousTasksLimit)
        {
            WindowLimit = newWindowLimit;
            SimultaneousTasksLimit = newSimultaneousTasksLimit;
        }

        public void StartTask()
        {
            bool finished = false;
            while (!finished)
            {
                while (NumberOfOutstandingTasks >= SimultaneousTasksLimit)
                {
                    System.Threading.Thread.Sleep(100);
                }

                lock(_lockObject)
                {
                    if (NumberOfOutstandingTasks >= SimultaneousTasksLimit)
                    {
                        // Another thread added a task to the queue before we could get the lock.
                        continue;
                    }

                    RemoveExpiredTasks();
                    while (CurrentTasks.Count >= WindowLimit)
                    {
                        TimeSpan waitTime = CurrentTasks.Dequeue() - DateTime.Now;
                        if (waitTime.TotalMilliseconds > 0)
                        {
                            Throttled?.Invoke(this, new ThottledEventArgs(waitTime));
                            System.Threading.Thread.Sleep(waitTime);
                        }
                    }

                    System.Threading.Interlocked.Increment(ref _numberOfOutstandingTasks);
                    CurrentTasks.Enqueue(DateTime.Now + WindowSize);
                    finished = true;
                }
            }
        }

        public void CompleteTask()
        {
            lock(_lockObject)
            {
                System.Threading.Interlocked.Decrement(ref _numberOfOutstandingTasks);
                RemoveExpiredTasks();
            }
        }

        protected void RemoveExpiredTasks()
        {
            while (CurrentTasks.Count > 0 && CurrentTasks.Peek() <= DateTime.Now)
            {
                CurrentTasks.Dequeue();
            }
        }
    }
}
