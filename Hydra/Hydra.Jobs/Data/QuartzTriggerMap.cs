namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzTriggerMap : ClassMap<QuartzTrigger>
    {
        public QuartzTriggerMap()
        {
            ////CREATE TABLE QRTZ_TRIGGERS (
            this.Table("QRTZ_TRIGGERS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_T_J ON QRTZ_TRIGGERS(SCHED_NAME,JOB_NAME,JOB_GROUP);
            ////CREATE INDEX IDX_QRTZ_T_JG ON QRTZ_TRIGGERS(SCHED_NAME,JOB_GROUP);
            ////CREATE INDEX IDX_QRTZ_T_C ON QRTZ_TRIGGERS(SCHED_NAME,CALENDAR_NAME);
            ////CREATE INDEX IDX_QRTZ_T_G ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_GROUP);
            ////CREATE INDEX IDX_QRTZ_T_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_N_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_N_G_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NEXT_FIRE_TIME ON QRTZ_TRIGGERS(SCHED_NAME,NEXT_FIRE_TIME);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_STATE,NEXT_FIRE_TIME);
            ////CREATE INDEX IDX_QRTZ_T_NFT_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE_GRP ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_GROUP,TRIGGER_STATE);
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable()
                .Index("IDX_QRTZ_T_J")
                .Index("IDX_QRTZ_T_JG")
                .Index("IDX_QRTZ_T_C")
                .Index("IDX_QRTZ_T_G")
                .Index("IDX_QRTZ_T_STATE")
                .Index("IDX_QRTZ_T_N_STATE")
                .Index("IDX_QRTZ_T_N_G_STATE")
                .Index("IDX_QRTZ_T_NEXT_FIRE_TIME")
                .Index("IDX_QRTZ_T_NFT_ST")
                .Index("IDX_QRTZ_T_NFT_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

            ////TRIGGER_NAME VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_T_N_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            this.Map(x => x.TRIGGER_NAME).Length(200).Not.Nullable()
                .Index("IDX_QRTZ_T_N_STATE");

            ////TRIGGER_GROUP VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_T_G ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_GROUP);
            ////CREATE INDEX IDX_QRTZ_T_N_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_N_G_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE_GRP ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_GROUP,TRIGGER_STATE);
            this.Map(x => x.TRIGGER_GROUP).Length(200).Not.Nullable()
                .Index("IDX_QRTZ_T_G")
                .Index("IDX_QRTZ_T_N_STATE")
                .Index("IDX_QRTZ_T_N_G_STATE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

            ////JOB_NAME VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_T_J ON QRTZ_TRIGGERS(SCHED_NAME,JOB_NAME,JOB_GROUP);
            this.Map(x => x.JOB_NAME).Length(200).Not.Nullable().Index("IDX_QRTZ_T_J");

            ////JOB_GROUP VARCHAR(200) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_T_J ON QRTZ_TRIGGERS(SCHED_NAME,JOB_NAME,JOB_GROUP);
            ////CREATE INDEX IDX_QRTZ_T_JG ON QRTZ_TRIGGERS(SCHED_NAME,JOB_GROUP);
            this.Map(x => x.JOB_GROUP).Length(200).Not.Nullable()
                .Index("IDX_QRTZ_T_J")
                .Index("IDX_QRTZ_T_JG");

            ////DESCRIPTION VARCHAR(250) NULL,
            this.Map(x => x.DESCRIPTION).Length(250);

            ////NEXT_FIRE_TIME BIGINT(19) NULL,
            ////CREATE INDEX IDX_QRTZ_T_NEXT_FIRE_TIME ON QRTZ_TRIGGERS(SCHED_NAME,NEXT_FIRE_TIME);
            ////CREATE INDEX IDX_QRTZ_T_NFT_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE_GRP ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_GROUP,TRIGGER_STATE);
            this.Map(x => x.NEXT_FIRE_TIME).Length(19)
                .Index("IDX_QRTZ_T_NEXT_FIRE_TIME")
                .Index("IDX_QRTZ_T_NFT_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

            ////PREV_FIRE_TIME BIGINT(19) NULL,
            this.Map(x => x.PREV_FIRE_TIME).Length(19);

            ////PRIORITY INTEGER NULL,
            this.Map(x => x.PRIORITY);

            ////TRIGGER_STATE VARCHAR(16) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_T_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_N_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_N_G_STATE ON QRTZ_TRIGGERS(SCHED_NAME,TRIGGER_GROUP,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE_GRP ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_GROUP,TRIGGER_STATE);
            this.Map(x => x.TRIGGER_STATE).Length(16).Not.Nullable()
                .Index("IDX_QRTZ_T_STATE")
                .Index("IDX_QRTZ_T_N_STATE")
                .Index("IDX_QRTZ_T_N_G_STATE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

            ////TRIGGER_TYPE VARCHAR(8) NOT NULL,
            this.Map(x => x.TRIGGER_TYPE).Length(8).Not.Nullable();

            ////START_TIME BIGINT(19) NOT NULL,
            this.Map(x => x.START_TIME).Length(19);

            ////END_TIME BIGINT(19) NULL,
            this.Map(x => x.END_TIME).Length(19);

            ////CALENDAR_NAME VARCHAR(200) NULL,
            ////CREATE INDEX IDX_QRTZ_T_C ON QRTZ_TRIGGERS(SCHED_NAME,CALENDAR_NAME);
            this.Map(x => x.CALENDAR_NAME).Length(200)
                .Index("IDX_QRTZ_T_C");

            ////MISFIRE_INSTR SMALLINT(2) NULL,
            ////CREATE INDEX IDX_QRTZ_T_NFT_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_STATE);
            ////CREATE INDEX IDX_QRTZ_T_NFT_ST_MISFIRE_GRP ON QRTZ_TRIGGERS(SCHED_NAME,MISFIRE_INSTR,NEXT_FIRE_TIME,TRIGGER_GROUP,TRIGGER_STATE);
            this.Map(x => x.MISFIRE_INSTR).Length(2)
                .Index("IDX_QRTZ_T_NFT_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE")
                .Index("IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

            ////JOB_DATA BLOB NULL,
            this.Map(x => x.JOB_DATA);

            ////PRIMARY KEY (SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP),
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.JOB_NAME)
                .KeyProperty(x => x.JOB_GROUP);

            ////FOREIGN KEY (SCHED_NAME,JOB_NAME,JOB_GROUP)
            ////REFERENCES QRTZ_JOB_DETAILS(SCHED_NAME,JOB_NAME,JOB_GROUP))
            this.References(x => x.JobDetails).Columns(x => x.SCHED_NAME, x => x.JOB_NAME, x => x.JOB_GROUP);
        }
    }
}