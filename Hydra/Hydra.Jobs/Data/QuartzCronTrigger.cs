namespace Hydra.Jobs.Data
{
    using System;

    public class QuartzCronTrigger
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string TRIGGER_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

        public virtual string CRON_EXPRESSION { get; set; }

        public virtual string TIME_ZONE_ID { get; set; }

        public virtual QuartzTrigger Trigger { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            QuartzCronTrigger other = obj as QuartzCronTrigger;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.TRIGGER_NAME, other.TRIGGER_NAME, StringComparison.Ordinal) &&
                string.Equals(this.TRIGGER_GROUP, other.TRIGGER_GROUP, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.TRIGGER_NAME + "|" + this.TRIGGER_GROUP).GetHashCode();
        }
    }
}