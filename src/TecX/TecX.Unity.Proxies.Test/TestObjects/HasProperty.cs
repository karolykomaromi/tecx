namespace TecX.Unity.Proxies.Test.TestObjects
{
    public class HasProperty : IHaveProperty
    {
        public HasProperty()
        {
            this.MyProperty = "1";
        }

        public string MyProperty { get; set; }
    }
}