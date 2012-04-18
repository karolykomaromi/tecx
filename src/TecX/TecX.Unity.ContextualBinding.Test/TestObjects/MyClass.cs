namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    public class MyClass : IMyInterface
    {
        private readonly string str;

        public MyClass()
            : this("parameterless c'tor")
        {
        }

        public MyClass(string str)
        {
            this.str = str;
        }

        public string Str
        {
            get { return this.str; }
        }
    }
}