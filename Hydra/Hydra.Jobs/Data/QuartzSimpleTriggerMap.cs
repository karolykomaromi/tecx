namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzSimpleTriggerMap : ClassMap<QuartzSimpleTrigger>
    {
        public QuartzSimpleTriggerMap()
        {
            ////CREATE TABLE QRTZ_SIMPLE_TRIGGERS (
            this.Table("QRTZ_SIMPLE_TRIGGERS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////TRIGGER_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_NAME).Length(200).Not.Nullable();

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable();

            ////REPEAT_COUNT BIGINT(7) NOT NULL,
            this.Map(x => x.REPEAT_COUNT).Length(7).Not.Nullable();

            ////REPEAT_INTERVAL BIGINT(12) NOT NULL,
            this.Map(x => x.REPEAT_INTERVAL).Length(12).Not.Nullable();

            ////TIMES_TRIGGERED BIGINT(10) NOT NULL,
            this.Map(x => x.TIMES_TRIGGERED).Length(10).Not.Nullable();

            ////PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.TRIGGER_NAME)
                .KeyProperty(x => x.TRIGGER_GROUP);
        }
    }
}