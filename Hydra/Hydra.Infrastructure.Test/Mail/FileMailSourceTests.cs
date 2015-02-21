namespace Hydra.Infrastructure.Test.Mail
{
    using System.IO;
    using System.Linq;
    using Hydra.Infrastructure.Mail;
    using Hydra.TestTools;
    using MimeKit;
    using Xunit;

    public class FileMailSourceTests
    {
        [Fact]
        public void Should_Return_MimeMessage_From_File_And_Delete_File()
        {
            FileInfo eml = new FileInfo(@".\Should_Return_MimeMessage_From_File_And_Delete_File.eml");

            using (Stream fileStream = eml.Create())
            {
                using (Stream embeddedResxStream = this.GetType().Assembly.GetManifestResourceStream(KnownTestFiles.TestFiles.Mails.Mail01))
                {
                    embeddedResxStream.CopyTo(fileStream);

                    fileStream.Flush();
                }
            }

            IUnsentMailSource sut = new FileMailSource(new DirectoryInfo("."));

            MimeMessage actual = sut.Single();

            MailboxAddress from = new MailboxAddressBuilder().JohnWayne();

            Assert.Equal(from, actual.From.OfType<MailboxAddress>().Single());

            eml = new FileInfo(@".\TestFiles\Mails\NotEmbedded.eml");

            Assert.False(eml.Exists);
        }
    }
}
