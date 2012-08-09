namespace TecX.Unity.Tracking
{
    using System;
    using System.Reflection;

    public interface ITarget
    {
        Type Type { get; }

        string Name { get; }

        MemberInfo Member { get; }
    }
}