namespace Hydra.Infrastructure.Mail
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.Mail;

    public class InMemoryMailSource : IUnsentMailSource
    {
        private readonly Queue<MailMessage> messages;

        public InMemoryMailSource(params MailMessage[] messages)
        {
            this.messages = new Queue<MailMessage>(messages ?? new MailMessage[0]);
        }

        public IEnumerator<MailMessage> GetEnumerator()
        {
            while (this.messages.Count > 0)
            {
                MailMessage msg = this.messages.Dequeue();

                yield return msg;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}