namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class ClassWithPredefinedObjectCtorParameter
    {
        public IFoo Foo { get; set; }

        public ClassWithPredefinedObjectCtorParameter(IFoo foo)
        {
            this.Foo = foo;
        }
    }
}