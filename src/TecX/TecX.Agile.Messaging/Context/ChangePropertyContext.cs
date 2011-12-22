namespace TecX.Agile.Messaging.Context
{
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;

    public class ChangePropertyContext : InboundCommandContext<ChangeProperty>
    {
        public ChangePropertyContext(ChangeProperty command)
            : base(command)
        {
        }

        public override bool MatchesEvent(object outboundEvent)
        {
            Guard.AssertNotNull(outboundEvent, "outboundEvent");

            PropertyChanged pc = outboundEvent as PropertyChanged;

            if (pc != null)
            {
                bool matches = this.Command.ArtefactId == pc.ArtefactId && 
                    string.Equals(this.Command.PropertyName, pc.PropertyName) && 
                    Equals(this.Command.To, pc.After);

                return matches;
            }

            return false;
        }
    }
}
