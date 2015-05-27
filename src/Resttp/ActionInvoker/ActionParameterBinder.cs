using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Resttp.ActionInvoker
{
    public class ActionParameterBinder : IActionParameterBinder
    {
        public void BindParameters(ActionDescriptor actionDescriptor, IDictionary<string, object> environment, IEnumerable<MediaTypeFormatter> formatters)
        {
            var queryString = environment["owin.RequestQueryString"] as string;
            if (queryString == null)
                throw new ArgumentNullException("environment[\"owin.RequestQueryString\"]");
            var queryParameters = GenerateQueryParameter(queryString);
            foreach (var param in actionDescriptor.ActionArguments)
                BindParameter(queryParameters, param);
        }

        private void BindParameter(IDictionary<string, string> queryParameters, ActionArgumentDescriptor actionParam)
        {
            if (actionParam.ParamValue != null)
                return;

            var queryParam = queryParameters.Any(p => p.Key.Equals(actionParam.ParamName, StringComparison.OrdinalIgnoreCase)) ? queryParameters[actionParam.ParamName] : null;

            var isNullableActionParam = !actionParam.ParamType.IsValueType || (Nullable.GetUnderlyingType(actionParam.ParamType) != null);
            if ((queryParam == null && isNullableActionParam))
                return;
            else if (queryParam == null && !isNullableActionParam)
                throw new Exception("Failed to bind parameter " + actionParam.ParamName + " of type " + actionParam.ParamType);
            else
                AssignValue(queryParam, actionParam);
        }

        private IDictionary<string, string> GenerateQueryParameter(string queryString)
        {
            return queryString.Split('&').Where(x => !string.IsNullOrEmpty(x)).ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);
        }

        private void AssignValue(string queryParam, ActionArgumentDescriptor actionParam)
        {
            try
            {
                actionParam.ParamValue = Convert.ChangeType(queryParam, actionParam.ParamType);
            }
            catch(FormatException e)
            {
                throw new Exception("Failed to bind parameter " + actionParam.ParamName + " of type " + actionParam.ParamType.Name + ". Query string value: " + queryParam);
            }
        }
    }
}
