namespace POEApi.Model.Events
{
    public class AuthenticateEventArgs : POEEventArgs
    {
        public string Email { get; private set; }
        public AuthenticateEventArgs(string email, POEEventState state) :
            base(state)
        {
            this.Email = email;
        }
    }
}
