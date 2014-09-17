namespace Hydra.Unity.Tracking
{
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;

    public interface ICurrentBuildNodePolicy : IBuilderPolicy
    {
        Stack<string> Tags { get; }
    }
}