namespace TecX.Unity.Configuration.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;
    using TecX.Common.Extensions.Collections;
    using TecX.Unity.Configuration.Extensions;
    using TecX.Unity.Configuration.Utilities;

    public class AssemblyScanner
    {
        private readonly List<Assembly> assemblies;

        private readonly CompositeFilter<Type> filter;

        private readonly List<IRegistrationConvention> conventions;

        public AssemblyScanner()
        {
            this.assemblies = new List<Assembly>();
            this.filter = new CompositeFilter<Type>();
            this.conventions = new List<IRegistrationConvention>();
        }

        #region Select assemblies to scan

        public void Assembly(Assembly assembly)
        {
            Guard.AssertNotNull(assembly, "assembly");

            if (!this.assemblies.Contains(assembly))
            {
                this.assemblies.Add(assembly);
            }
        }

        public void Assembly(string assemblyName)
        {
            Guard.AssertNotEmpty(assemblyName, "assemblyName");

            this.Assembly(AppDomain.CurrentDomain.Load(assemblyName));
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
                this.Assembly(callingAssembly);
            }
        }

        public void AssemblyContainingType<T>()
        {
            this.AssemblyContainingType(typeof(T));
        }

        public void AssemblyContainingType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            Assembly assembly = type.Assembly;

            this.Assembly(assembly);
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
                .Where(file => string.Equals(Path.GetExtension(file), ".exe", StringComparison.OrdinalIgnoreCase) ||
                               string.Equals(Path.GetExtension(file), ".dll", StringComparison.OrdinalIgnoreCase));

            foreach (string assemblyPath in assemblyPaths)
            {
                Assembly assembly = null;
                try
                {
                    assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
                }
                catch (Exception)
                {
                    // intentionally left blank
                }

                if (assembly != null &&
                    assemblyFilter(assembly))
                {
                    this.Assembly(assembly);
                }
            }
        }

        public void AssembliesFromApplicationBaseDirectory()
        {
            this.AssembliesFromApplicationBaseDirectory(a => true);
        }

        public void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter)
        {
            Guard.AssertNotNull(assemblyFilter, "assemblyFilter");

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            this.AssembliesFromPath(baseDirectory, assemblyFilter);
            string binPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (Directory.Exists(binPath))
            {
                this.AssembliesFromPath(binPath, assemblyFilter);
            }
        }

        public bool Contains(string assemblyName)
        {
            Guard.AssertNotEmpty(assemblyName, "assemblyName");

            foreach (Assembly assembly in this.assemblies)
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

        public void Exclude(Predicate<Type> exclude)
        {
            Guard.AssertNotNull(exclude, "exclude");

            this.filter.Excludes += exclude;
        }

        public void ExcludeNamespace(string nameSpace)
        {
            Guard.AssertNotEmpty(nameSpace, "nameSpace");

            this.Exclude(type => type.IsInNamespace(nameSpace));
        }

        public void ExcludeNamespaceContainingType<T>()
        {
            this.ExcludeNamespace(typeof(T).Namespace);
        }

        public void Include(Predicate<Type> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.filter.Includes += predicate;
        }

        public void IncludeNamespace(string nameSpace)
        {
            Guard.AssertNotEmpty(nameSpace, "nameSpace");

            this.Include(type => type.IsInNamespace(nameSpace));
        }

        public void IncludeNamespaceContainingType<T>()
        {
            this.IncludeNamespace(typeof(T).Namespace);
        }

        public void ExcludeType<T>()
        {
            this.Exclude(type => type == typeof(T));
        }

        #endregion Filtering

        #region Conventions

        public void LookForConfigBuilders()
        {
            this.With<FindConfigBuildersConvention>();
        }

        public FindAllImplementationsConvention AddAllImplementationsOf<TPlugin>()
        {
            return this.AddAllImplementationsOf(typeof(TPlugin));
        }

        public FindAllImplementationsConvention AddAllImplementationsOf(Type pluginType)
        {
            Guard.AssertNotNull(pluginType, "pluginType");

            var convention = new FindAllImplementationsConvention(pluginType);

            this.With(convention);

            return convention;
        }

        public void With<T>() where T : IRegistrationConvention, new()
        {
            IRegistrationConvention existing = this.conventions.FirstOrDefault(convention => convention is T);

            if (existing == null)
            {
                this.With(new T());
            }
        }

        public void With(IRegistrationConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            this.conventions.Fill(convention);
        }

        public void WithDefaultConventions()
        {
            ImplementsIInterfaceNameConvention convention = new ImplementsIInterfaceNameConvention();

            this.With(convention);
        }

        public void RegisterConcreteTypesAgainstTheFirstInterface()
        {
            FirstInterfaceConvention convention = new FirstInterfaceConvention();

            this.With(convention);
        }

        public void SingleImplementationsOfInterface()
        {
            SingleImplementationOfInterfaceConvention convention = new SingleImplementationOfInterfaceConvention();

            this.With(convention);
        }

        #endregion Conventions

        internal void ScanForAll(Configuration config)
        {
            Guard.AssertNotNull(config, "config");

            ConfigurationBuilder builder = new ConfigurationBuilder();

            // we iterate over all assemblies that were added to this scanner
            // we run the exported types from each of these assemblies through the scanners filter
            // the types that get past the filters are processed by the registered conventions
            // the conventions take care about registering a type with the container if it fits their scheme
            config
                .Types
                .For(this.assemblies, this.filter)
                .Each(type => this.conventions.Each(c => c.Process(type, builder)));

            builder.BuildUp(config);
        }
    }
}