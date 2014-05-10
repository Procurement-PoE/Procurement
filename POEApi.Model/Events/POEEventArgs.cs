using System;

namespace POEApi.Model.Events
{
    public class POEEventArgs : EventArgs
    {
        public POEEventState State { get; private set; }
        public POEEventArgs(POEEventState state)
        {
            this.State = state;
        }
    }
}
