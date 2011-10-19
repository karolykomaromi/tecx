using System.Diagnostics;

using TecX.Agile.Infrastructure;

namespace TecX.Agile.Modules.Main
{
    [DebuggerDisplay("{Description}")]
    public class Module : ModuleBase
    {
        public override string Description
        {
            get { return "Main Module"; }
        }

        public void Initialize()
        {
        }
    }
}
