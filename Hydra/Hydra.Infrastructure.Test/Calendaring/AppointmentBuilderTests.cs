namespace Hydra.Infrastructure.Test.Calendaring
{
    using Hydra.Infrastructure.Calendaring;
    using Hydra.TestTools;
    using Xunit;
    using Xunit.Extensions;

    public class AppointmentBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Never_Return_Null(AppointmentBuilder sut, MailAddressBuilder mailTo)
        {
            Assert.NotNull(sut.OrganizedBy(mailTo.JohnWayne()).Build());
        }

        [Theory, ContainerData]
        public void Should(AppointmentBuilder sut, MailAddressBuilder mailTo)
        {
            string ics = sut
                .StartsAt(TimeProvider.Now.Date.AddDays(1))
                .EndsAt(TimeProvider.Now.Date.AddDays(2))
                .OrganizedBy(mailTo.JohnWayne())
                .WithDescription("This is the description.")
                .WithSummary("This is the summary.")
                .WithAttendee(attendee => attendee.Invite(mailTo.ClintEastwood()))
                .WithReminder(reminder => reminder.RemindAt(d => d.Duration(-45.Minutes())).WithDescription("Before"))
                .WithReminder(reminder => reminder.RemindAt(d => d.Duration(2.Hours())).WithDescription("After"));
        }
    }
}