namespace Hydra.Jobs.Data
{
    using System;

    public class QuartzTrigger
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string TRIGGER_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

        public virtual string JOB_NAME { get; set; }

        public virtual string JOB_GROUP { get; set; }

        public virtual string DESCRIPTION { get; set; }

        public virtual long NEXT_FIRE_TIME { get; set; }

        public virtual long PREV_FIRE_TIME { get; set; }

        public virtual int PRIORITY { get; set; }

        public virtual string TRIGGER_STATE { get; set; }

        public virtual string TRIGGER_TYPE { get; set; }

        public virtual long START_TIME { get; set; }

        public virtual long END_TIME { get; set; }

        public virtual string CALENDAR_NAME { get; set; }

        public virtual short MISFIRE_INSTR { get; set; }

        public virtual byte[] JOB_DATA { get; set; }

        public virtual QuartzJobDetails JobDetails { get; set; }

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

            QuartzTrigger other = obj as QuartzTrigger;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.JOB_NAME, other.JOB_NAME, StringComparison.Ordinal) &&
                string.Equals(this.JOB_GROUP, other.JOB_GROUP, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.JOB_NAME + "|" + this.JOB_GROUP).GetHashCode();
        }
    }
}