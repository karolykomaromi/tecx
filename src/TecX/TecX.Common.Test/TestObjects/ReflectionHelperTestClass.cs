namespace TecX.Common.Test.TestObjects
{
    internal class ReflectionHelperTestClass
    {
        public string TestProperty { get; set; }
            

        public string TestMethod<T>()
        {
            return "TestMethodWithoutParameter";
        }

        public string TestMethod<T>(T first, string second)
        {
            return "OverloadedTestMethodWithString_" + second;
        }

        public string TestMethod<T>(T first, int second)
        {
            return "OverloadedTestMethodWithInt" + second;
        }
    }
}