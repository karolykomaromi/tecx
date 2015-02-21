namespace Hydra.Infrastructure.Test.Mail
{
    using System.Linq;
    using Hydra.Infrastructure.Mail;
    using Hydra.TestTools;
    using MimeKit;
    using Xunit;

    public class EmbeddedResourceMailSourceTests
    {
        [Fact]
        public void Should_Return_MailMessage_From_EmbeddedResource()
        {
            IUnsentMailSource sut = new EmbeddedResourceMailSource(this.GetType().Assembly, KnownTestFiles.TestFiles.Mails.Mail01);

            MimeMessage actual = sut.Single();

            MailboxAddress from = new MailboxAddressBuilder().JohnWayne();

            Assert.Equal(from, actual.From.OfType<MailboxAddress>().Single());
        }
    }
}
