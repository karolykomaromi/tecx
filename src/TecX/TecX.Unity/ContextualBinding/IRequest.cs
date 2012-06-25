namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Unity.Tracking;

    public interface IRequest
    {
        IBuilderContext Build { get; set; }

        BuildTreeNode CurrentBuildNode { get; }

        object this[string key] { get; set; }
    }
}
