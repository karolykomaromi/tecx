namespace Hydra.Jobs.Server.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using Hydra.Infrastructure.Logging;
    using Quartz;

    public class SendMail : IJob
    {
        private readonly IUnsentMailQueue unsent;
        private readonly ISentMailQueue sent;
        private readonly MailSettings mailSettings;

        public SendMail(IUnsentMailQueue unsent, ISentMailQueue sent, MailSettings mailSettings)
        {
            this.unsent = unsent;
            this.sent = sent;
            this.mailSettings = mailSettings;
        }

        public void Execute(IJobExecutionContext context)
        {
            using (var client = new SmtpClient(this.mailSettings.Host, this.mailSettings.Port) { Credentials = this.mailSettings.Credentials })
            {
                MailMessage message;
                while ((message = this.unsent.Dequeue()) != null)
                {
                    try
                    {
                        client.Send(message);

                        this.sent.Enqueue(message);
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                    }
                }
            }
        }
    }

    public class MailSettings
    {
        public virtual ICredentialsByHost Credentials { get; set; }

        public virtual int Port { get; set; }

        public virtual string Host { get; set; }
    }

    public interface IUnsentMailQueue
    {
        void Enqueue(MailMessage message);

        MailMessage Dequeue();
    }

    public interface ISentMailQueue
    {
        void Enqueue(MailMessage message);

        MailMessage Dequeue();
    }

    public class InMemoryMailQueue : IUnsentMailQueue, ISentMailQueue
    {
        private readonly Queue<MailMessage> messages;

        public InMemoryMailQueue(params MailMessage[] messages)
        {
            this.messages = new Queue<MailMessage>(messages ?? new MailMessage[0]);
        }

        public void Enqueue(MailMessage message)
        {
            this.messages.Enqueue(message);
        }

        public MailMessage Dequeue()
        {
            if (this.messages.Count > 0)
            {
                return this.messages.Dequeue();
            }

            return null;
        }
    }
}
