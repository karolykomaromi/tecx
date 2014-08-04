namespace Infrastructure.Server.Test.TestObjects
{
    internal class ClassWithInterfaceCtorParameter
    {
        public ClassWithInterfaceCtorParameter(IFoo foo)
        {
            this.Foo = foo;
        }
        
        public IFoo Foo { get; set; }
    }
}