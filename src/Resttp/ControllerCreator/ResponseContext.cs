using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resttp
{
    public class ResponseContext
    {
        public ResponseContext(IDictionary<string, object> owinEnvironment)
        {
            OwinEnvironment = owinEnvironment;
        }

        public IDictionary<string, object> OwinEnvironment { get; set; }

        public Stream Body { get { return OwinEnvironment["owin.ResponseBody"] as Stream; } }

        public IDictionary<string, string[]> Headers
        {
            get { return OwinEnvironment["owin.ResponseHeaders"] as IDictionary<string, string[]>; }
        }

        /// <summary>
        /// Default 200.
        /// </summary>
        public int StatusCode { get { return (int)OwinEnvironment["owin.ResponseStatusCode"]; } set { OwinEnvironment["owin.ResponseStatusCode"] = value; } }

        public string ReasonPhrase { get { return OwinEnvironment["owin.ResponseReasonPhrase"] as string; } set { OwinEnvironment["owin.ResponseReasonPhrase"] = value; } }

        public string Protocol { get { return OwinEnvironment["owin.ResponseProtocol"] as string; } set { OwinEnvironment["owin.ResponseProtocol"] = value; } }
    
        public void CreateResponse(string responseText, int statusCode = 200)
        {
            var responseBody = OwinEnvironment["owin.ResponseBody"] as Stream;
            OwinEnvironment["owin.ResponseStatusCode"] = statusCode;
            using (var writer = new StreamWriter(responseBody))
            {
                writer.Write(responseText);
            }
        }
    }
}
