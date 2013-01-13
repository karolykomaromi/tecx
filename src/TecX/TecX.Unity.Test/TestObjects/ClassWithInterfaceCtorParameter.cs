namespace TecX.Unity.Test.TestObjects
{
    public class ClassWithInterfaceCtorParameter
    {
        public IFoo Foo { get; set; }

        public ClassWithInterfaceCtorParameter(IFoo foo)
        {
            this.Foo = foo;
        }
    }
}