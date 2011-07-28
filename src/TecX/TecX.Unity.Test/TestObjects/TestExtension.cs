using Microsoft.Practices.Unity;

namespace TecX.Unity.Test.TestObjects
{
    public class TestExtension : UnityContainerExtension, ITestExtensionConfig
    {
        protected override void Initialize()
        {
            Prop1 = false;

            Container.RegisterInstance<TestExtension>(this);
        }

        public bool Prop1 { get; set; }
    }
}