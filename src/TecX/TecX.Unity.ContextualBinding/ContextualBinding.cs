namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ContextualBinding : InjectionMember
    {
        private readonly Predicate<IBindingContext, IBuilderContext> predicate;

        public ContextualBinding(Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(policies, "policies");

            var context = policies.Get<IContextPolicy>(Constants.ContextPolicyKey);

            if (context == null)
            {
                throw new InvalidOperationException("Can't find IContextPolicy. " +
                    "Did you forget to add the ContextualBindingExtension to your container instance?");
            }

            context.AddContextualMapping(serviceType, implementationType, this.predicate);
        }
    }

    public interface IContextPolicy : IBuilderPolicy
    {
        void AddContextualMapping(Type @from, Type to, Predicate<IBindingContext, IBuilderContext> predicate);
    }
}
