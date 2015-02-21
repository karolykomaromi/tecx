namespace Hydra.Jobs.Server.Jobs
{
    using System;
    using Hydra.Infrastructure.Logging;
    using Hydra.Infrastructure.Mail;
    using MailKit.Net.Smtp;
    using MimeKit;
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
            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(this.mailSettings.Host, this.mailSettings.Port);

                if (this.mailSettings.NeedsAuthentication)
                {
                    client.Authenticate(this.mailSettings.Credentials);
                }

                foreach (MimeMessage message in this.unsent)
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
