namespace TecX.Unity.ContextualBinding
{
    using TecX.Unity.Tracking;

    public interface IBindingContext
    {
        BuildTreeNode CurrentBuildNode { get; }

        object this[string key] { get; set; }
    }
}
