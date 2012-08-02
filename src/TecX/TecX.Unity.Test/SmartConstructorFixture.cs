namespace TecX.Unity.Test
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Injection;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class SmartConstructorFixture
    {
        private static class Constants
        {
            /// <summary>blablub</summary>
            public const string ConnectionStringValue = "blablub";
        }

        [TestMethod]
        public void WhenSpecifyingOneCtorArgUnityWillResolveTheOthers()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<ICtorTest, CtorTest>(
                new SmartConstructor("connectionString", Constants.ConnectionStringValue));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void WhenSpecifyingCtorArgWithAmbiguousNameCtorIsMatchedByType()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<ICtorTest, CtorTest>(new SmartConstructor(
                new ConstructorParameter("connectionString", Constants.ConnectionStringValue),
                new ConstructorParameter("anotherParam", "I'm a string")));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void WhenFilterByCtorTakesAllArgsFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new[]
                    {
                        new ConstructorParameter("connectionString", Constants.ConnectionStringValue)
                    },
                new DefaultMatchingConventionsPolicy());

            Predicate<ConstructorInfo> filter = matcher.ConstructorDoesNotTakeAllArguments;

            var ctors = typeof(CtorTest).GetConstructors();

            var result = ctors
                .Where(ctor => !filter(ctor))
                .OrderBy(ctor => ctor.GetParameters().Length);

            // must find Ctortest(string,ILogger), CtorTest(string,ILogger,string)
            // and CtorTest(string,ILogger,int)
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void WhenFilterByNonSatisfiedPrimitiveArgsFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new[]
                    {
                        new ConstructorParameter("connectionString", Constants.ConnectionStringValue)
                    }, 
                new DefaultMatchingConventionsPolicy());

            Predicate<ConstructorInfo> filter = matcher.NonSatisfiedPrimitiveArgs;

            var ctors = typeof(CtorTest).GetConstructors();

            var result = ctors.Where(ctor => !filter(ctor))
                .OrderBy(ctor => ctor.GetParameters().Length);

            // must find CtorTest(ILogger) and CtorTest(string,ILogger)
            // the other two have primitive parameters that are not satisfied
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.ElementAt(0).GetParameters().Length);
            Assert.AreEqual(2, result.ElementAt(1).GetParameters().Length);
        }

        [TestMethod]
        public void CanMatchArgumentNamesByConvention()
        {
            var container = new UnityContainer();

            var instance = new MatchByConvention();

            container.RegisterType<MyService>(
                new SmartConstructor(new[] { new ConstructorParameter(instance) }));

            var sut = container.Resolve<MyService>();

            Assert.IsNotNull(sut.Convention);
            Assert.AreSame(instance, sut.Convention);
        }

        [TestMethod]
        public void CanInvokeDefaultCtor()
        {
            var container = new UnityContainer();

            container.RegisterType<DefaultCtorOnly>(new SmartConstructor());

            var sut = container.Resolve<DefaultCtorOnly>();

            Assert.IsNotNull(sut);
        }
    }
}