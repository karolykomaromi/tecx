namespace Hydra.Nh.Quartz
{
    using FluentNHibernate.Mapping;

    public class QuartzSimplePropertiesForTriggersMap : ClassMap<QuartzSimplePropertiesForTriggers>
    {
        public QuartzSimplePropertiesForTriggersMap()
        {
            ////CREATE TABLE QRTZ_SIMPROP_TRIGGERS
            this.Table("QRTZ_SIMPROP_TRIGGERS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////TRIGGER_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_NAME).Length(200).Not.Nullable();

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable();

            ////STR_PROP_1 VARCHAR(512) NULL,
            this.Map(x => x.STR_PROP_1).Length(512);

            ////STR_PROP_2 VARCHAR(512) NULL,
            this.Map(x => x.STR_PROP_2).Length(512);

            ////STR_PROP_3 VARCHAR(512) NULL,
            this.Map(x => x.STR_PROP_3).Length(512);

            ////INT_PROP_1 INT NULL,
            this.Map(x => x.INT_PROP_1);

            ////INT_PROP_2 INT NULL,
            this.Map(x => x.INT_PROP_2);

            ////LONG_PROP_1 BIGINT NULL,
            this.Map(x => x.LONG_PROP_1);

            ////LONG_PROP_2 BIGINT NULL,
            this.Map(x => x.LONG_PROP_2);

            ////DEC_PROP_1 NUMERIC(13,4) NULL,
            this.Map(x => x.DEC_PROP_1).Scale(4).Precision(13);

            ////DEC_PROP_2 NUMERIC(13,4) NULL,
            this.Map(x => x.DEC_PROP_2).Scale(4).Precision(13);

            ////BOOL_PROP_1 BOOLEAN NULL,
            this.Map(x => x.BOOL_PROP_1);

            ////BOOL_PROP_2 BOOLEAN NULL,
            this.Map(x => x.BOOL_PROP_2);

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