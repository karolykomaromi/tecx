using Microsoft.Practices.Unity;

namespace TecX.Unity.Configuration
{
    public abstract class Registration : IContainerConfigurator
    {
        private string _name;

        public string Name
        {
            get { return _name; }
        }

        protected Registration(string name)
        {
            _name = name;
        }

        public abstract void Configure(IUnityContainer container);
    }
}
