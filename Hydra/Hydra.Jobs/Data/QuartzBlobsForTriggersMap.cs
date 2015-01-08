namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzBlobsForTriggersMap : ClassMap<QuartzBlobsForTriggers>
    {
        public QuartzBlobsForTriggersMap()
        {
            ////CREATE TABLE QRTZ_BLOB_TRIGGERS (
            this.Table("QRTZ_BLOB_TRIGGERS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////TRIGGER_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_NAME).Length(200).Not.Nullable();

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable();

            ////BLOB_DATA BLOB NULL,
            this.Map(x => x.BLOB_DATA);

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