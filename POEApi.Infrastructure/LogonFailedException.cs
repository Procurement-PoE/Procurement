using System;

namespace POEApi.Infrastructure
{
    public class LogonFailedException : Exception
    {
        public LogonFailedException(string userName) 
            : base(string.Format("Username or password incorrect. User {0}", userName))
        { }

        public LogonFailedException()
            : base("Incorrect session id")
        { }
    }
}
