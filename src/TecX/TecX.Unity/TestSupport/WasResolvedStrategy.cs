namespace TecX.Unity.TestSupport
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    public class WasResolvedStrategy : BuilderStrategy
    {
        private readonly IList<NamedTypeBuildKey> buildKeys;

        public WasResolvedStrategy()
        {
            this.buildKeys = new List<NamedTypeBuildKey>();
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            this.buildKeys.Add(context.BuildKey);
        }

        public bool WasResolved<T>()
        {
            return this.WasResolved<T>(null);
        }

        public bool WasResolved<T>(string name)
        {
            var buildKey = this.buildKeys.FirstOrDefault(k => typeof(T).IsAssignableFrom(k.Type) && k.Name == name);

            return buildKey != null && buildKey.Type != null;
        }
    }
}