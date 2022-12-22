namespace POEApi.Model.JSONProxy
{
    // Represents a pathofexile.com account; should match what is defined at
    // https://www.pathofexile.com/developer/docs/reference#profile .
    public class Account
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string realm { get; set; }

        // TODO: Add objects for guild and twitch information.
    }
}