namespace TecX.Unity.ContextualBinding
{
    public interface IBindingContext
    {
        BuildTreeNode CurrentBuildNode { get; }

        object this[string key] { get; set; }
    }
}
