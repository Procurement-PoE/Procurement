using System;

namespace POEApi.Infrastructure
{
    public class LogonFailedException : Exception
    {
        public LogonFailedException(string userName, string server_type) 
            : base(BuildMessage(userName,server_type))
        { }

        private static string BuildMessage(string userName,string server_type)
        {
            if (server_type == "Garena (RU)")
            {
                return string.Format("Неправильный логин или пароль. Логин {0}", userName);
            }
            else
            {
                return string.Format("Username or password incorrect. User {0}", userName);
            }
        }

        private static string BuildMessage(string server_type)
        {
            if (server_type == "Garena (RU)")
            {
                return "Некорректный идентификатор сессии";
            }
            else
            {
                return "Incorrect session id";
            }
        }

        public LogonFailedException(string server_type)
            : base(BuildMessage(server_type))
        { }
    }
}
