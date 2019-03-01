using System;

namespace POEApi.Infrastructure.Events
{
    public class ThottledEventArgs : EventArgs
    {
        public TimeSpan WaitTime { get; private set; }
        public ThottledEventArgs(TimeSpan waitTime)
        {
            WaitTime = waitTime;
        }
    }
}
