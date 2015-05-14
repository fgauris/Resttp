using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;

namespace Resttp.ContentNegotiation
{
    public interface IContentNegotiator
    {
        ContentNegotiatorResult NegotiateContent(Type type, IDictionary<string, object> owinEnvironment, IEnumerable<MediaTypeFormatter> formatters);
    }
}
