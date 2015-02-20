namespace Hydra.Infrastructure.Mail
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net.Mail;
    using MimeKit;

    public class FileMailSource : MimeKitBasedMailSource
    {
        private readonly DirectoryInfo unsentMailFolder;

        public FileMailSource(DirectoryInfo unsentMailFolder)
        {
            Contract.Requires(unsentMailFolder != null);
            Contract.Requires(unsentMailFolder.Exists);

            this.unsentMailFolder = unsentMailFolder;
        }

        public override IEnumerator<MailMessage> GetEnumerator()
        {
            foreach (FileInfo mailFile in this.unsentMailFolder.GetFiles("*.eml"))
            {
                using (Stream stream = new FileStream(mailFile.FullName, FileMode.Open))
                {
                    MimeMessage mime = MimeMessage.Load(stream);

                    MailMessage mail = this.ConvertToMailMessage(mime);

                    yield return mail;
                }

                mailFile.Delete();
            }
        }
    }
}