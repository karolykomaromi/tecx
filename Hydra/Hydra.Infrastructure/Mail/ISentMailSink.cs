namespace Hydra.Infrastructure.Mail
{
    using System.Net.Mail;

    public interface ISentMailSink
    {
        void Drop(MailMessage message);
    }
}