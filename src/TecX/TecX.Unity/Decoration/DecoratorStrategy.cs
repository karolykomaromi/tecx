namespace TecX.Unity.Decoration
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class DecoratorStrategy : BuilderStrategy
    {
        private readonly RegistrationStack registrationStack;

        public DecoratorStrategy(RegistrationStack registrationStack)
        {
            Guard.AssertNotNull(registrationStack, "registrationStack");

            this.registrationStack = registrationStack;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            var key = context.OriginalBuildKey;

            if (!key.Type.IsInterface)
            {
                return;
            }

            if (!this.registrationStack.IsRegistered(key.Type))
            {
                return;
            }

            if (context.GetOverriddenResolver(key.Type) != null)
            {
                return;
            }

            Stack<Type> stack = new Stack<Type>(this.registrationStack[key.Type]);

            object value = null;
            stack.ForEach(
                type =>
                {
                    value = context.NewBuildUp(new NamedTypeBuildKey(type, key.Name));
                    var overrides = new DependencyOverride(key.Type, value);
                    context.AddResolverOverrides(overrides);
                });

            context.Existing = value;
            context.BuildComplete = true;
        }
    }
}