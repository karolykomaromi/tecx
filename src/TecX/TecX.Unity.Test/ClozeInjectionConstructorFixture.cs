using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Injection;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class ClozeInjectionConstructorFixture
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
                new ClozeInjectionConstructor("connectionString", Constants.ConnectionStringValue));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void WhenSpecifyingCtorArgWithAmbiguousNameCtorIsMatchedByType()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<ICtorTest, CtorTest>(
                new ClozeInjectionConstructor(new ConstructorArgumentCollection
                                                  {
                                                      new ConstructorArgument("connectionString", Constants.ConnectionStringValue),
                                                      new ConstructorArgument("anotherParam", "I'm a string")
                                                  }));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void WhenFilterByCtorTakesAllArgsFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new ConstructorArgumentCollection
                    {
                        new ConstructorArgument("connectionString", Constants.ConnectionStringValue)
                    });

            Predicate<ConstructorInfo> filter = matcher.ConstructorTakesAllArguments;

            IEnumerable<Predicate<ConstructorInfo>> filters = new[] {filter};

            var ctors = typeof (CtorTest).GetConstructors();

            var result = ctors.Where(ctor => !filters.Any(f => f(ctor)));

            //must find Ctortest(string,ILogger), CtorTest(string,ILogger,string)
            //and CtorTest(string,ILogger,int)
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void WhenFilterByNonSatisfiedPrimitiveArgsFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new ConstructorArgumentCollection
                    {
                        new ConstructorArgument("connectionString", Constants.ConnectionStringValue)
                    });

            Predicate<ConstructorInfo> filter = matcher.NonSatisfiedPrimitiveArgs;

            IEnumerable<Predicate<ConstructorInfo>> filters = new[] { filter };

            var ctors = typeof(CtorTest).GetConstructors();

            var result = ctors.Where(ctor => !filters.Any(f => f(ctor)))
                .OrderBy(ctor => ctor.GetParameters().Length);

            //must find CtorTest(ILogger) and CtorTest(string,ILogger)
            //the other two have primitive parameters that are not satisfied
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.First().GetParameters().Length);
            Assert.AreEqual(2, result.ElementAt(1).GetParameters().Length);
        }

        [TestMethod]
        public void WhenFilterByArgTypesFitFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new ConstructorArgumentCollection
                    {
                        new ConstructorArgument("connectionString", Constants.ConnectionStringValue),
                        new ConstructorArgument("anotherParam", "I'm a string")
                    });

            Predicate<ConstructorInfo> filter = matcher.ArgumentTypesMatch;

            IEnumerable<Predicate<ConstructorInfo>> filters = new[] { filter };

            var ctors = typeof(CtorTest).GetConstructors();

            var result = ctors.Where(ctor => !filters.Any(f => f(ctor)))
                .OrderBy(ctor => ctor.GetParameters().Length);

            //must find CtorTest(string,ILogger,string), CtorTest(string,ILogger) and CtorTest(ILogger)
            //ctor with one and two args do not take any argument whose type does not match the
            //type of any provided arg
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(1, result.First().GetParameters().Length);
            Assert.AreEqual(2, result.ElementAt(1).GetParameters().Length);
            Assert.AreEqual(3, result.ElementAt(2).GetParameters().Length);
        }
    }
}