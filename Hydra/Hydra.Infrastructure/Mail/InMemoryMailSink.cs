namespace Hydra.Infrastructure.Mail
{
    using System.Collections.Generic;
    using System.Net.Mail;

    public class InMemoryMailSink : ISentMailSink
    {
        private readonly Queue<MailMessage> messages;

        public InMemoryMailSink()
        {
            this.messages = new Queue<MailMessage>();
        }

        public IReadOnlyList<MailMessage> Messages
        {
            get { return this.messages.ToArray(); }
        }

        public void Drop(MailMessage message)
        {
            this.messages.Enqueue(message);
        }
    }
}