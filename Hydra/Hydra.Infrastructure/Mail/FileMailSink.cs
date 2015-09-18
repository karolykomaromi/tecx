namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Hydra.Infrastructure.Logging;
    using MimeKit;

    public class FileMailSink : ISentMailSink
    {
        private readonly DirectoryInfo sentMailFolder;

        public FileMailSink(DirectoryInfo sentMailFolder)
        {
            Contract.Requires(sentMailFolder != null);

            this.sentMailFolder = sentMailFolder;
        }

        public void Drop(MimeMessage message)
        {
            try
            {
                string fileName = Path.Combine(this.sentMailFolder.FullName, Guid.NewGuid().ToString(FormatStrings.Guid.Digits) + ".eml");

                using (Stream stream = new FileStream(fileName, FileMode.Create))
                {
                    message.WriteTo(stream);
                }
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);
            }
        }
    }
}