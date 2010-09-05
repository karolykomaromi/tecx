using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class UnityExtensionFixture
    {
        [TestMethod]
        public void WhenSpecifyingOneCtorArgUnityWillResolveTheOthers()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<ICtorTest, CtorTest>(
                new CtorArgsInjectionConstructur("connectionString", "blablub"));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void WhenSpecifyingCtorArgWithAmbiguousNameCtorIsMatchedByType()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<ICtorTest, CtorTest>(
                new CtorArgsInjectionConstructur(new Dictionary<string, object>
                                                     {
                                                         {"connectionString", "blablub"},
                                                         {"anotherParam", "I'm a string"}
                                                     }));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void WhenFilterByCtorTakesAllArgsFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new Dictionary<string, object>
                    {
                        {"connectionString", "blablub"}
                    });

            Predicate<ConstructorInfo> filter = matcher.FilterByCtorTakesAllArgs;

            IEnumerable<Predicate<ConstructorInfo>> filters = new[] { filter };

            var ctors = typeof(CtorTest).GetConstructors();

            var result = ctors.Where(ctor => !filters.Any(f => f(ctor)));

            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void WhenFilterByNonSatisfiedPrimitiveArgsFindsExpectedCtors()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new Dictionary<string, object>
                    {
                        {"connectionString", "blablub"}
                    });

            Predicate<ConstructorInfo> filter = matcher.FilterByNonSatisfiedPrimitiveArgs;

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
                new Dictionary<string, object>
                    {
                        {"connectionString", "blablub"},
                        {"anotherParam", "I'm a string"}
                    });

            Predicate<ConstructorInfo> filter = matcher.FilterByArgTypesFit;

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

    class CtorTest : ICtorTest
    {
        public CtorTest(ILogger logger)
        {
            Assert.Fail("must not be called");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CtorTest"/> class
        /// </summary>
        public CtorTest(string connectionString, ILogger logger)
        {
            Assert.AreEqual("blablub", connectionString);
            Assert.IsNotNull(logger);
        }

        public CtorTest(string connectionString, ILogger logger, int anotherParam)
        {
            Assert.Fail("must not be selected");
        }

        public CtorTest(string connectionString, ILogger logger, string anotherParam)
        {
            Assert.AreEqual("blablub", connectionString);
            Assert.IsNotNull(logger);
            Assert.AreEqual("I'm a string", anotherParam);
        }
    }

    interface ICtorTest
    {
    }
}
