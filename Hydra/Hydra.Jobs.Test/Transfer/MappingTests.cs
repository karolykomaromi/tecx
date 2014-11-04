namespace Hydra.Jobs.Test.Transfer
{
    using System;
    using AutoMapper;
    using Hydra.Infrastructure;
    using Hydra.Jobs.Transfer;
    using Quartz;
    using Xunit;
    using Xunit.Extensions;

    public class MappingTests
    {
        [Theory, ContainerData]
        public void Should_Map_CalendarIntervalTrigger_From_Quartz(IMappingEngine mapper)
        {
            ITrigger source = TriggerBuilder.Create().WithCalendarIntervalSchedule(
                x => x.WithIntervalInDays(2).SkipDayIfHourDoesNotExist(true).WithMisfireHandlingInstructionDoNothing()).Build();

            Trigger destination = mapper.Map<ITrigger, Trigger>(source);

            CalendarIntervalTrigger t = Assert.IsType<CalendarIntervalTrigger>(destination);

            Assert.Equal(2, t.RepeatInterval);
            Assert.Equal(IntervalUnit.Day, t.RepeatIntervalUnit);
            Assert.True(t.SkipDayIfHourDoesNotExist);
            Assert.Equal(MisfireInstruction.CalendarIntervalTrigger.DoNothing, t.MisfireInstruction);
        }

        [Theory, ContainerData]
        public void Should_Map_SimpleTrigger_From_Quartz(IMappingEngine mapper)
        {
            ITrigger source = TriggerBuilder.Create().WithSimpleSchedule(
                x => x.WithIntervalInHours(2).WithMisfireHandlingInstructionIgnoreMisfires()).Build();

            Trigger destination = mapper.Map<ITrigger, Trigger>(source);

            SimpleTrigger t = Assert.IsType<SimpleTrigger>(destination);

            Assert.Equal(2.Hours(), t.RepeatInterval);
            Assert.Equal(MisfireInstruction.IgnoreMisfirePolicy, t.MisfireInstruction);
        }

        [Theory, ContainerData]
        public void Should_Map_CronTrigger_From_Quartz(IMappingEngine mapper)
        {
            ITrigger source = TriggerBuilder.Create().WithSchedule(
                CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(12, 0, DayOfWeek.Friday).WithMisfireHandlingInstructionFireAndProceed()).Build();

            Trigger destination = mapper.Map<ITrigger, Trigger>(source);

            CronTrigger t = Assert.IsType<CronTrigger>(destination);

            Assert.Equal("0 0 12 ? * 6", t.CronExpressionString);
            Assert.Equal(MisfireInstruction.CronTrigger.FireOnceNow, t.MisfireInstruction);
        }

        [Theory, ContainerData]
        public void Should_Map_CalendarIntervalTrigger_To_Quartz(IMappingEngine mapper)
        {
            DateTimeOffset startTimeUtc = new DateTimeOffset(2014, 11, 3, 15, 37, 0, TimeSpan.Zero);
            DateTimeOffset endTimeUtc = new DateTimeOffset(2014, 11, 3, 16, 44, 0, TimeSpan.Zero);

            Trigger source = new CalendarIntervalTrigger
                {
                    Key = new Jobs.Transfer.TriggerKey { Name = "Foo", Group = string.Empty },
                    StartTimeUtc = startTimeUtc,
                    EndTimeUtc = endTimeUtc,
                };

            ITrigger destination = mapper.Map<Trigger, Quartz.ITrigger>(source);

            ICalendarIntervalTrigger t = Assert.IsAssignableFrom<ICalendarIntervalTrigger>(destination);

            Assert.Equal(endTimeUtc, t.EndTimeUtc);
        }

        [Theory, ContainerData]
        public void Should_Map_SimpleTrigger_To_Quartz(IMappingEngine mapper)
        {
            DateTimeOffset startTimeUtc = new DateTimeOffset(2014, 11, 3, 15, 37, 0, TimeSpan.Zero);
            DateTimeOffset endTimeUtc = new DateTimeOffset(2014, 11, 3, 16, 44, 0, TimeSpan.Zero);

            Trigger source = new SimpleTrigger
                {
                    Key = new Jobs.Transfer.TriggerKey { Name = "Foo", Group = string.Empty },
                    StartTimeUtc = startTimeUtc,
                    EndTimeUtc = endTimeUtc,
                };

            ITrigger destination = mapper.Map<Trigger, Quartz.ITrigger>(source);

            ISimpleTrigger t = Assert.IsAssignableFrom<ISimpleTrigger>(destination);

            Assert.Equal(endTimeUtc, t.EndTimeUtc);
        }
    }
}

