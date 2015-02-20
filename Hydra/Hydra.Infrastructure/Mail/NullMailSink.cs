namespace Hydra.Infrastructure.Mail
{
    using System.Net.Mail;

    public class NullMailSink : ISentMailSink
    {
        public void Drop(MailMessage message)
        {
            /* intentionally not implemented */
        }
    }
}