namespace TecX.Agile.Modules.Main
{
    using System.Diagnostics;
    
    using TecX.CaliburnEx.Modularization;

    [DebuggerDisplay("{Description}")]
    public class MainModule : Module
    {
        public override string Description
        {
            get { return "Main Module"; }
        }

        protected override void Initialize()
        {
        }
    }
}
