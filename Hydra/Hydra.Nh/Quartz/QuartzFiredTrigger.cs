namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzFiredTrigger
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string ENTRY_ID { get; set; }

        public virtual string TRIGGER_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

        public virtual string INSTANCE_NAME { get; set; }

        public virtual long FIRED_TIME { get; set; }

        public virtual long SCHED_TIME { get; set; }

        public virtual int PRIORITY { get; set; }

        public virtual string STATE { get; set; }

        public virtual string JOB_NAME { get; set; }

        public virtual string JOB_GROUP { get; set; }

        public virtual bool IS_NONCONCURRENT { get; set; }

        public virtual bool REQUESTS_RECOVERY { get; set; }

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

            QuartzFiredTrigger other = obj as QuartzFiredTrigger;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.ENTRY_ID, other.ENTRY_ID, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.ENTRY_ID).GetHashCode();
        }
    }
}