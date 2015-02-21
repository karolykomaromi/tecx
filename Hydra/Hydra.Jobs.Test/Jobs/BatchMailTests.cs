namespace Hydra.Jobs.Test.Jobs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using Hydra.Infrastructure.Mail;
    using Hydra.Jobs.Server.Jobs;
    using Hydra.TestTools;
    using MimeKit;
    using Moq;
    using netDumbster.smtp;
    using NHibernate;
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

                ////using (Stream stream = new FileStream(@".\message.eml", FileMode.Create))
                ////{
                ////    message.Save(stream);
                ////}
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
        public void Should_Parse_Eml_File()
        {
            MimeMessage msg;

            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(KnownTestFiles.TestFiles.Mails.Mail01))
            {
                msg = MimeMessage.Load(stream);
            }

            Assert.NotNull(msg);
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

    public class NhMailCycle : IUnsentMailSource, ISentMailSink
    {
        private readonly ISession session;

        private readonly IDictionary<MimeMessage, long> mailMessageIdentityMap;

        public NhMailCycle(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
            this.mailMessageIdentityMap = new Dictionary<MimeMessage, long>();
        }

        public void Drop(MimeMessage message)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<MimeMessage> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
