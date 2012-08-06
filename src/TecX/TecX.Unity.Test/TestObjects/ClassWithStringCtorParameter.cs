namespace TecX.Unity.Test.TestObjects
{
    public class ClassWithStringCtorParameter
    {
        public string SomeString { get; set; }

        public ClassWithStringCtorParameter(string someString)
        {
            this.SomeString = someString;
        }
    }
}