namespace TecX.Unity.Literals
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Unity.Groups;

    public class LiteralParametersStrategy : BuilderStrategy
    {
        private readonly ResolverConventionCollection conventions;

        public LiteralParametersStrategy(ResolverConventionCollection conventions)
        {
            this.conventions = conventions;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.BuildKey, "context.BuildKey");
            Guard.AssertNotNull(context.BuildKey.Type, "context.BuildKey.Type");

            if (context.BuildKey.Type.IsPrimitive || context.BuildKey.Type == typeof(string))
            {
                return;
            }

            this.Constructors(context);

            this.Methods(context);

            this.Properties(context);
        }

        private void Properties(IBuilderContext context)
        {
            IPolicyList resolverPolicyDestination;
            IPropertySelectorPolicy propertySelector = context.Policies.Get<IPropertySelectorPolicy>(
                context.BuildKey, out resolverPolicyDestination);
            IEnumerable<SelectedProperty> selectedProperties;

            if (propertySelector != null
                && (selectedProperties = propertySelector.SelectProperties(context, resolverPolicyDestination)) != null)
            {
                selectedProperties = selectedProperties.ToArray();

                if (selectedProperties.Any())
                {
                    foreach (SelectedProperty property in selectedProperties)
                    {
                        DependencyInfo dependency = new DependencyInfo
                            { DependencyName = property.Property.Name, DependencyType = property.Property.PropertyType };

                        var resolver = this.conventions.CreateResolver(context, dependency);

                        if (resolver != null)
                        {
                            resolverPolicyDestination.Set<IDependencyResolverPolicy>(resolver, property.Key);
                        }
                    }

                    resolverPolicyDestination.Set<IPropertySelectorPolicy>(
                        new SelectedPropertiesCache(selectedProperties), context.BuildKey);
                }
            }
        }

        private void Methods(IBuilderContext context)
        {
            IPolicyList resolverPolicyDestination;
            IMethodSelectorPolicy methodSelector = context.Policies.Get<IMethodSelectorPolicy>(
                context.BuildKey, out resolverPolicyDestination);
            IEnumerable<SelectedMethod> selectedMethods;

            if (methodSelector != null
                && (selectedMethods = methodSelector.SelectMethods(context, resolverPolicyDestination)) != null)
            {
                selectedMethods = selectedMethods.ToArray();

                if (selectedMethods.Any())
                {
                    foreach (SelectedMethod method in selectedMethods)
                    {
                        var keys = method.GetParameterKeys();
                        var parameters = method.Method.GetParameters();

                        for (int i = 0; i < keys.Length; i++)
                        {
                            DependencyInfo dependency = new DependencyInfo
                                { DependencyName = parameters[i].Name, DependencyType = parameters[i].ParameterType };

                            var resolver = this.conventions.CreateResolver(context, dependency);

                            if (resolver != null)
                            {
                                resolverPolicyDestination.Set<IDependencyResolverPolicy>(resolver, keys[i]);
                            }
                        }
                    }

                    resolverPolicyDestination.Set<IMethodSelectorPolicy>(
                        new SelectedMethodsCache(selectedMethods), context.BuildKey);
                }
            }
        }

        private void Constructors(IBuilderContext context)
        {
            IPolicyList resolverPolicyDestination;

            IConstructorSelectorPolicy ctorSelector = context.Policies.Get<IConstructorSelectorPolicy>(
                context.BuildKey, out resolverPolicyDestination);
            SelectedConstructor selectedConstructor;

            if (ctorSelector != null
                && (selectedConstructor = ctorSelector.SelectConstructor(context, resolverPolicyDestination)) != null)
            {
                var keys = selectedConstructor.GetParameterKeys();
                var parameters = selectedConstructor.Constructor.GetParameters();

                for (int i = 0; i < keys.Length; i++)
                {
                    DependencyInfo dependency = new DependencyInfo
                        { DependencyName = parameters[i].Name, DependencyType = parameters[i].ParameterType };

                    var resolver = this.conventions.CreateResolver(context, dependency);

                    if (resolver != null)
                    {
                        resolverPolicyDestination.Set<IDependencyResolverPolicy>(resolver, keys[i]);
                    }
                }

                resolverPolicyDestination.Set<IConstructorSelectorPolicy>(
                    new SelectedConstructorCache(selectedConstructor), context.BuildKey);
            }
        }
    }
}