namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzSchedulerState
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string INSTANCE_NAME { get; set; }

        public virtual long LAST_CHECKIN_TIME { get; set; }

        public virtual long CHECKIN_INTERVAL { get; set; }

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

            QuartzSchedulerState other = obj as QuartzSchedulerState;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.INSTANCE_NAME, other.INSTANCE_NAME, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.INSTANCE_NAME).GetHashCode();
        }
    }
}