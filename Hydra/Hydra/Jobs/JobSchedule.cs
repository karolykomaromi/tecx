namespace Hydra.Jobs
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Hydra.Infrastructure;
    using Quartz;

    [TypeConverter(typeof(EnumerationConverter<JobSchedule>))]
    public abstract class JobSchedule : Enumeration<JobSchedule>
    {
        public static readonly JobSchedule Simple = new SimpleJobSchedule();

        public static readonly JobSchedule DailyInterval = new DailyIntervalJobSchedule();

        protected JobSchedule(string name, int key)
            : base(name, key)
        {
        }

        public abstract void WithSchedule(TriggerBuilder triggerBuilder, int interval, IntervalUnit intervalUnit);

        private class SimpleJobSchedule : JobSchedule
        {
            public SimpleJobSchedule([CallerMemberName]string name = "", [CallerLineNumber]int key = -1)
                : base(name, key)
            {
            }

            public override void WithSchedule(TriggerBuilder triggerBuilder, int interval, IntervalUnit intervalUnit)
            {
                triggerBuilder.WithSimpleSchedule(s =>
                    {
                        switch (intervalUnit)
                        {
                            case IntervalUnit.Second:
                                s.WithIntervalInSeconds(interval);
                                break;
                            case IntervalUnit.Minute:
                                s.WithIntervalInMinutes(interval);
                                break;
                            case IntervalUnit.Hour:
                                s.WithIntervalInHours(interval);
                                break;
                            case IntervalUnit.Millisecond:
                            case IntervalUnit.Day:
                            case IntervalUnit.Week:
                            case IntervalUnit.Month:
                            case IntervalUnit.Year:
                            default:
                                throw new ArgumentOutOfRangeException("intervalUnit");
                        }
                    });
            }
        }

        private class DailyIntervalJobSchedule : JobSchedule
        {
            public DailyIntervalJobSchedule([CallerMemberName]string name = "", [CallerLineNumber]int key = -1)
                : base(name, key)
            {
            }

            public override void WithSchedule(TriggerBuilder triggerBuilder, int interval, IntervalUnit intervalUnit)
            {
                triggerBuilder.WithDailyTimeIntervalSchedule(s => s.WithInterval(interval, intervalUnit));
            }
        }
    }
}