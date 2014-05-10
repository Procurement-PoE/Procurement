using System;

namespace POEApi.Infrastructure
{
    public class ExcessiveRequestException: Exception
    {
        public ExcessiveRequestException() 
            : base("Too many requests to GGG server")
        { }
    }
}
