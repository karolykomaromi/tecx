namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzFiredTriggerMap : ClassMap<QuartzFiredTrigger>
    {
        public QuartzFiredTriggerMap()
        {
            ////CREATE TABLE QRTZ_FIRED_TRIGGERS (
            this.Table("QRTZ_FIRED_TRIGGERS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_FT_TRIG_INST_NAME ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,INSTANCE_NAME);
            ////CREATE INDEX IDX_QRTZ_FT_INST_JOB_REQ_RCVRY ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,INSTANCE_NAME,REQUESTS_RECOVERY);
            ////CREATE INDEX IDX_QRTZ_FT_J_G ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,JOB_NAME,JOB_GROUP);
            ////CREATE INDEX IDX_QRTZ_FT_JG ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,JOB_GROUP);
            ////CREATE INDEX IDX_QRTZ_FT_T_G ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP);
            ////CREATE INDEX IDX_QRTZ_FT_TG ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,TRIGGER_GROUP);
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable()
                .Index("IDX_QRTZ_FT_TRIG_INST_NAME")
                .Index("IDX_QRTZ_FT_INST_JOB_REQ_RCVRY")
                .Index("IDX_QRTZ_FT_J_G")
                .Index("IDX_QRTZ_FT_JG")
                .Index("IDX_QRTZ_FT_T_G")
                .Index("IDX_QRTZ_FT_TG");

            ////ENTRY_ID VARCHAR(95) NOT NULL,
            this.Map(x => x.ENTRY_ID).Length(95).Not.Nullable();

            ////TRIGGER_NAME VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_FT_T_G ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP);
            this.Map(x => x.TRIGGER_NAME).Length(200).Not.Nullable()
                .Index("IDX_QRTZ_FT_T_G");

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_FT_T_G ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP);
            ////CREATE INDEX IDX_QRTZ_FT_TG ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,TRIGGER_GROUP);
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable()
                .Index("IDX_QRTZ_FT_T_G")
                .Index("IDX_QRTZ_FT_TG");

            ////INSTANCE_NAME VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_FT_TRIG_INST_NAME ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,INSTANCE_NAME);
            ////CREATE INDEX IDX_QRTZ_FT_INST_JOB_REQ_RCVRY ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,INSTANCE_NAME,REQUESTS_RECOVERY);
            this.Map(x => x.INSTANCE_NAME).Length(200).Not.Nullable()
                .Index("IDX_QRTZ_FT_TRIG_INST_NAME")
                .Index("IDX_QRTZ_FT_INST_JOB_REQ_RCVRY");

            ////FIRED_TIME BIGINT(19) NOT NULL,
            this.Map(x => x.FIRED_TIME).Length(19).Not.Nullable();

            ////SCHED_TIME BIGINT(19) NOT NULL,
            this.Map(x => x.SCHED_TIME).Length(19).Not.Nullable();

            ////PRIORITY INTEGER NOT NULL,
            this.Map(x => x.PRIORITY).Not.Nullable();

            ////STATE VARCHAR(16) NOT NULL,
            this.Map(x => x.STATE).Length(16).Not.Nullable();

            ////JOB_NAME VARCHAR(200) NULL,
            ////CREATE INDEX IDX_QRTZ_FT_J_G ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,JOB_NAME,JOB_GROUP);
            this.Map(x => x.JOB_NAME).Length(200)
                .Index("IDX_QRTZ_FT_J_G");

            ////JOB_GROUP VARCHAR(200) NULL,
            ////CREATE INDEX IDX_QRTZ_FT_J_G ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,JOB_NAME,JOB_GROUP);
            ////CREATE INDEX IDX_QRTZ_FT_JG ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,JOB_GROUP);
            this.Map(x => x.JOB_GROUP).Length(200)
                .Index("IDX_QRTZ_FT_J_G")
                .Index("IDX_QRTZ_FT_JG");

            ////IS_NONCONCURRENT BOOLEAN NULL,
            this.Map(x => x.IS_NONCONCURRENT);

            ////REQUESTS_RECOVERY BOOLEAN NULL,
            ////CREATE INDEX IDX_QRTZ_FT_INST_JOB_REQ_RCVRY ON QRTZ_FIRED_TRIGGERS(SCHED_NAME,INSTANCE_NAME,REQUESTS_RECOVERY);
            this.Map(x => x.REQUESTS_RECOVERY)
                .Index("IDX_QRTZ_FT_INST_JOB_REQ_RCVRY");

            ////PRIMARY KEY (SCHED_NAME,ENTRY_ID))
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.ENTRY_ID);
        }
    }
}