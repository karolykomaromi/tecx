namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class SemanticGroup : ISemanticGroup
    {
        private readonly ISemanticGroupContext context;

        private readonly Type to;

        private readonly string name;

        private readonly Type @from;

        private SemanticGroupPolicy policy;

        public SemanticGroup(ISemanticGroupContext context, Type from, Type to, string name)
        {
            this.context = context;
            this.to = to;
            this.name = name;

            this.policy = new SemanticGroupPolicy();

            context.Policies.Set(typeof(ISemanticGroupPolicy), policy, new NamedTypeBuildKey(to, name));

            context.Container.RegisterType(from, to, name);
        }

        public ISemanticGroup Use<TFrom, TTo>()
        {
            policy.Usings.Add(new Using { From = typeof(TFrom), To = typeof(TTo), Name = this.name });

            context.Container.RegisterType(typeof(TFrom), typeof(TTo), this.name);

            return this;
        }
    }
}