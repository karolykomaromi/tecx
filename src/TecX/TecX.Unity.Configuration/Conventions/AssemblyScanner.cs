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
        #region Fields

        private readonly List<Assembly> _assemblies;
        private readonly CompositeFilter<Type> _filter;
        private readonly List<IRegistrationConvention> _conventions;
        private readonly List<Action<RegistrationGraph>> _postScanningActions;

        #endregion Fields

        #region c'tor

        public AssemblyScanner()
        {
            _assemblies = new List<Assembly>();
            _filter = new CompositeFilter<Type>();
            _conventions = new List<IRegistrationConvention>();
            _postScanningActions = new List<Action<RegistrationGraph>>();
        }

        #endregion c'tor

        #region Select assemblies to scan

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

        public bool Contains(string assemblyName)
        {
            Guard.AssertNotEmpty(assemblyName, "assemblyName");

            foreach (Assembly assembly in _assemblies)
            {
                if (assembly.GetName().Name == assemblyName)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Select assemblies to scan

        #region Filtering

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

        #endregion Filtering

        #region Conventions

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
            Guard.AssertNotNull(pluginType, "pluginType");

            var filter = new FindAllTypesConvention(pluginType);
            With(filter);

            return filter;
        }

        public void Convention<T>()
            where T : IRegistrationConvention, new()
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

            var modifyGraph = convention as IRegistrationConventionWithPostScanningAction;

            if(modifyGraph != null)
            {
                ModifyGraphAfterScan(modifyGraph.PostScanningAction);
            }
        }

        public void WithDefaultConventions()
        {
            ImplementsIInterfaceNameConvention convention = new ImplementsIInterfaceNameConvention();

            With(convention);
        }

        public void RegisterConcreteTypesAgainstTheFirstInterface()
        {
            FirstInterfaceConvention convention = new FirstInterfaceConvention();

            With(convention);
        }

        public void SingleImplementationsOfInterface()
        {
            SingleImplementationOfInterfaceConvention convention = new SingleImplementationOfInterfaceConvention();

            With(convention);
        }

        #endregion Conventions

        #region Infrastructure

        internal void ScanForAll(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            Registry registry = new Registry();

            //we iterate over all assemblies that were added to this scanner
            //we run the exported types from each of these assemblies through the scanners filter
            //the types that get past the filters are processed by the registered conventions
            //the conventions take care about registering a type with the container if it fits their scheme
            graph
                .Types
                .For(_assemblies, _filter)
                .Each(type => _conventions.Each(c => c.Process(type, registry)));

            registry.ConfigureRegistrationGraph(graph);

            _postScanningActions.Each(action => action(graph));
        }

        public void ModifyGraphAfterScan(Action<RegistrationGraph> modifyGraph)
        {
            Guard.AssertNotNull(modifyGraph, "modifyGraph");

            _postScanningActions.Add(modifyGraph);
        }

        #endregion Infrastructure
    }
}