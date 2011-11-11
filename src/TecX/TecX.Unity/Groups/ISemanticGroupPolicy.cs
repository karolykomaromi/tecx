namespace TecX.Unity.Groups
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    public interface ISemanticGroupPolicy : IBuilderPolicy
    {
        ICollection<ScopedMapping> ScopedMappings { get; }
    }
}