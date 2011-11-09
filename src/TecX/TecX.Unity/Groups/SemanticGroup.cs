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

        public ISemanticGroup Use<TFrom, TTo>()
        {
            this.policy.Usings.Add(new Using { From = typeof(TFrom), To = typeof(TTo), Name = this.name });

            this.context.Container.RegisterType(typeof(TFrom), typeof(TTo), this.name);

            return this;
        }
    }
}