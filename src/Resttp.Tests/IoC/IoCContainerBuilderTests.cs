using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resttp.IoC;
using System.Linq.Expressions;

namespace Resttp.Tests.IoC
{
    [TestClass]
    public class IoCContainerBuilderTests
    {
        [TestMethod]
        public void Should_AddSetDelegateAndLookupType()
        {
            var builder = new IoCContainerBuilder();
            var component = builder.AddInstance(() => 3).ForSelf();
            Assert.IsNotNull(component.ComponentRegistration.CreateType);
            Assert.IsNotNull(component.ComponentRegistration.LookupTypes.First());
            Assert.IsNotNull(component.ComponentRegistration.ResultFunc);
            Assert.IsTrue(component.ComponentRegistration.CreateType == typeof(int));
            Assert.IsNotNull(component.ComponentRegistration.LookupTypes.First() == typeof(int));
            Assert.IsTrue(component.ComponentRegistration.ResultFunc() is int); 
        }
    }
}
