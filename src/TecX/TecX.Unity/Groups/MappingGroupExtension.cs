namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class MappingGroupExtension : UnityContainerExtension, IGroupedMappings
    {
        public void AddPolicy(IMappingGroupPolicy policy, Type type, string name)
        {
            Guard.AssertNotNull(policy, "policy");
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotEmpty(name, "name");

            this.Context.Policies.Set<IMappingGroupPolicy>(policy, new NamedTypeBuildKey(type, name));
        }

        protected override void Initialize()
        {
            var strategy = new MappingGroupStrategy();

            Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}
