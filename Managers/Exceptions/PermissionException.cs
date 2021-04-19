using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Exceptions
{
    public class PermissionException : Exception
    {
        public PermissionException(string msg)
            : base(String.Format("Invalid Permissions: {0}", msg))
        {

        }
    }
}


