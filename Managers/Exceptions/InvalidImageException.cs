using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Exceptions
{
    public class InvalidImageException : Exception
    {
        public InvalidImageException(string msg)
            : base(String.Format("Invalid Image: {0}", msg))
        {

        }
    }
}
