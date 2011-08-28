namespace TecX.Unity.Test.TestObjects
{
    public class Bar : IFoo
    {
        public string Name { get; set; }

        public Bar(string name)
        {
            Name = name;
        }
    }
}