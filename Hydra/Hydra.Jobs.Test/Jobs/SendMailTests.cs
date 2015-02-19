namespace Hydra.Jobs.Test.Jobs
{
    using System.Net.Mail;
    using Hydra.Infrastructure.Calendaring;
    using Hydra.Infrastructure.Mail;
    using Hydra.TestTools;
    using netDumbster.smtp;
    using Xunit;

    public class SendMailTests
    {
        [Fact]
        public async void Should_Send_Mail()
        {
            SimpleSmtpServer server = null;

            try
            {
                server = SimpleSmtpServer.Start();

                using (var client = new SmtpClient("localhost", server.Port))
                {
                    string johnWayne = new MailAddressBuilder().JohnWayne().Build().ToString();
                    string clintEastwood = new MailAddressBuilder().ClintEastwood().Build().ToString();

                    await client.SendMailAsync(johnWayne, clintEastwood, "Foo!", "Bar");
                }

                Assert.Equal(1, server.ReceivedEmailCount);
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }
    }
}
