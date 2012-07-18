namespace TecX.Unity.Configuration
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Collections;
    using TecX.Unity.Configuration.Builders;
    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.ContextualBinding;
    using TecX.Unity.Decoration;
    using TecX.Unity.Enrichment;
    using TecX.Unity.Groups;
    using TecX.Unity.Literals;
    using TecX.Unity.Mapping;
    using TecX.Unity.Proxies;

    public class ConfigurationBuilder : UnityContainerExtension
    {
        private readonly List<Action<Configuration>> alternations;

        public ConfigurationBuilder()
        {
            this.alternations = new List<Action<Configuration>>();
        }

        public static bool IsPublicBuilder(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.Assembly == typeof(ConfigurationBuilder).Assembly)
            {
                return false;
            }

            if (!typeof(ConfigurationBuilder).IsAssignableFrom(type))
            {
                return false;
            }

            if (type.IsInterface ||
                type.IsAbstract ||
                type.IsGenericType)
            {
                return false;
            }

            return type.GetConstructor(new Type[0]) != null;
        }

        public RegistrationFamilyBuilder For<T>()
        {
            return this.For(typeof(T));
        }

        public RegistrationFamilyBuilder For(Type from)
        {
            Guard.AssertNotNull(from, "from");

            return new RegistrationFamilyBuilder(from, this);
        }

        public RegistrationFamilyBuilder ForConcreteType<T>()
            where T : class
        {
            return this.ForConcreteType(typeof(T));
        }

        public RegistrationFamilyBuilder ForConcreteType(Type concreteType)
        {
            Guard.AssertNotNull(concreteType, "concreteType");
            Guard.AssertCondition(!concreteType.IsInterface, concreteType, "concreteType", "concreteType must not be an interface.");
            Guard.AssertCondition(!concreteType.IsAbstract, concreteType, "concreteType", "concreteType must not be abstract.");

            var expression = new RegistrationFamilyBuilder(concreteType, this);

            expression.Use(concreteType);

            return expression;
        }

        public ContainerExtensionBuilder<TExtension> Extension<TExtension>()
            where TExtension : UnityContainerExtension, new()
        {
            return this.Extension(new TExtension());
        }

        public ContainerExtensionBuilder<TExtension> Extension<TExtension>(TExtension extension)
            where TExtension : UnityContainerExtension
        {
            Guard.AssertNotNull(extension, "extension");

            return new ContainerExtensionBuilder<TExtension>(this, extension);
        }

        public ConfigurationBuilder WithDefaultExtensions()
        {
            this.AddExpression(config => config.AddExtension(new DecoratorExtension()));
            this.AddExpression(config => config.AddExtension(new DefaultMappingExtension()));
            this.AddExpression(config => config.AddExtension(new ContextualBindingExtension()));
            this.AddExpression(config => config.AddExtension(new CollectionResolutionExtension()));
            this.AddExpression(config => config.AddExtension(new EnrichmentExtension()));
            this.AddExpression(config => config.AddExtension(new MappingGroupExtension()));
            this.AddExpression(config => config.AddExtension(new LiteralParametersExtension()));
            this.AddExpression(config => config.AddExtension(new ProxyGenerator()));

            return this;
        }

        public void ImportBuilder<T>()
            where T : ConfigurationBuilder, new()
        {
            this.ImportBuilder(new T());
        }

        public void ImportBuilder(ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            this.alternations.Add(builder.BuildUp);
        }

        public void Scan(Action<AssemblyScanner> action)
        {
            Guard.AssertNotNull(action, "action");

            var scanner = new AssemblyScanner();

            action(scanner);

            this.alternations.Add(graph => graph.AddScanner(scanner));
        }

        #region Infrastructure

        internal void BuildUp(Configuration config)
        {
            Guard.AssertNotNull(config, "config");

            if (config.Builders.Contains(this))
            {
                return;
            }

            this.alternations.ForEach(action => action(config));

            config.Builders.Add(this);
        }

        protected internal void AddExpression(Action<Configuration> alteration)
        {
            Guard.AssertNotNull(alteration, "alteration");

            this.alternations.Add(alteration);
        }

        protected sealed override void Initialize()
        {
            this.PreBuildUp();

            Configuration graph = new Configuration();

            this.BuildUp(graph);

            graph.Configure(this.Container);

            this.PostBuildUp();
        }

        protected virtual void PreBuildUp()
        {
        }

        protected virtual void PostBuildUp()
        {
        }

        #endregion Infrastructure
    }
}
