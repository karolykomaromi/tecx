namespace Hydra.Infrastructure.Mail
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using MimeKit;

    public abstract class MimeKitBasedMailSource : IUnsentMailSource
    {
        public abstract IEnumerator<MailMessage> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected virtual MailMessage ConvertToMailMessage(MimeMessage mime)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(mime.From.First().ToString()),
                Subject = mime.Subject,
                Body = mime.TextBody
            };

            foreach (InternetAddress address in mime.To)
            {
                mail.To.Add(new MailAddress(address.ToString()));
            }

            return mail;
        }
    }
}