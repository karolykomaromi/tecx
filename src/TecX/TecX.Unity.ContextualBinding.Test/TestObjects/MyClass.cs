namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    public class MyClass : IMyInterface
    {
        private readonly string _str;

        public MyClass()
            : this("parameterless c'tor")
        {
        }

        public MyClass(string str)
        {
            _str = str;
        }

        public string Str
        {
            get { return _str; }
        }
    }
}