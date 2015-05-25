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
    using Resttp.ActionInvoker;
    using System.Reflection;
    using Resttp.Common;

    public class ActionInvokerComponent
    {
        private AppFunc _next;

        private MediaTypeFormatterCollection _formatters;

        private IContentNegotiator _contentNegotiator;

        private IActionParameterBinder _actionParameterBinder;

        public ActionInvokerComponent(AppFunc next, MediaTypeFormatterCollection formatters, IContentNegotiator contentNegotiator, IActionParameterBinder parameterBinder)
        {
            _next = next;
            _formatters = formatters;
            _contentNegotiator = contentNegotiator;
            _actionParameterBinder = parameterBinder;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var controller = environment["resttp.Controller"] as RestController;
            if (controller == null)
                throw new Exception("Controller not found in OWIN environment dictionary");


            object result = null;
            try
            {
                result = await InvokeAction(controller, environment);
            }
            catch (Exception e)
            {
                result = e;
            }

            await ReturnActionResultAsync(environment, result);
        }

        private async Task<object> InvokeAction(RestController controller, IDictionary<string, object> environment)
        {
            var actionDescriptor = new ActionDescriptorGenerator().GenerateActionDescriptor(controller);

            _actionParameterBinder.BindParameters(actionDescriptor, environment);
            return await InvokeActionAsync(controller, actionDescriptor);
        }

        private async Task<object> InvokeActionAsync(RestController controller, ActionDescriptor descriptor)
        {
            var controllerType = controller.GetType();
            var actionMethod = controllerType.GetMethod(descriptor.ActionName);
            var result = actionMethod.Invoke(controller, descriptor.ActionArguments.Select(a => a.ParamValue).ToArray());
            if (result == null)
                result = (object)DBNull.Value;

            return await Task.FromResult(result);
        }

        private async Task ReturnActionResultAsync(IDictionary<string, object> environment, object @object)
        {
            try
            {
                if (!(@object is DBNull))
                {
                    var result = _contentNegotiator.NegotiateContent(@object.GetType(), environment, _formatters);
                    if (result == null)
                        await GenerateFailedToNegotiateContentError(environment, @object.GetType());
                    else
                        await WriteResultToResponse(environment, @object, result);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task GenerateFailedToNegotiateContentError(IDictionary<string, object> enviroment, Type type)
        {
            var responseBody = enviroment["owin.ResponseBody"] as Stream;
            enviroment["owin.ResponseStatusCode"] = 500;
            using (var writer = new StreamWriter(responseBody))
            {
                await writer.WriteAsync("Failed to negotatiate type <" + type.Name + "> or no acceptable content type not found.");
            }
        }

        public async Task WriteResultToResponse(IDictionary<string, object> environment, object @object, ContentNegotiatorResult result)
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
