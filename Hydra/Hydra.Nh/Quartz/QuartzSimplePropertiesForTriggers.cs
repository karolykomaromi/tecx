namespace Hydra.Nh.Quartz
{
    using System;

    public class QuartzSimplePropertiesForTriggers
    {
        public virtual string SCHED_NAME { get; set; }

        public virtual string TRIGGER_NAME { get; set; }

        public virtual string TRIGGER_GROUP { get; set; }

        public virtual string STR_PROP_1 { get; set; }

        public virtual string STR_PROP_2 { get; set; }

        public virtual string STR_PROP_3 { get; set; }

        public virtual int INT_PROP_1 { get; set; }

        public virtual int INT_PROP_2 { get; set; }

        public virtual long LONG_PROP_1 { get; set; }

        public virtual long LONG_PROP_2 { get; set; }

        public virtual decimal DEC_PROP_1 { get; set; }

        public virtual decimal DEC_PROP_2 { get; set; }

        public virtual bool BOOL_PROP_1 { get; set; }

        public virtual bool BOOL_PROP_2 { get; set; }

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

            QuartzSimplePropertiesForTriggers other = obj as QuartzSimplePropertiesForTriggers;

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