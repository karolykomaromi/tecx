using System.Diagnostics;

using TecX.Agile.Infrastructure;

namespace TecX.Agile.Modules.Main
{
    [DebuggerDisplay("{Description}")]
    public class MainModule : Module
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
