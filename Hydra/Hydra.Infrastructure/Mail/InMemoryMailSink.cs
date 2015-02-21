namespace Hydra.Infrastructure.Mail
{
    using System.Collections.Generic;
    using MimeKit;

    public class InMemoryMailSink : ISentMailSink
    {
        private readonly Queue<MimeMessage> messages;

        public InMemoryMailSink()
        {
            this.messages = new Queue<MimeMessage>();
        }

        public IReadOnlyList<MimeMessage> Messages
        {
            get { return this.messages.ToArray(); }
        }

        public void Drop(MimeMessage message)
        {
            this.messages.Enqueue(message);
        }
    }
}