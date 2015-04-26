using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resttp.IoC;
using Resttp.IoC.Registration;

namespace Resttp.Tests.IoC
{
    [TestClass]
    public class IoCContainerTests
    {
        [TestMethod]
        public void Should_ResolveAddInstance()
        {
            var builder = new IoCContainerBuilder();
            builder.AddInstance<IA1>(() => new A()).SetSingleton();
            var container = builder.Build();
            var ia1 = container.Resolve<IA1>();
            var ia2 = container.Resolve<IA1>();
            Assert.IsNotNull(ia1);
            Assert.IsNotNull(ia2);
            Assert.AreSame(ia1, ia2);
        }

        [TestMethod]
        public void Should_ResolveAddType()
        {
            var builder = new IoCContainerBuilder();
            builder.AddType<A>().For<IA1>().SetSingleton();
            var container = builder.Build();
            var ia1 = container.Resolve<IA1>();
            Assert.IsNotNull(ia1);
        }

        [TestMethod]
        public void Should_ResolveTwoInterfacesWithSameClass()
        {
            var builder = new IoCContainerBuilder();
            builder.AddType<A>().For<IA1>().SetSingleton();
            builder.AddType<A>().For<IA2>().SetSingleton();
            var container = builder.Build();
            var ia1 = container.Resolve<IA1>();
            var ia2 = container.Resolve<IA2>();
            Assert.IsNotNull(ia1);
            Assert.IsNotNull(ia2);
            Assert.AreNotSame(ia1, ia2);
        }

        [TestMethod]
        public void Should_ReturnNullIfNotRegistered()
        {
            var builder = new IoCContainerBuilder();
            var container = builder.Build();
            try
            {
                var ia1 = container.Resolve<IA1>();
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void Should_UseDefaultParameters()
        {
            var builder = new IoCContainerBuilder();
            builder
                .AddType<AParam>().ForSelf()
                .WithParameters(new Parameter("sk", 5))
                .SetSingleton();
            var container = builder.Build();
            var a = container.Resolve<AParam>();
            Assert.IsNotNull(a);
            Assert.AreEqual(a.Sk, 5);

        }
    }


    public class AParam : IA1
    {
        public int Sk { get; set; }
        public AParam(int sk)
        {
            Sk = sk;
        }
        public int vienas() { return 1; }
    }


    public class A : IA1, IA2
    {
        public int vienas() { return 1; }

        public int du() { return 2; }
    }

    public interface IA1
    {
        int vienas();
    }

    public interface IA2
    {
        int du();
    }
}
