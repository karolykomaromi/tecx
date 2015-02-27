namespace Hydra.Unity.Decoration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class DecoratorStrategy : BuilderStrategy, IObserver<EventPattern<RegisterEventArgs>>
    {
        private readonly DecoratorRegistrationsCollection decoratorRegistrationsCollection;

        public DecoratorStrategy()
        {
            this.decoratorRegistrationsCollection = new DecoratorRegistrationsCollection();
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            Contract.Requires(context != null);

            var key = context.OriginalBuildKey;

            if (this.IsRequestForDecoratedInterface(key) && ParameterForRequestedTypeIsNotOverriden(context, key))
            {
                ICollection<Type> decorators = this.decoratorRegistrationsCollection[key];

                object value = null;

                foreach (Type type in decorators)
                {
                    value = context.NewBuildUp(new NamedTypeBuildKey(type, key.Name));
                    DependencyOverride @override = new DependencyOverride(key.Type, value);
                    context.AddResolverOverrides(@override);
                }

                context.Existing = value;
                context.BuildComplete = true;
            }
        }

        public void OnNext(EventPattern<RegisterEventArgs> value)
        {
            NamedTypeBuildKey key = new NamedTypeBuildKey(value.EventArgs.TypeFrom, value.EventArgs.Name);

            ICollection<Type> stack = this.decoratorRegistrationsCollection[key];

            stack.Add(value.EventArgs.TypeTo);
        }

        public void OnError(Exception error)
        {
            HydraEventSource.Log.Error(error);
        }

        public void OnCompleted()
        {
        }

        private static bool ParameterForRequestedTypeIsNotOverriden(IBuilderContext context, NamedTypeBuildKey key)
        {
            return context.GetOverriddenResolver(key.Type) == null;
        }

        private bool IsRequestForDecoratedInterface(NamedTypeBuildKey key)
        {
            return key.Type.IsInterface && this.decoratorRegistrationsCollection.IsDecorated(key);
        }
    }
}