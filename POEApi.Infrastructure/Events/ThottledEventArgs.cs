using System;

namespace POEApi.Infrastructure.Events
{
    public class ThottledEventArgs : EventArgs
    {
        public TimeSpan WaitTime { get; private set; }
        /// <summary>
        /// Whether the throttling event was expected.  If it was not expected, there might be other agents or
        /// untracked actions using up resources towards the limit.
        /// </summary>
        public bool Expected { get; private set; }
        public ThottledEventArgs(TimeSpan waitTime, bool expected = true)
        {
            WaitTime = waitTime;
            Expected = expected;
        }
    }
}
