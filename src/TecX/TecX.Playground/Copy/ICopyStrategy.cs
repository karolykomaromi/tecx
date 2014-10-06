namespace TecX.Playground.Copy
{
    public interface ICopyStrategy
    {
    }

    public interface ICopyStrategy<in T> : ICopyStrategy
        where T : A
    {
        void CopyValues(T original, T copy, CopyContext ctx);
    }
}