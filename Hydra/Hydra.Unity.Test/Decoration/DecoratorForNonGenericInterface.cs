namespace Hydra.Unity.Test.Decoration
{
    public class DecoratorForNonGenericInterface : INonGenericInterface
    {
        private readonly INonGenericInterface inner;

        public DecoratorForNonGenericInterface(INonGenericInterface inner)
        {
            this.inner = inner;
        }

        public int Foo()
        {
            return this.inner.Foo() * 2;
        }
    }
}