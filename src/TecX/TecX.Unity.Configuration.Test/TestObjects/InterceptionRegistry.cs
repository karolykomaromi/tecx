using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace TecX.Unity.Configuration.Test.TestObjects
{
    internal class InterceptionRegistry : Registry
    {
        public InterceptionRegistry()
        {
            RegisterAction(() => Container.AddNewExtension<Interception>());
        }
    }
}