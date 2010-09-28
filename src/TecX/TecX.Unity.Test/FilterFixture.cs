using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.Extensions.Primitives;
using TecX.Unity.Registration;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class FilterFixture
    {
        [TestMethod]
        public void CanFilterByConcreteType()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.GetName().Name.StartsWith("Microsoft.VisualStudio", StringComparison.OrdinalIgnoreCase))
                .SelectMany(assembly => assembly.GetTypes());

            Filter<Type> filter = If.Is<TestLogger>();

            var filters = new[] { filter };

            Type type = types
                .Where(t => filters.Any(f => f.IsMatch(t)))
                .Single();

            Assert.IsNotNull(type);
            Assert.IsTrue(type == typeof(TestLogger));
        }

        [TestMethod]
        public void CanFilterByInterface()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.GetName().Name.StartsWith("Microsoft.VisualStudio",StringComparison.OrdinalIgnoreCase))
                .SelectMany(assembly => assembly.GetTypes());

            Filter<Type> filter = If.Implements<ILogger>();

            var filters = new[] { filter };

            var filtered = types
                .Where(t => filters.Any(f => f.IsMatch(t)))
                .OrderBy(t => t.Name);

            Assert.AreEqual(4, filtered.Count());
            Assert.AreEqual(typeof(EnhancedTraceLogger), filtered.ElementAt(0));
            Assert.AreEqual(typeof(Logger), filtered.ElementAt(1));
            Assert.AreEqual(typeof(TestLogger), filtered.ElementAt(2));
            Assert.AreEqual(typeof(TraceLogger), filtered.ElementAt(3));
        }

        [TestMethod]
        public void CanFilterSystemAssemblies()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Filter<Assembly> filter = Filters.ForAssemblies.IsSystemAssembly();

            var filters = new[] { filter };

            assemblies = assemblies.Where(a => !filters.Any(f => f.IsMatch(a)));

            Assembly mscorlib = assemblies.Where(a => a.GetName().Name == "mscorlib")
                .SingleOrDefault();

            Assert.IsNull(mscorlib);

            var systems = assemblies.Where(a => a.GetName().Name.StartsWith("System"));

            Assert.IsTrue(systems.Count() == 0);
        }

        [TestMethod]
        public void CanFilterByInheritedAttribute()
        {
            Filter<Type> filter = Filters.ForTypes.IsDecoratedWith<LoggerAttribute>();

            Assert.IsTrue(filter.IsMatch(typeof(EnhancedTraceLogger)));
        }

        [TestMethod]
        public void CanFilterByImplementedOpenGeneric()
        {
            Filter<Type> filter = Filters.ForTypes.ImplementsOpenGeneric(typeof(IEnumerable<>));

            Assert.IsTrue(filter.IsMatch(typeof(DummyEnumerable)));
        }

        [TestMethod]
        public void CanFilterByImplementsITypeName()
        {
            Filter<Type> filter = Filters.ForTypes.ImplementsITypeName();

            Assert.IsTrue(filter.IsMatch(typeof(Logger)));
        }

        [TestMethod]
        public void CanFilterByImplementsSingleInterface()
        {
            Filter<Type> filter = Filters.ForTypes.ImplementsSingleInterface();

            Assert.IsTrue(filter.IsMatch(typeof(TraceLogger)));
            Assert.IsFalse(filter.IsMatch(typeof(TestLogger)));
        }

        [TestMethod]
        public void CanFilterByAssemblyContainsType()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Filter<Assembly> filter = Filters.ForAssemblies.ContainsType<ILogger>();

            Assembly assembly = assemblies.Single(filter.IsMatch);

            Assert.IsNotNull(assembly);
            Assert.AreEqual("TecX.Unity.Test", assembly.GetName().Name);
        }

        [TestMethod]
        public void CanFilterByAssemblyName()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Filter<Assembly> filter = Filters.ForAssemblies.NameContains("TecX.Unity");

            IEnumerable<Assembly> filtered = assemblies.Where(filter.IsMatch);

            Assert.AreEqual(2, filtered.Count());
        }

        [TestMethod]
        public void CanFilterByIsAssignableFrom()
        {
            Filter<Type> filter = Filters.ForTypes.IsAssignableFrom(typeof(TraceLogger));
            Assert.IsTrue(filter.IsMatch(typeof(ILogger)));

            //a TraceLogger is an ILogger thus whenever you have to assign a value
            //to a variable of Type ILogger you can use a TraceLogger
            //I hate IsAssignableFrom...
            Assert.IsTrue(typeof(ILogger).IsAssignableFrom(typeof(TraceLogger)));
        }

        [TestMethod]
        public void CanGetAllBaseClasses()
        {
            var c = new ClassC();

            var types = c.GetType().GetBaseTypes();

            Assert.AreEqual(3, types.Count());
            Assert.AreEqual(typeof(object), types.ElementAt(0));
            Assert.AreEqual(typeof(ClassA), types.ElementAt(1));
            Assert.AreEqual(typeof(ClassB), types.ElementAt(2));
        }

        [TestMethod]
        public void CanFilterByInherits()
        {
            Filter<Type> filter = Filters.ForTypes.InheritsFrom(typeof (ClassB));

            Assert.IsTrue(filter.IsMatch(typeof(ClassC)));
        }
    }
}
