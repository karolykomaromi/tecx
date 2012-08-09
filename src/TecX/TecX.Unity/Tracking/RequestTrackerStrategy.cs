namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    public class RequestTrackerStrategy : BuilderStrategy
    {
        [ThreadStatic]
        private static IRequest request;

        public override void PreBuildUp(IBuilderContext context)
        {
            IPolicyList destination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out destination);

            IEnumerable<ITarget> targets = new ITarget[0];

            if (selector != null)
            {
                SelectedConstructor selectedConstructor = selector.SelectConstructor(context, destination);

                ConstructorInfo constructor = selectedConstructor.Constructor;

                targets = targets.Concat(constructor.GetParameters().Select(p => new ParameterTarget(constructor, p)));

                request = request != null ? request.CreateChild(context.BuildKey.Type, context, targets) : new Request(context.BuildKey.Type, context, targets);

                destination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
            }

            IPropertySelectorPolicy propertySelector = context.Policies.Get<IPropertySelectorPolicy>(context.BuildKey, out destination);

            if (propertySelector != null)
            {
                var selectedProperties = propertySelector.SelectProperties(context, destination).ToArray();

                targets = targets.Concat(selectedProperties.Select(p => new PropertyTarget(p.Property)));

                destination.Set<IPropertySelectorPolicy>(new SelectedPropertiesCache(selectedProperties), context.BuildKey);
            }

            IMethodSelectorPolicy methodSelector = context.Policies.Get<IMethodSelectorPolicy>(context.BuildKey, out destination);

            if (methodSelector != null)
            {
                var selectedMethods = methodSelector.SelectMethods(context, destination).ToArray();

                foreach (var selectedMethod in selectedMethods)
                {
                    MethodInfo method = selectedMethod.Method;

                    targets = targets.Concat(method.GetParameters().Select(p => new ParameterTarget(method, p)));
                }

                destination.Set<IMethodSelectorPolicy>(new SelectedMethodsCache(selectedMethods), context.BuildKey);
            }
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            request = request.ParentRequest;
        }
    }
}