namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzCronTriggerMap : ClassMap<QuartzCronTrigger>
    {
        public QuartzCronTriggerMap()
        {
            ////CREATE TABLE QRTZ_CRON_TRIGGERS (
            this.Table("QRTZ_CRON_TRIGGERS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////TRIGGER_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_NAME).Length(200).Not.Nullable();

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable();

            ////CRON_EXPRESSION VARCHAR(120) NOT NULL,
            this.Map(x => x.CRON_EXPRESSION).Length(120).Not.Nullable();

            ////TIME_ZONE_ID VARCHAR(80),
            this.Map(x => x.TIME_ZONE_ID).Length(80);

            ////PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.TRIGGER_NAME)
                .KeyProperty(x => x.TRIGGER_GROUP);

            ////FOREIGN KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP)
            ////REFERENCES QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP))
            this.References(x => x.Trigger).Columns(x => x.SCHED_NAME, x => x.TRIGGER_NAME, x => x.TRIGGER_GROUP);
        }
    }
}