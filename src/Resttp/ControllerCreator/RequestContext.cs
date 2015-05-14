using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Resttp
{
    public class RequestContext
    {
        public RequestContext(IDictionary<string, object> owinEnvironment)
        {
            OwinEnvironment = owinEnvironment;
        }

        public IDictionary<string, object> OwinEnvironment { get; set; }

        /// <summary>
        /// Stream.Null MAY be used as a placeholder if there is no request body.
        /// </summary>
        public Stream Body { get { return OwinEnvironment["owin.RequestBody"] as Stream; } }

        public IDictionary<string, string[]> Headers
        {
            get { return OwinEnvironment["owin.RequestHeaders"] as IDictionary<string, string[]>; }
        }

        public string Method { get { return OwinEnvironment["owin.RequestMethod"] as string; } }

        public string Scheme { get { return OwinEnvironment["owin.RequestScheme"] as string; } }

        public string PathBase { get { return OwinEnvironment["owin.RequestPathBase"] as string; } }

        public string RelativePath { get { return OwinEnvironment["owin.RequestPath"] as string; } }

        public string FullPath { get { return PathBase + RelativePath; } }

        public string Uri
        {
            get
            {
                var uri = (string)OwinEnvironment["owin.RequestScheme"] + "://" + Headers["Host"].First() + (string)OwinEnvironment["owin.RequestPathBase"] + (string)OwinEnvironment["owin.RequestPath"];

                if ((string)OwinEnvironment["owin.RequestQueryString"] != "")
                    uri += "?" + (string)OwinEnvironment["owin.RequestQueryString"];
                return uri;
            }
        }

        /// <summary>
        /// Without the leading "?". May be empty string.
        /// </summary>
        public string QueryString { get { return OwinEnvironment["owin.RequestQueryString"] as string; } }

    }
}
