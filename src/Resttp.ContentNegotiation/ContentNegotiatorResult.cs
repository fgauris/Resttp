using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace Resttp.ContentNegotiation
{
    public class ContentNegotiatorResult
    {
        public ContentNegotiatorResult(MediaTypeFormatter formatter, string mediaType)
        {
            MediaType = mediaType;
            Formatter = formatter;
        }
        public string MediaType { get; set; }

        public MediaTypeFormatter Formatter { get; set; }
    }
}
