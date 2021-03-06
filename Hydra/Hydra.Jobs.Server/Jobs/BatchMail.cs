﻿namespace Hydra.Jobs.Server.Jobs
{
    using System;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Logging;
    using Hydra.Infrastructure.Mail;
    using MailKit.Net.Smtp;
    using MimeKit;
    using Quartz;

    public class BatchMail : IJob
    {
        private readonly IUnsentMailSource unsent;
        private readonly ISentMailSink sent;
        private readonly IMailJobSettings mailSettings;

        public BatchMail(IUnsentMailSource unsent, ISentMailSink sent, IMailJobSettings mailSettings)
        {
            Contract.Requires(unsent != null);
            Contract.Requires(sent != null);
            Contract.Requires(mailSettings != null);

            this.unsent = unsent;
            this.sent = sent;
            this.mailSettings = mailSettings;
        }

        public void Execute(IJobExecutionContext context)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(this.mailSettings.Host, this.mailSettings.Port);

                if (this.mailSettings.IsAuthenticationRequired)
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

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.unsent != null);
            Contract.Invariant(this.sent != null);
            Contract.Invariant(this.mailSettings != null);
        }
    }
}
