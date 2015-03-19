using System;

namespace Resttp
{
    public class HttpAttribute : Attribute
    {
        private static string[] methods = { "get", "post", "put", "delete", "head", "options", "patch" };

        private readonly string _method;
        public string Method { get { return _method; } }
        public HttpAttribute(string methodName)
        {
            if (!IsValidMethodName(methodName))
                throw new ArgumentException("Allowed methods: 'Get', 'Post', 'Put', 'Delete', 'Head', 'Patch', 'Options'");
            _method = methodName;
        }

        public override string ToString()
        {
            return Method;
        }

        private bool IsValidMethodName(string method)
        {
            foreach (var item in methods)
	        {
		        if(string.Equals(item, method, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
	        }
            return false;
        }
    }

}
