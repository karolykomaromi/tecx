namespace Hydra.Nh.Quartz
{
    using FluentNHibernate.Mapping;

    public class QuartzCalendarMap : ClassMap<QuartzCalendar>
    {
        public QuartzCalendarMap()
        {
            ////CREATE TABLE QRTZ_CALENDARS (
            this.Table("QRTZ_CALENDARS");

            ////SCHED_NAME VARCHAR(120) NOT NULL,
            this.Map(x => x.SCHED_NAME).Length(120).Not.Nullable();

            ////CALENDAR_NAME VARCHAR(200) NOT NULL,
            this.Map(x => x.CALENDAR_NAME).Length(200).Not.Nullable();

            ////CALENDAR BLOB NOT NULL,
            this.Map(x => x.CALENDAR).Not.Nullable();

            ////PRIMARY KEY (SCHED_NAME,CALENDAR_NAME))
            this.CompositeId()
                .KeyProperty(x => x.SCHED_NAME)
                .KeyProperty(x => x.CALENDAR_NAME);
        }
    }
}