using TecX.Agile.Infrastructure;

namespace TecX.Agile.Modules.Main
{
    public class Module : IModule
    {
        public string Description
        {
            get { return "Main Module"; }
        }

        public void Initialize()
        {
        }
    }
}
