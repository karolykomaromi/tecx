namespace Hydra.Jobs.Test.Jobs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Mail;
    using Hydra.Jobs.Server.Jobs;
    using MimeKit;
    using Moq;
    using netDumbster.smtp;
    using NHibernate;
    using NHibernate.Linq;
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
    }

    public class NhMailCycle : IUnsentMailSource, ISentMailSink
    {
        private const int BatchSize = 1000;

        private readonly ISession session;

        private readonly IDictionary<MimeMessage, PersistentMail> pending;

        public NhMailCycle(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
            this.pending = new Dictionary<MimeMessage, PersistentMail>();
        }

        public void Drop(MimeMessage message)
        {
            PersistentMail mail;
            if (this.pending.TryGetValue(message, out mail))
            {
                mail.SentAt = TimeProvider.UtcNow;
                mail.IsSent = true;

                this.session.Update(mail);
                this.pending.Remove(message);
            }
        }

        public IEnumerator<MimeMessage> GetEnumerator()
        {
            var mails = this.session.Query<PersistentMail>()
                .Where(m => m.IsSent == false)
                .OrderBy(m => m.EnqueuedAt)
                .Take(BatchSize)
                .ToArray();

            foreach (PersistentMail mail in mails)
            {
                this.pending[mail.Message] = mail;

                yield return mail.Message;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class PersistentMail : Entity
    {
        public virtual MimeMessage Message { get; set; }

        public virtual bool IsSent { get; set; }

        public virtual DateTime EnqueuedAt { get; set; }

        public virtual DateTime? SentAt { get; set; }
    }

    public abstract class Entity
    {
        public virtual long Id { get; set; }
    }
}
