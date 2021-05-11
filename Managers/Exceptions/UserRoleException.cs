using System;

namespace Managers.Exceptions
{
    public class UserRoleException : Exception
    {
        public UserRoleException(string msg)
            : base(String.Format("Invalid role: {0}", msg))
        {

        }
    }
}
