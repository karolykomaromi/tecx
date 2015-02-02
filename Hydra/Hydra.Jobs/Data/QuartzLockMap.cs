namespace Hydra.Jobs.Data
{
    using FluentNHibernate.Mapping;

    public class QuartzLockMap : ClassMap<QuartzLock>
    {
        public QuartzLockMap()
        {
            ////CREATE TABLE QRTZ_LOCKS (
            this.Table("QRTZ_LOCKS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////LOCK_NAME VARCHAR(40) NOT NULL,
            this.Map(x => x.LOCK_NAME).Length(40).Not.Nullable();

            ////PRIMARY KEY (SCHED_NAME,LOCK_NAME))
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.LOCK_NAME);
        }
    }
}