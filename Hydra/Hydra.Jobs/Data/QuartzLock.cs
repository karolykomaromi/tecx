namespace Hydra.Jobs.Data
{
    using System;

    public class QuartzLock
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string LOCK_NAME { get; set; }

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

            QuartzLock other = obj as QuartzLock;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.LOCK_NAME, other.LOCK_NAME, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.LOCK_NAME).GetHashCode();
        }
    }
}