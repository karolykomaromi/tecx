namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzSimpleTrigger
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string TRIGGER_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

        public virtual long REPEAT_COUNT { get; set; }

        public virtual long REPEAT_INTERVAL { get; set; }

        public virtual long TIMES_TRIGGERED { get; set; }

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

            QuartzSimpleTrigger other = obj as QuartzSimpleTrigger;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.TRIGGER_NAME, other.TRIGGER_NAME, StringComparison.Ordinal) &&
                string.Equals(this.TRIGGER_GROUP, other.TRIGGER_NAME, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.TRIGGER_NAME + "|" + this.TRIGGER_GROUP).GetHashCode();
        }
    }
}