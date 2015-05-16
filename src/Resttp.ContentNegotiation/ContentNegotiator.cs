using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.ContentNegotiation
{
    public class ContentNegotiator : IContentNegotiator
    {
        private const string JsonMimeType = "application/json";
        private readonly string[] XmlMimeTypes;

        public ContentNegotiator()
        {
            XmlMimeTypes = new[] { "application/xml", "text/xml" };
        }

        public ContentNegotiatorResult NegotiateContent(Type type, IDictionary<string, object> owinEnvironment, IEnumerable<MediaTypeFormatter> formatters)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (owinEnvironment == null)
            {
                throw new ArgumentNullException("owinEnvironment");
            }
            if (formatters == null)
            {
                throw new ArgumentNullException("formatters");
            }

            //get best formatter, which would work for given type and request
            var bestFormatter = GetBestFormatter(type, owinEnvironment, formatters);

            return bestFormatter;
        }


        private ContentNegotiatorResult GetBestFormatter(Type type, IDictionary<string, object> owinEnvironment, IEnumerable<MediaTypeFormatter> formatters)
        {
            var jsonFormatter = formatters.FirstOrDefault(f => f is JsonMediaTypeFormatter);
            var xmlFormatter = formatters.FirstOrDefault(f => f is XmlMediaTypeFormatter);


            var headers = owinEnvironment["owin.RequestHeaders"] as IDictionary<string, string[]>;

            var formatter = GetContentTypeFormatter(type, headers, formatters);
            if (formatter != null)
                return formatter;

            if (jsonFormatter.CanWriteType(type) && AcceptsJson(jsonFormatter as JsonMediaTypeFormatter, headers))
                return new ContentNegotiatorResult(jsonFormatter, JsonMimeType);
            if (xmlFormatter.CanWriteType(type) && AcceptsXml(xmlFormatter as XmlMediaTypeFormatter, headers))
                return new ContentNegotiatorResult(xmlFormatter, XmlMimeTypes.First());
            return null;
        }

        private ContentNegotiatorResult GetContentTypeFormatter(Type type, IDictionary<string, string[]> headers, IEnumerable<MediaTypeFormatter> formatters)
        {
            ContentNegotiatorResult result = null;
            var contentTypeHeader = headers.Where(h => string.Equals(h.Key, "content-type", StringComparison.OrdinalIgnoreCase));
            var jsonFormatter = formatters.FirstOrDefault(f => f is JsonMediaTypeFormatter);
            var xmlFormatter = formatters.FirstOrDefault(f => f is XmlMediaTypeFormatter);

            if (contentTypeHeader.Any() && contentTypeHeader.First().Value.Any(v => v.Equals(JsonMimeType, StringComparison.OrdinalIgnoreCase)) && jsonFormatter != null)//Has json content-type
                result = new ContentNegotiatorResult(jsonFormatter, JsonMimeType);
            else if (contentTypeHeader.Any() && contentTypeHeader.First().Value.Any(v => XmlMimeTypes.FirstOrDefault(xml => v.Equals(xml, StringComparison.OrdinalIgnoreCase)) != null))//Has xml content-type
                result = new ContentNegotiatorResult(xmlFormatter, XmlMimeTypes.First());
            return result;
        }


        /// <summary>
        /// Checks if request accepts json as response type
        /// </summary>
        /// <param name="jsonFormatter"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private bool AcceptsJson(JsonMediaTypeFormatter jsonFormatter, IDictionary<string, string[]> headers)
        {
            if (jsonFormatter == null)
                return false;
            
            //Accepts 'application/json'
            if (HasAcceptHeader(headers, JsonMimeType))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if request accepts xml as response type
        /// </summary>
        /// <param name="xmlFormatter"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private bool AcceptsXml(XmlMediaTypeFormatter xmlFormatter, IDictionary<string, string[]> headers)
        {
            if (xmlFormatter == null)
                return false;

            if (XmlMimeTypes.Any(xml => HasAcceptHeader(headers, xml)))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if acceptValue is in one of acceptable values in ACCEPT header
        /// </summary>
        /// <param name="headers">Collection of all request headers</param>
        /// <param name="acceptValue">Acceptable type</param>
        /// <returns></returns>

        private bool HasAcceptHeader(IDictionary<string, string[]> headers, string acceptValue)
        {
            acceptValue = acceptValue.ToLower();
            var acceptHeader = headers.Where(h => string.Equals(h.Key, "accept", StringComparison.OrdinalIgnoreCase));
            if (!acceptHeader.Any())
                return false;
            foreach (var headerValue in acceptHeader.First().Value)
            {
                if (headerValue.ToLower().Contains(acceptValue))
                    return true;
            }
            return false;
        }

        private class BestMediaTypeFormatter
        {
            public BestMediaTypeFormatter(MediaTypeFormatter formatter, string mediaType)
            {
                Formatter = formatter;
                MediaType = mediaType;
            }

            public MediaTypeFormatter Formatter { get; set; }
            public string MediaType { get; set; }
        }
    }
}
