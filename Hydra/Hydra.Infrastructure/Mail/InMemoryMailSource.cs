namespace Hydra.Infrastructure.Mail
{
    using System.Collections;
    using System.Collections.Generic;
    using MimeKit;

    public class InMemoryMailSource : IUnsentMailSource
    {
        private readonly Queue<MimeMessage> messages;

        public InMemoryMailSource(params MimeMessage[] messages)
        {
            this.messages = new Queue<MimeMessage>(messages ?? new MimeMessage[0]);
        }

        public IEnumerator<MimeMessage> GetEnumerator()
        {
            while (this.messages.Count > 0)
            {
                MimeMessage msg = this.messages.Dequeue();

                yield return msg;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}