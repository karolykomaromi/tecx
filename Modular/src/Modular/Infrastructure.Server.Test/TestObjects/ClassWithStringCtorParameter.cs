namespace Infrastructure.Server.Test.TestObjects
{
    internal class ClassWithStringCtorParameter
    {
        public ClassWithStringCtorParameter(string someString)
        {
            this.SomeString = someString;
        }

        public string SomeString { get; set; }
    }
}