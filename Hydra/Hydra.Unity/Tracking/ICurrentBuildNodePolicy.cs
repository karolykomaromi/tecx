using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;

namespace Hydra.Unity.Tracking
{
    public interface ICurrentBuildNodePolicy : IBuilderPolicy
    {
        Stack<string> Tags { get; }
    }
}