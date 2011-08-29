namespace TecX.Unity.ContextualBinding
{
    public interface IBindingContext
    {
        object this[string key] { get; set; }

        BuildTreeNode CurrentBuildNode { get; }
    }
}
