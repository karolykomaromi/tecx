namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class SemanticGroup : ISemanticGroup
    {
        private readonly ISemanticGroupContext context;

        private readonly string name;

        private SemanticGroupPolicy policy;

        public SemanticGroup(ISemanticGroupContext context, Type from, Type to, string name)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            this.context = context;
            this.name = name;

            this.policy = new SemanticGroupPolicy();

            context.Policies.Set(typeof(ISemanticGroupPolicy), this.policy, new NamedTypeBuildKey(to, name));

            context.Container.RegisterType(from, to, name);
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public ISemanticGroup Use(Type from, Type to, string name, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            // doesn't make sense to use default mapping with no name. this would be used anyway.
            Guard.AssertNotEmpty(name, "name");

            this.policy.Usings.Add(new Using { From = from, To = to, Name = name });

            this.context.Container.RegisterType(from, to, name, injectionMembers);

            return this;
        }
    }
}