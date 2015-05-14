namespace POEApi.Model.Events
{
    public class AuthenticateEventArgs : POEEventArgs
    {
        public string Email { get; private set; }
        public string AccName { get; private set; }
        public AuthenticateEventArgs(string email, string accname, POEEventState state) :
            base(state)
        {
            this.Email = email;
            this.AccName = accname;
        }
    }
}
