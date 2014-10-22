namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzJobDetails
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string JOB_NAME { get; set; }

        public virtual string JOB_GROUP { get; set; }

        public virtual string DESCRIPTION { get; set; }

        public virtual string JOB_CLASS_NAME { get; set; }

        public virtual bool IS_DURABLE { get; set; }

        public virtual bool IS_NONCONCURRENT { get; set; }

        public virtual bool IS_UPDATE_DATA { get; set; }

        public virtual bool REQUESTS_RECOVERY { get; set; }

        public virtual byte[] JOB_DATA { get; set; }

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

            QuartzJobDetails other = obj as QuartzJobDetails;

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