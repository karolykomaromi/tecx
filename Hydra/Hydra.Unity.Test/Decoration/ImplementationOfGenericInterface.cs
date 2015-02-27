namespace Hydra.Unity.Test.Decoration
{
    public class ImplementationOfGenericInterface : IGenericInterface<int>
    {
        public int Foo()
        {
            return 42;
        }
    }
}