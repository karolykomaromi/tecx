namespace Hydra.Unity.Test.Decoration
{
    public interface IGenericInterface<out T>
    {
        T Foo();
    }
}