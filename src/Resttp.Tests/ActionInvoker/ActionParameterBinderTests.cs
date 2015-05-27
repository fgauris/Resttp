using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using Resttp.ActionInvoker;
using System.Net.Http.Formatting;

namespace Resttp.Tests.ActionInvoker
{
    [TestClass]
    public class ActionParameterBinderTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            int labasValue = 121;
            var queryString = "labas="+labasValue+"&iki=qwe";
            IDictionary<string, object> environment = new Dictionary<string, object>();
            environment.Add("owin.RequestQueryString", queryString);
            var paramBinder = new ActionParameterBinder();
            var actionDescriptor = new ActionDescriptor("test");
            actionDescriptor.ActionArguments.Add(new ActionArgumentDescriptor("labas", typeof(int), null));
            paramBinder.BindParameters(actionDescriptor, environment, new List<MediaTypeFormatter>());

            Assert.AreEqual(actionDescriptor.ActionArguments[0].ParamValue, labasValue);
            
            

        }
    }
}
