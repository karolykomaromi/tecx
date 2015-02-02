namespace Hydra.Jobs.Transfer
{
    using AutoMapper;
    using Quartz.Impl.Triggers;

    public class JobsProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<Quartz.TriggerKey, TriggerKey>().ReverseMap().ConvertUsing(dto => new Quartz.TriggerKey(dto.Name, dto.Group));

            this.CreateMap<Quartz.JobKey, JobKey>().ReverseMap().ConvertUsing(dto => new Quartz.JobKey(dto.Name, dto.Group));

            this.CreateMap<Quartz.IJobDetail, JobDetail>().ReverseMap();

            this.CreateMap<CalendarIntervalTriggerImpl, CalendarIntervalTrigger>();

            this.CreateMap<CalendarIntervalTrigger, CalendarIntervalTriggerImpl>()
                .ForMember(impl => impl.Key, x => x.Ignore())
                .ForMember(impl => impl.JobKey, x => x.Ignore())
                .As<Quartz.ICalendarIntervalTrigger>();

            this.CreateMap<SimpleTrigger, SimpleTriggerImpl>()
                .ForMember(impl => impl.Key, x => x.Ignore())
                .ForMember(impl => impl.JobKey, x => x.Ignore())
                .As<Quartz.ISimpleTrigger>();

            this.CreateMap<CronTriggerImpl, CronTrigger>();

            this.CreateMap<SimpleTriggerImpl, SimpleTrigger>();

            this.CreateMap<Trigger, Quartz.ITrigger>()
                .Include<CalendarIntervalTrigger, Quartz.ICalendarIntervalTrigger>()
                .Include<SimpleTrigger, Quartz.ISimpleTrigger>()
                .Include<CronTrigger, Quartz.ICronTrigger>();

            this.CreateMap<Quartz.ITrigger, Trigger>()
                .Include<Quartz.ICalendarIntervalTrigger, CalendarIntervalTrigger>()
                .Include<Quartz.ISimpleTrigger, SimpleTrigger>()
                .Include<Quartz.ICronTrigger, CronTrigger>();
        }
    }
}
