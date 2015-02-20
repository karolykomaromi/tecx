namespace Hydra.Jobs.Test.Jobs
{
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using Hydra.Infrastructure.Mail;
    using Hydra.Jobs.Server.Jobs;
    using Hydra.TestTools;
    using Moq;
    using netDumbster.smtp;
    using Quartz;
    using Xunit;

    public class BatchMailTests
    {
        [Fact]
        public void Should_Send_Mail_And_Place_In_Appropiate_Queue()
        {
            SimpleSmtpServer server = null;

            try
            {
                server = SimpleSmtpServer.Start();

                var context = new Mock<IJobExecutionContext>();

                MailMessage message = new MailMessageBuilder()
                    .From(x => x.JohnWayne())
                    .Recipient(x => x.ClintEastwood())
                    .Recipient(x => x.HenryFonda())
                    .ReplyTo(x => x.DoNotReply())
                    .Subject("Foo")
                    .Body("Bar!");

                var unsent = new InMemoryMailSource(message);

                var sent = new InMemoryMailSink();

                MailSettings settings = new MailSettings
                {
                    Host = "localhost",
                    Port = server.Port,
                    Credentials = new NetworkCredential("user", "password")
                };

                IJob sut = new BatchMail(unsent, sent, settings);

                sut.Execute(context.Object);

                Assert.Same(message, sent.Messages[0]);
                Assert.Equal(1, server.ReceivedEmailCount);

                using (Stream stream = new FileStream(@".\message.eml", FileMode.Create))
                {
                    message.Save(stream);
                }
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }

        [Fact]
        public async void Should_Send_Mail()
        {
            SimpleSmtpServer server = null;

            try
            {
                server = SimpleSmtpServer.Start();

                using (var client = new SmtpClient("localhost", server.Port))
                {
                    MailMessage message = new MailMessageBuilder()
                        .From(x => x.JohnWayne())
                        .Recipient(x => x.ClintEastwood())
                        .Recipient(x => x.HenryFonda())
                        .ReplyTo(x => x.DoNotReply())
                        .Subject("Foo")
                        .Body("Bar!");

                    await client.SendMailAsync(message);
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
