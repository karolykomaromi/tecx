namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzPausedTriggerGroupMap : ClassMap<QuartzPausedTriggerGroup>
    {
        public QuartzPausedTriggerGroupMap()
        {
            ////CREATE TABLE QRTZ_PAUSED_TRIGGER_GRPS (
            this.Table("QRTZ_PAUSED_TRIGGER_GRPS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable();

            ////PRIMARY KEY (SCHED_NAME,TRIGGER_GROUP))
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.TRIGGER_GROUP);
        }
    }
}