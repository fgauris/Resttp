using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.IoC.Exceptions
{
    public class IoCException : Exception
    {
        public IoCException(string message)
            : base(message)
        {
        }
    }
}
