namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzSchedulerStateMap : ClassMap<QuartzSchedulerState>
    {
        public QuartzSchedulerStateMap()
        {
            ////CREATE TABLE QRTZ_SCHEDULER_STATE (
            this.Table("QRTZ_SCHEDULER_STATE");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////INSTANCE_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.INSTANCE_NAME).Length(200).Not.Nullable();

            ////LAST_CHECKIN_TIME BIGINT(19) NOT NULL,
            this.Map(x => x.LAST_CHECKIN_TIME).Length(19).Not.Nullable();

            ////CHECKIN_INTERVAL BIGINT(19) NOT NULL,
            this.Map(x => x.CHECKIN_INTERVAL).Length(19).Not.Nullable();

            ////PRIMARY KEY (SCHED_NAME,INSTANCE_NAME))
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.INSTANCE_NAME);
        }
    }
}