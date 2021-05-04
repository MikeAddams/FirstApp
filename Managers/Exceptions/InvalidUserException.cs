using System;

namespace Managers.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string msg)
            : base(String.Format("Invalid User: {0}", msg))
        {

        }
    }
}
