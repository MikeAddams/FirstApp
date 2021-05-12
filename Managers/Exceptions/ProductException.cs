using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Exceptions
{
    public class ProductException : Exception
    {
        public ProductException(string msg)
            : base(String.Format("Product Exception: {0}", msg))
        {

        }
    }
}
