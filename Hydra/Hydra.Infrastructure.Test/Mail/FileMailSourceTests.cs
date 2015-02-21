namespace Hydra.Infrastructure.Test.Mail
{
    using System.IO;
    using System.Linq;
    using Hydra.Infrastructure.Mail;
    using Hydra.Infrastructure.Reflection;
    using Hydra.TestTools;
    using MimeKit;
    using Xunit;

    public class FileMailSourceTests
    {
        [Fact]
        public void Should_Return_MimeMessage_From_File()
        {
            FileInfo eml = new FileInfo(@".\" + TypeHelper.GetCallerMemberName() + ".eml");

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
        }

        [Fact]
        public void Should_Delete_File_Of_Delivered_Message()
        {
            FileInfo eml = new FileInfo(@".\" + TypeHelper.GetCallerMemberName() + ".eml");

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
            
            eml = new FileInfo(@".\" + TypeHelper.GetCallerMemberName() + ".eml");

            Assert.False(eml.Exists);
        }
    }
}
