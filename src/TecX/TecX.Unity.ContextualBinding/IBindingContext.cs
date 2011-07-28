namespace TecX.Unity.ContextualBinding
{
    public interface IBindingContext
    {
        object this[string key] { get; }

        void Put(string key, object value);
    }
}
