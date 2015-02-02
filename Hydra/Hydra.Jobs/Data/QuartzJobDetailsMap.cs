namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzJobDetailsMap : ClassMap<QuartzJobDetails>
    {
        public QuartzJobDetailsMap()
        {
            ////CREATE TABLE QRTZ_JOB_DETAILS(
            this.Table("QRTZ_JOB_DETAILS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            ////CREATE INDEX IDX_QRTZ_J_REQ_RECOVERY ON QRTZ_JOB_DETAILS(SCHED_NAME,REQUESTS_RECOVERY);
            ////CREATE INDEX IDX_QRTZ_J_GRP ON QRTZ_JOB_DETAILS(SCHED_NAME,JOB_GROUP);
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable()
                .Index("IDX_QRTZ_J_REQ_RECOVERY")
                .Index("IDX_QRTZ_J_GRP");

            ////JOB_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.JOB_NAME).Length(200).Not.Nullable();

            ////JOB_GROUP VARCHAR(200) NOT NULL,
            this.Map(x => x.JOB_GROUP).Length(200).Not.Nullable();

            ////DESCRIPTION VARCHAR(250) NULL,
            this.Map(x => x.DESCRIPTION).Length(250);

            ////JOB_CLASS_NAME VARCHAR(250) NOT NULL,
            this.Map(x => x.JOB_CLASS_NAME).Length(250).Not.Nullable();

            ////IS_DURABLE BOOLEAN NOT NULL,
            this.Map(x => x.IS_DURABLE).Not.Nullable();

            ////IS_NONCONCURRENT BOOLEAN NOT NULL,
            this.Map(x => x.IS_NONCONCURRENT).Not.Nullable();

            ////IS_UPDATE_DATA BOOLEAN NOT NULL,
            this.Map(x => x.IS_UPDATE_DATA).Not.Nullable();

            ////REQUESTS_RECOVERY BOOLEAN NOT NULL,
            ////CREATE INDEX IDX_QRTZ_J_REQ_RECOVERY ON QRTZ_JOB_DETAILS(SCHED_NAME,REQUESTS_RECOVERY);
            ////CREATE INDEX IDX_QRTZ_J_GRP ON QRTZ_JOB_DETAILS(SCHED_NAME,JOB_GROUP);
            this.Map(x => x.REQUESTS_RECOVERY).Not.Nullable()
                .Index("IDX_QRTZ_J_REQ_RECOVERY")
                .Index("IDX_QRTZ_J_GRP");

            ////JOB_DATA BLOB NULL,
            this.Map(x => x.JOB_DATA);

            ////PRIMARY KEY (SCHED_NAME,JOB_NAME,JOB_GROUP))
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.JOB_NAME)
                .KeyProperty(x => x.JOB_GROUP);
        }
    }
}