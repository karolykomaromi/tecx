namespace Hydra.Infrastructure.Test.Calendaring
{
    using Hydra.Infrastructure.Calendaring;
    using Hydra.TestTools;
    using Xunit;
    using Xunit.Extensions;

    public class AttendeeBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_Required_Participant(AttendeeBuilder sut, MailAddressBuilder mailTo)
        {
            string actual = sut.Invite(mailTo.JohnWayne()).Required().Build();

            string expected = "ATTENDEE;ROLE=REQ-PARTICIPANT;CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Tentative(AttendeeBuilder sut, MailAddressBuilder mailTo)
        {
            string actual = sut.Invite(mailTo.JohnWayne()).Tentative().Build();

            string expected = "ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=TENTATIVE;CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Delegated_From_And_Accepted(AttendeeBuilder sut, MailAddressBuilder mailTo)
        {
            string actual = sut.Invite(mailTo.JohnWayne()).DelegatedFrom(mailTo.ClintEastwood()).Accepted().Build();

            string expected =
                "ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;DELEGATED-FROM=\"MAILTO:clint.eastwood@mail.invalid\";CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Build_Delegated_To(AttendeeBuilder sut, MailAddressBuilder mailTo)
        {
            string actual = sut.Invite(mailTo.JohnWayne()).DelegatedTo(mailTo.ClintEastwood()).Build();

            string expected =
                "ATTENDEE;ROLE=NON-PARTICIPANT;PARTSTAT=DELEGATED;DELEGATED-TO=\"MAILTO:clint.eastwood@mail.invalid\";CN=John Wayne:MAILTO:john.wayne@mail.invalid";

            Assert.Equal(expected, actual);
        }
    }
}
