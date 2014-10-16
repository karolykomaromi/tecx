namespace Hydra.Infrastructure.Test.Calendaring
{
    using System.Net.Mail;
    using Hydra.Infrastructure.Calendaring;
    using Hydra.Infrastructure.Test.Utility;
    using Xunit;
    using Xunit.Extensions;

    public class AttendeeBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_Required_Participant(AttendeeBuilder sut)
        {
            string actual = sut.Invite(new MailAddress("john.wayne@mail.invalid", "John Wayne")).Required();

            string expected = "ATTENDEE;ROLE=REQ-PARTICIPANT;CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Tentative(AttendeeBuilder sut)
        {
            string actual = sut.Invite(new MailAddress("john.wayne@mail.invalid", "John Wayne")).Tentative();

            string expected = "ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=TENTATIVE;CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Delegated_From_And_Accepted(AttendeeBuilder sut)
        {
            string actual =
                sut.Invite(new MailAddress("john.wayne@mail.invalid", "John Wayne"))
                   .DelegatedFrom(new MailAddress("clint.eastwood@mail.invalid", "Clint Eastwood"))
                   .Accepted();

            string expected =
                "ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;DELEGATED-FROM=\"MAILTO:clint.eastwood@mail.invalid\";CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Delegated_To(AttendeeBuilder sut)
        {
            string actual =
                sut.Invite(new MailAddress("john.wayne@mail.invalid", "John Wayne"))
                   .DelegatedTo(new MailAddress("clint.eastwood@mail.invalid", "Clint Eastwood"));

            string expected =
                "ATTENDEE;ROLE=NON-PARTICIPANT;PARTSTAT=DELEGATED;DELEGATED-TO=\"MAILTO:clint.eastwood@mail.invalid\";CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }
    }
}
