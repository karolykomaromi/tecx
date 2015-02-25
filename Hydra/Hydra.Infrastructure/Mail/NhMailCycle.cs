namespace Hydra.Infrastructure.Mail
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using MimeKit;
    using NHibernate;
    using NHibernate.Linq;

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
}