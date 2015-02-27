namespace Hydra.Unity.Test.Decoration
{
    public class DecoratorForGenericInterface : IGenericInterface<int>
    {
        private readonly IGenericInterface<int> inner;

        public DecoratorForGenericInterface(IGenericInterface<int> inner)
        {
            this.inner = inner;
        }

        public int Bar { get; set; }

        public int Foo()
        {
            return this.inner.Foo() * 2;
        }
    }
}