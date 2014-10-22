namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzCalendar
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string CALENDAR_NAME { get; set; }

        public virtual byte[] CALENDAR { get; set; }

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

            QuartzCalendar other = obj as QuartzCalendar;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.SCHED_NAME, other.SCHED_NAME, StringComparison.Ordinal) &&
                string.Equals(this.CALENDAR_NAME, other.CALENDAR_NAME, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (this.SCHED_NAME + "|" + this.CALENDAR_NAME).GetHashCode();
        }
    }
}