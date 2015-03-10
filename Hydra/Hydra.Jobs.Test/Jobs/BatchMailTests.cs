namespace Hydra.Jobs.Test.Jobs
{
    using System.IO;
    using System.Net;
    using Hydra.Infrastructure.Mail;
    using Hydra.Jobs.Server.Jobs;
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

                ////MailMessage message = new MailMessageBuilder()
                ////    .From(x => x.JohnWayne())
                ////    .Recipient(x => x.ClintEastwood())
                ////    .Recipient(x => x.HenryFonda())
                ////    .ReplyTo(x => x.DoNotReply())
                ////    .Subject("Foo")
                ////    .Body("Bar!");

                ////IUnsentMailSource unsent = new InMemoryMailSource(message);

                ////ISentMailSink sent = new InMemoryMailSink();

                ////DirectoryInfo unsentFolder = new DirectoryInfo(@".\Unsent");
                ////if (!unsentFolder.Exists)
                ////{
                ////    unsentFolder.Create();
                ////}

                ////IUnsentMailSource unsent = new FileMailSource(unsentFolder);

                IUnsentMailSource unsent = new EmbeddedResourceMailSource(this.GetType().Assembly);

                DirectoryInfo sentFolder = new DirectoryInfo(@".\Sent");
                if (!sentFolder.Exists)
                {
                    sentFolder.Create();
                }

                ISentMailSink sent = new FileMailSink(sentFolder);

                InMemoryMailJobSettings settings = new InMemoryMailJobSettings
                {
                    Host = "localhost",
                    Port = server.Port,
                    Credentials = new NetworkCredential("userName", "password"),
                    IsAuthenticationRequired = false
                };

                IJob sut = new BatchMail(unsent, sent, settings);

                sut.Execute(context.Object);

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
