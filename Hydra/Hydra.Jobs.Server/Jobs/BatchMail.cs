namespace Hydra.Jobs.Server.Jobs
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Mail;
    using Hydra.Infrastructure.Logging;
    using Hydra.Infrastructure.Mail;
    using Quartz;

    public class BatchMail : IJob
    {
        private readonly IUnsentMailSource unsent;
        private readonly ISentMailSink sent;
        private readonly MailSettings mailSettings;

        public BatchMail(IUnsentMailSource unsent, ISentMailSink sent, MailSettings mailSettings)
        {
            this.unsent = unsent;
            this.sent = sent;
            this.mailSettings = mailSettings;
        }

        public void Execute(IJobExecutionContext context)
        {
            using (var client = new SmtpClient(this.mailSettings.Host, this.mailSettings.Port) { Credentials = this.mailSettings.Credentials })
            {
                foreach (MailMessage message in this.unsent)
                {
                    try
                    {
                        client.Send(message);

                        this.sent.Drop(message);
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                    }
                }
            }
        }
    }
}
