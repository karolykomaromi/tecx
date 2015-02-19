namespace Hydra.Infrastructure.Mail
{
    using System.Net.Mail;

    public class MailMessageBuilder : Builder<MailMessage>
    {
        public override MailMessage Build()
        {
            return new MailMessage();
        }
    }
}