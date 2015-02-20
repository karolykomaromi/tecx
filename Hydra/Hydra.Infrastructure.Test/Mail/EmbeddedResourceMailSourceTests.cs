namespace Hydra.Infrastructure.Test.Mail
{
    using System.Linq;
    using System.Net.Mail;
    using Hydra.Infrastructure.Mail;
    using Hydra.TestTools;
    using Xunit;

    public class EmbeddedResourceMailSourceTests
    {
        [Fact]
        public void Should_Return_MailMessage_From_EmbeddedResource()
        {
            IUnsentMailSource sut = new EmbeddedResourceMailSource(this.GetType().Assembly, KnownTestFiles.TestFiles.Mails.Mail01);

            MailMessage actual = sut.Single();

            MailAddress from = new MailAddressBuilder().ClintEastwood();

            Assert.Equal(from, actual.From);
        }
    }
}
