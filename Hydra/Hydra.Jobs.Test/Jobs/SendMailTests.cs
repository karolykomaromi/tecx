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

    public class SendMailTests
    {
        [Fact]
        public void Should_Send_Mail_And_Place_In_Appropiate_Queue()
        {
            SimpleSmtpServer server = null;

            try
            {
                server = SimpleSmtpServer.Start();

                var context = new Mock<IJobExecutionContext>();

                MailAddressBuilder address = new MailAddressBuilder();

                MailMessage message = new MailMessageBuilder()
                    .From(x => x.JohnWayne())
                    .WithRecipient(x => x.ClintEastwood())
                    .WithRecipient(x => x.HenryFonda())
                    .WithSubject("Foo")
                    .WithBody("Bar!");

                IUnsentMailQueue unsent = new InMemoryMailQueue(message);

                ISentMailQueue sent = new InMemoryMailQueue();

                MailSettings settings = new MailSettings
                {
                    Host = "localhost",
                    Port = server.Port,
                    Credentials = new NetworkCredential("user", "password")
                };

                IJob sut = new SendMail(unsent, sent, settings);

                sut.Execute(context.Object);

                Assert.Same(message, sent.Dequeue());
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
