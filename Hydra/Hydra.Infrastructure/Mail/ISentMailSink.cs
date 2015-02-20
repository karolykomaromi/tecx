namespace Hydra.Infrastructure.Mail
{
    using System.Diagnostics.Contracts;
    using System.Net.Mail;

    [ContractClass(typeof(SentMailSinkContract))]
    public interface ISentMailSink
    {
        void Drop(MailMessage message);
    }

    [ContractClassFor(typeof(ISentMailSink))]
    internal abstract class SentMailSinkContract : ISentMailSink
    {
        public void Drop(MailMessage message)
        {
            Contract.Requires(message != null);
        }
    }
}