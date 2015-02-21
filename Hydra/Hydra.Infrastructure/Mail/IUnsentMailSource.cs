namespace Hydra.Infrastructure.Mail
{
    using System.Collections.Generic;
    using MimeKit;

    public interface IUnsentMailSource : IEnumerable<MimeMessage>
    {
    }
}