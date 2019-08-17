using System;

namespace Procurement.Utility
{
    public class ClientLogFileEventArgs : EventArgs
    {
        public DateTime EventDateTime { get; private set; }
        public long EventTimestamp { get; private set; }
        public string LocationEntered { get; private set; }

        public ClientLogFileEventArgs(DateTime eventDateTime, long eventTimestamp, string locationEntered)
        {
            EventDateTime = eventDateTime;
            EventTimestamp = eventTimestamp;
            LocationEntered = locationEntered;
        }
    }
}
