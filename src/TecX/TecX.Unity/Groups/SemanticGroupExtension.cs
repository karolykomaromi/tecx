namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class SemanticGroupExtension : UnityContainerExtension, ISemanticGroupConfigurator
    {
        public ISemanticGroup RegisterAsGroup(Type from, Type to, string name)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            return new SemanticGroup(new SemanticGroupContext(this), from, to, name);
        }

        protected override void Initialize()
        {
            var strategy = new SemanticGroupStrategy();

            Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }

        private class SemanticGroupContext : ISemanticGroupContext
        {
            private readonly SemanticGroupExtension semanticGroupExtension;

            public SemanticGroupContext(SemanticGroupExtension semanticGroupExtension)
            {
                this.semanticGroupExtension = semanticGroupExtension;
            }

            public IPolicyList Policies
            {
                get
                {
                    return this.semanticGroupExtension.Context.Policies;
                }
            }

            public IUnityContainer Container
            {
                get
                {
                    return this.semanticGroupExtension.Container;
                }
            }
        }
    }
}
