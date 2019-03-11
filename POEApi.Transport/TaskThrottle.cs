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
        public int SimultaneiousTasksLimit { get; protected set; }

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

        public TaskThrottle(TimeSpan windowSize, int windowLimit, int simultaneiousTasksLimit)
        {
            CurrentTasks = new Queue<DateTime>(simultaneiousTasksLimit);
            WindowSize = windowSize;
            WindowLimit = windowLimit;
            SimultaneiousTasksLimit = simultaneiousTasksLimit;
        }

        public void StartTask()
        {
            bool finished = false;
            while (!finished)
            {
                while (NumberOfOutstandingTasks >= SimultaneiousTasksLimit)
                {
                    System.Threading.Thread.Sleep(100);
                }

                lock(_lockObject)
                {
                    if (NumberOfOutstandingTasks >= SimultaneiousTasksLimit)
                    {
                        // Another thread added a task to the queue before we could get the lock.
                        continue;
                    }

                    RemvoeExpiredTasks();
                    if (CurrentTasks.Count == WindowLimit)
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
                RemvoeExpiredTasks();
            }
        }

        protected void RemvoeExpiredTasks()
        {
            while (CurrentTasks.Count > 0 && CurrentTasks.Peek() <= DateTime.Now)
            {
                CurrentTasks.Dequeue();
            }
        }

        public void HandleUnexpectedOverload()
        {
            lock (_lockObject)
            {
                // We've unexpectedly gone over the number of allowed requests in a time period.  Fill up the set of
                // current tasks as if we had started tasks.
                while (CurrentTasks.Count < WindowLimit)
                    CurrentTasks.Enqueue(DateTime.Now);

                // Wait at least as long as the WindowSize before trying another task.
                var timeUntilNextTask = CurrentTasks.Peek() - DateTime.Now;
                TimeSpan waitTime = timeUntilNextTask > WindowSize ? timeUntilNextTask : WindowSize;
                Throttled?.Invoke(this, new ThottledEventArgs(waitTime, false));  // Not an expected throttle event.
                System.Threading.Thread.Sleep(waitTime);
            }
        }
    }
}
