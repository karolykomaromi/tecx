namespace Hydra.Infrastructure.Calendaring
{
    using System.Text;

    public class Reminder : CalendarItem<Reminder>
    {
        public Reminder()
        {
            this.Description = Properties.Resources.Reminder;

            this.Trigger = new DurationBuilder();

            this.Action = string.Empty;
        }

        public string Description { get; set; }

        public Duration Trigger { get; set; }

        public string Action { get; set; }

        public override Reminder Clone()
        {
            var clone = new Reminder
                {
                    Description = this.Description,
                    Trigger = this.Trigger.Clone(),
                    Action = this.Action
                };

            return clone;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);

            sb.AppendLine("BEGIN:VALARM");

            sb.Append("TRIGGER:").AppendLine(this.Trigger);

            sb.Append("ACTION:").AppendLine(this.Action);

            sb.Append("DESCRIPTION:").AppendLine(this.Description);

            sb.Append("END:VALARM");

            return sb.ToString();
        }
    }
}