using System;

namespace Resttp
{
    public class RoutingException : Exception
    {
        public RoutingException(string message): base(message){}
    }
}
