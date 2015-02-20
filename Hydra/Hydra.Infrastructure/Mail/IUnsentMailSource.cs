namespace Hydra.Infrastructure.Mail
{
    using System.Collections.Generic;
    using System.Net.Mail;

    public interface IUnsentMailSource : IEnumerable<MailMessage>
    {
    }
}