using System;

namespace POEApi.Infrastructure
{
    public class ForumThreadException : Exception
    {
        public ForumThreadException()
            : base() { }

        public ForumThreadException(string message)
            : base(message) { }
    }
}
