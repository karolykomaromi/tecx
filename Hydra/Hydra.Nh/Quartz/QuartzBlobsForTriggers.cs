namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzBlobsForTriggers
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string TRIGGER_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

        public virtual byte[] BLOB_DATA { get; set; }

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

            QuartzBlobsForTriggers other = obj as QuartzBlobsForTriggers;

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