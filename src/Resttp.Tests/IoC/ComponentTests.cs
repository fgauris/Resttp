using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resttp.IoC.Registration;

namespace Resttp.Tests.IoC
{
    [TestClass]
    public class ComponentTests
    {
        [TestMethod]
        public void Should_ImplementInterface()
        {

            var component = new Component(typeof(Dummy)).ForImplementedInterfaces();
            component.WithParameters(new Parameter("Labas", "labas"));
            Assert.IsTrue(component.ComponentRegistration.LookupTypes.Count == 1);
            Assert.IsTrue(component.ComponentRegistration.LookupTypes[0].IsInterface && component.ComponentRegistration.LookupTypes[0].Name=="IDummy");

        }


        public class Dummy : IDummy
        {
            public bool CalledLabas { get; set; }
            public bool CalledIki { get; set; }
            public void Labas()
            {
                CalledLabas = true;
            }

            public void Iki()
            {
                CalledIki = true;
            }
        }

        public interface IDummy
        {
            void Labas();
            void Iki();
        }
    }
}
