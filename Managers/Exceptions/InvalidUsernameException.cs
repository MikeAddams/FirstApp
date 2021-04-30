using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException(string msg)
            : base(String.Format("Invalid Username: {0}", msg))
        {

        }
    }
}
