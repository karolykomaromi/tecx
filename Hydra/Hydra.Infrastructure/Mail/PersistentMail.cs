namespace Hydra.Infrastructure.Mail
{
    using System;
    using MimeKit;

    public class PersistentMail : Entity
    {
        public virtual MimeMessage Message { get; set; }

        public virtual bool IsSent { get; set; }

        public virtual DateTime EnqueuedAt { get; set; }

        public virtual string EnqueuedBy { get; set; }

        public virtual DateTime? SentAt { get; set; }

        public virtual MailCharge Charge { get; set; }
    }
}