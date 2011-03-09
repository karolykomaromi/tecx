using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly List<Assembly> _assemblies;
        private readonly CompositeFilter<Type> _filter;
        private readonly List<IRegistrationConvention> _conventions;

        public AssemblyScanner()
        {
            _assemblies = new List<Assembly>();
            _filter = new CompositeFilter<Type>();
            _conventions = new List<IRegistrationConvention>();
        }

        public void Assembly(Assembly assembly)
        {
            Guard.AssertNotNull(assembly, "assembly");

            if (!_assemblies.Contains(assembly))
                _assemblies.Add(assembly);
        }

        public void Assembly(string assemblyName)
        {
            Guard.AssertNotEmpty(assemblyName, "assemblyName");

            Assembly(AppDomain.CurrentDomain.Load(assemblyName));
        }

        public void TheCallingAssembly()
        {
            var trace = new StackTrace(false);

            Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Assembly callingAssembly = null;
            for (int i = 0; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                Assembly assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly != thisAssembly)
                {
                    callingAssembly = assembly;
                    break;
                }
            }

            if (callingAssembly != null)
            {
                Assembly(callingAssembly);
            }
        }

        public void AssemblyContainingType<T>()
        {
            AssemblyContainingType(typeof(T));
        }

        public void AssemblyContainingType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            Assembly assembly = type.Assembly;

            Assembly(assembly);
        }

        public void AssembliesFromPath(string path)
        {
            Guard.AssertNotEmpty(path, "path");

            AssembliesFromPath(path, a => true);
        }

        public void AssembliesFromPath(string path, Predicate<Assembly> assemblyFilter)
        {
            Guard.AssertNotEmpty(path, "path");
            Guard.AssertNotNull(assemblyFilter, "assemblyFilter");

            IEnumerable<string> assemblyPaths = Directory.GetFiles(path)
                .Where(file => Path.GetExtension(file).Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
                               Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase));

            foreach (string assemblyPath in assemblyPaths)
            {
                Assembly assembly = null;
                try
                {
                    assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
                }
                catch
                {
                    //intentionally left blank
                }

                if (assembly != null &&
                    assemblyFilter(assembly))
                {
                    Assembly(assembly);
                }
            }
        }

        public void AssembliesFromApplicationBaseDirectory()
        {
            AssembliesFromApplicationBaseDirectory(a => true);
        }

        public void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter)
        {
            Guard.AssertNotNull(assemblyFilter, "assemblyFilter");

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            AssembliesFromPath(baseDirectory, assemblyFilter);
            string binPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (Directory.Exists(binPath))
            {
                AssembliesFromPath(binPath, assemblyFilter);
            }
        }

        public void LookForRegistries()
        {
            Convention<FindRegistriesConvention>();
        }

        public FindAllTypesConvention AddAllTypesOf<TPlugin>()
        {
            return AddAllTypesOf(typeof(TPlugin));
        }

        public FindAllTypesConvention AddAllTypesOf(Type pluginType)
        {
            var filter = new FindAllTypesConvention(pluginType);
            With(filter);

            return filter;
        }

        public void IgnoreStructureMapAttributes()
        {
            throw new NotImplementedException();
        }

        public void Exclude(Func<Type, bool> exclude)
        {
            Guard.AssertNotNull(exclude, "exclude");

            _filter.Excludes += exclude;
        }

        public void ExcludeNamespace(string nameSpace)
        {
            Guard.AssertNotEmpty(nameSpace, "nameSpace");

            Exclude(type => type.IsInNamespace(nameSpace));
        }

        public void ExcludeNamespaceContainingType<T>()
        {
            ExcludeNamespace(typeof(T).Namespace);
        }

        public void Include(Func<Type, bool> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            _filter.Includes += predicate;
        }

        public void IncludeNamespace(string nameSpace)
        {
            Guard.AssertNotEmpty(nameSpace, "nameSpace");

            Include(type => type.IsInNamespace(nameSpace));
        }

        public void IncludeNamespaceContainingType<T>()
        {
            IncludeNamespace(typeof(T).Namespace);
        }

        public void ExcludeType<T>()
        {
            Exclude(type => type == typeof(T));
        }

        public void Convention<T>() where T : IRegistrationConvention, new()
        {
            IRegistrationConvention previous = _conventions.FirstOrDefault(scanner => scanner is T);
            if (previous == null)
            {
                With(new T());
            }
        }

        public void With(IRegistrationConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            _conventions.Fill(convention);
        }

        public ConfigureConventionExpression WithDefaultConventions()
        {
            throw new NotImplementedException();
        }

        public ConfigureConventionExpression ConnectImplementationsToTypesClosing(Type openGenericType)
        {
            Guard.AssertNotNull(openGenericType, "openGenericType");

            var convention = new GenericConnectionConvention(openGenericType);

            With(convention);

            return new ConfigureConventionExpression(convention);
        }

        public ConfigureConventionExpression RegisterConcreteTypesAgainstTheFirstInterface()
        {
            var convention = new FirstInterfaceConvention();

            With(convention);

            return new ConfigureConventionExpression(convention);
        }

        public ConfigureConventionExpression SingleImplementationsOfInterface()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string assemblyName)
        {
            foreach (Assembly assembly in _assemblies)
            {
                if (assembly.GetName().Name == assemblyName)
                {
                    return true;
                }
            }

            return false;
        }

        public void Configure(Registry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            var types = new List<Type>();

            foreach (Assembly assembly in _assemblies)
            {
                foreach (Type exportedType in assembly.GetExportedTypes())
                {
                    if (_filter.Matches(exportedType))
                        types.Fill(exportedType);
                }
            }

            foreach(Type type in types)
            {
                foreach (IRegistrationConvention convention in _conventions)
                {
                    convention.Process(type, registry);
                }
            }
        }
    }
}