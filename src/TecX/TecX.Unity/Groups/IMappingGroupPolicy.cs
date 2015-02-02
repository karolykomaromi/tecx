namespace TecX.Unity.Groups
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IMappingGroupPolicy : IBuilderPolicy
    {
        ICollection<MappingInfo> MappingInfos { get; }
    }
}