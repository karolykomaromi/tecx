namespace TecX.Unity.ContextualBinding
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Unity.Tracking;

    public interface IRequest
    {
        IBuilderContext Build { get; set; }

        BuildTreeNode CurrentBuildNode { get; }

        IDictionary<string, object> RequestContext { get; }
    }
}
