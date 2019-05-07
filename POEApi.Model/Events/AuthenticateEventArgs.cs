namespace POEApi.Model.Events
{
    public class AuthenticateEventArgs : POEEventArgs
    {
        public string Email { get; private set; }
        public string Realm { get; private set; }

        public AuthenticateEventArgs(string email, string realm,POEEventState state) :
            base(state)
        {
            Email = email;
            Realm = realm;
        }
    }
}
