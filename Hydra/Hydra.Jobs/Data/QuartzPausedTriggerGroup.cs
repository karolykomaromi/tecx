namespace Hydra.Jobs.Data
{
    using System;

    public class QuartzPausedTriggerGroup
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

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

            QuartzPausedTriggerGroup other = obj as QuartzPausedTriggerGroup;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.TRIGGER_GROUP, other.TRIGGER_GROUP, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.TRIGGER_GROUP).GetHashCode();
        }
    }
}