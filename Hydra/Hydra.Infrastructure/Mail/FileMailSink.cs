namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net.Mail;
    using Hydra.Infrastructure.Logging;

    public class FileMailSink : ISentMailSink
    {
        private readonly DirectoryInfo sentMailFolder;

        public FileMailSink(DirectoryInfo sentMailFolder)
        {
            Contract.Requires(sentMailFolder != null);
            Contract.Requires(sentMailFolder.Exists);

            this.sentMailFolder = sentMailFolder;
        }

        public void Drop(MailMessage message)
        {
            try
            {
                string fileName = Path.Combine(this.sentMailFolder.FullName, Guid.NewGuid().ToString("N") + ".eml");

                using (Stream stream = new FileStream(fileName, FileMode.Create))
                {
                    message.Save(stream);
                }
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);
            }
        }
    }
}