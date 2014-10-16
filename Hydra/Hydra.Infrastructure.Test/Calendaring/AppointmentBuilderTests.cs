namespace Hydra.Infrastructure.Test.Calendaring
{
    using System.Net.Mail;
    using Hydra.Infrastructure.Calendaring;
    using Hydra.Infrastructure.Test.Utility;
    using Xunit;
    using Xunit.Extensions;

    public class AppointmentBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Never_Return_Null(AppointmentBuilder sut)
        {
            Assert.NotNull(sut.Build());
        }

        [Theory, ContainerData]
        public void Should(AppointmentBuilder sut)
        {
            string ics = sut
                .StartsAt(TimeProvider.Now.Date.AddDays(1))
                .EndsAt(TimeProvider.Now.Date.AddDays(2))
                .OrganizedBy(new MailAddress("john.wayne@mail.invalid", "John Wayne"))
                .WithDescription("This is the description.")
                .WithSummary("This is the summary.")
                .WithAttendee(attendee => attendee.Invite(new MailAddress("clint.eastwood@mail.invalid", "Clint Eastwood")))
                .WithReminder(reminder => reminder.RemindAt(d => d.Duration(-45.Minutes())).WithDescription("Before"))
                .WithReminder(reminder => reminder.RemindAt(d => d.Duration(2.Hours())).WithDescription("After"));
        }
    }
}