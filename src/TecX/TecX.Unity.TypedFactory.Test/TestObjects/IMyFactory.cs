namespace TecX.Unity.TypedFactory.Test.TestObjects
{
    public interface IMyFactory
    {
        IFoo Create();
        IFoo Create(string name);
    }
}