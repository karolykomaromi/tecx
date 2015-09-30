namespace Hydra.Infrastructure.Mail
{
    using System.Diagnostics.Contracts;
    using MimeKit;

    [ContractClass(typeof(SentMailSinkContract))]
    public interface ISentMailSink
    {
        void Drop(MimeMessage message);
    }

    [ContractClassFor(typeof(ISentMailSink))]
    internal abstract class SentMailSinkContract : ISentMailSink
    {
        public void Drop(MimeMessage message)
        {
            Contract.Requires(message != null);
        }
    }
}