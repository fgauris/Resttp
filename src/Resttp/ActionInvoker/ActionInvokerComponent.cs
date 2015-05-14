using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Formatting;

namespace Resttp
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using ContentNegotiator = ContentNegotiation.ContentNegotiator;
    using Resttp.ContentNegotiation;

    public class ActionInvokerComponent
    {
        private AppFunc _next;

        private MediaTypeFormatterCollection _formatters;

        private ContentNegotiator _contentNegotiator;

        public ActionInvokerComponent(AppFunc next, MediaTypeFormatterCollection formatters, ContentNegotiator contentNegotiator)
        {
            _next = next;
            _formatters = formatters;
            _contentNegotiator = contentNegotiator;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var controller = environment["resttp.Controller"] as RestController;
            if (controller == null)
                throw new Exception("Controller not found in OWIN environment dictionary");

            object result = null;
            try
            {
                result = await InvokeAction(controller);
            }
            catch (Exception e)
            {
                result = e;
            }

            await ReturnActionResultAsync(environment, result);
        }

        private async Task<object> InvokeAction(RestController controller)
        {

            return new { Labas = "labas", Skaicius = 1 };
            throw new NotImplementedException();
        }

        private async Task ReturnActionResultAsync(IDictionary<string, object> environment, object @object)
        {
            if (@object == null)
                return;
            var result = _contentNegotiator.NegotiateContent(@object.GetType(), environment, _formatters);
            if (result == null)
                await GenerateFailedToNegotiateContentError(environment, @object.GetType());
            else
                await WriteResultToStream(environment, @object, result);
        }

        public async Task GenerateFailedToNegotiateContentError(IDictionary<string, object> enviroment, Type type)
        {
            var responseBody = enviroment["owin.ResponseBody"] as Stream;
            enviroment["owin.ResponseStatusCode"] = 500;
            using (var writer = new StreamWriter(responseBody))
            {
                await writer.WriteAsync("Failed to negotatiate type " + type.Name);
            }
        }

        public async Task WriteResultToStream(IDictionary<string, object> environment, object @object, ContentNegotiatorResult result)
        {
            var stream = new MemoryStream();
            await result.Formatter.WriteToStreamAsync(@object.GetType(), @object, stream, null, null);
            var responseHeaders = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
            responseHeaders.Add("content-type", new[] { result.MediaType });
            stream.Position = 0;
            var response = environment["owin.ResponseBody"] as Stream;
            await stream.CopyToAsync(response);
        }
    }
}
