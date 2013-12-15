namespace TecX.Agile.Modules.Main
{
    using System.Diagnostics;

    using TecX.Agile.Messaging;
    using TecX.CaliburnEx.Modularization;
    using TecX.Common;

    [DebuggerDisplay("{Description}")]
    public class MainModule : Module
    {
        private readonly MessageHub hub;

        public MainModule(MessageHub hub)
        {
            Guard.AssertNotNull(hub, "hub");

            this.hub = hub;
        }

        public override string Description
        {
            get { return "Main Module"; }
        }

        protected override void Initialize()
        {
        }
    }
}
