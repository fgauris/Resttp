using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resttp.IoC;
using System.Linq.Expressions;

namespace Resttp.Tests.IoC
{
    [TestClass]
    public class IoCContainerBuilderTests
    {
        [TestMethod]
        public void Should_AddSetDelegateAndCreateType()
        {
            var builder = new IoCContainerBuilder();
            var component = builder.AddInstance(() => 3);
            Assert.IsNotNull(component.ComponentRegistration.CreateType);
            Assert.IsNotNull(component.ComponentRegistration.ResultFunc);
            Assert.IsTrue(component.ComponentRegistration.CreateType == typeof(int));
            Assert.IsTrue(component.ComponentRegistration.ResultFunc.Compile()() is int);
            
        }
    }
}
