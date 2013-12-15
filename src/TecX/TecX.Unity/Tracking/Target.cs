namespace TecX.Unity.Tracking
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using TecX.Common;

    [DebuggerDisplay("Name:'{Name}' Type:'{Type}'")]
    public abstract class Target<T> : ITarget
        where T : ICustomAttributeProvider
    {
        protected Target(MemberInfo member, T site)
        {
            Guard.AssertNotNull(member, "member");
            Guard.AssertNotNull(site, "site");

            this.Member = member;
            this.Site = site;
        }

        public MemberInfo Member { get; private set; }

        public T Site { get; private set; }

        public abstract string Name { get; }

        public abstract Type Type { get; }
    }
}