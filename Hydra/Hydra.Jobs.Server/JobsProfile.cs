namespace Hydra.Jobs.Server
{
    using AutoMapper;

    public class JobsProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<TriggerKey, Quartz.TriggerKey>().ConvertUsing(dto => new Quartz.TriggerKey(dto.Name, dto.Group));
            this.CreateMap<JobKey, Quartz.JobKey>().ConvertUsing(dto => new Quartz.JobKey(dto.Name, dto.Group));
            this.CreateMap<Quartz.TriggerKey, TriggerKey>();
            this.CreateMap<Quartz.JobKey, JobKey>();

            this.CreateMap<Quartz.IJobDetail, JobDetail>();
            this.CreateMap<Quartz.ICalendarIntervalTrigger, CalendarIntervalTrigger>();
            this.CreateMap<Quartz.ICronTrigger, CronTrigger>();
            this.CreateMap<Quartz.ISimpleTrigger, SimpleTrigger>();
        }
    }
}
