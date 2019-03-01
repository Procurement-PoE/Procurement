namespace POEApi.Model.Events
{
    public class StashLoadedEventArgs : POEEventArgs
    {
        public int StashID { get; private set; }
        public int NumTabs { get; private set; }

        public StashLoadedEventArgs(int index, int numTabs, POEEventState state) :
            base(state)
        {
            StashID = index;
            NumTabs = numTabs;
        }
    }
}
