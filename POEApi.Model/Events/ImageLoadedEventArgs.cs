using System;

namespace POEApi.Model.Events
{
    public class ImageLoadedEventArgs : POEEventArgs
    {
        public string URL { get; private set; }
        public ImageLoadedEventArgs(string url, POEEventState state) :
            base(state)
        {
            URL = url;
        }
    }
}
