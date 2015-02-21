namespace Hydra.Infrastructure.Mail
{
    using MimeKit;

    public class NullMailSink : ISentMailSink
    {
        public void Drop(MimeMessage message)
        {
            /* intentionally not implemented */
        }
    }
}