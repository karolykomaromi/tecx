namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Text;

    public class TriggerBuilder : Builder<string>
    {
        private string duration;

        private DateTime absolute;

        private bool afterEnd;

        public TriggerBuilder()
        {
            this.absolute = DateTime.MinValue;
            this.duration = new DurationBuilder();
        }

        public TriggerBuilder FromDuration(Action<DurationBuilder> action)
        {
            DurationBuilder builder = new DurationBuilder();

            action(builder);

            this.duration = builder;

            return this;
        }

        public TriggerBuilder Absolute(DateTime absolute)
        {
            this.absolute = absolute;

            return this;
        }

        public override string Build()
        {
            StringBuilder sb = new StringBuilder(50);

            sb.Append("TRIGGER");

            if (this.afterEnd)
            {
                sb.Append(";RELATED=END:").Append(this.duration);

                return sb.ToString();
            }

            if (this.absolute > DateTime.MinValue)
            {
                sb.Append(";VALUE=DATE-TIME:").AppendFormat("{0:yyyyMMddTHHmmssZ}", this.absolute);

                return sb.ToString();
            }

            sb.Append(":").Append(this.duration);

            return sb.ToString();
        }

        public TriggerBuilder AfterEnd()
        {
            this.afterEnd = true;

            return this;
        }
    }
}