namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class HasCtorWithParameterConvention
    {
        public IFoo Foo { get; set; }

        public HasCtorWithParameterConvention(IFoo foo)
        {
            this.Foo = foo;
        }
    }
}