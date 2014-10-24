namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Text;

    public class Trigger : CalendarItem<Trigger>
    {
        public Trigger()
        {
            this.Absolute = DateTime.MinValue;

            this.Duration = new DurationBuilder();
        }

        public Duration Duration { get; set; }

        public DateTime Absolute { get; set; }

        public bool AfterEnd { get; set; }

        public override Trigger Clone()
        {
            var clone = new Trigger
                {
                    Duration = this.Duration.Clone(),
                    Absolute = this.Absolute,
                    AfterEnd = this.AfterEnd
                };

            return clone;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(50);

            sb.Append("TRIGGER");

            if (this.AfterEnd)
            {
                sb.Append(";RELATED=END:").Append(this.Duration);

                return sb.ToString();
            }

            if (this.Absolute > DateTime.MinValue)
            {
                sb.Append(";VALUE=DATE-TIME:").AppendFormat("{0:yyyyMMddTHHmmssZ}", this.Absolute);

                return sb.ToString();
            }

            sb.Append(":").Append(this.Duration);

            return sb.ToString();
        }
    }
}