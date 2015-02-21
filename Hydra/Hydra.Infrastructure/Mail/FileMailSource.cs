namespace Hydra.Infrastructure.Mail
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using MimeKit;

    public class FileMailSource : IUnsentMailSource
    {
        private readonly DirectoryInfo unsentMailFolder;

        public FileMailSource(DirectoryInfo unsentMailFolder)
        {
            Contract.Requires(unsentMailFolder != null);
            Contract.Requires(unsentMailFolder.Exists);

            this.unsentMailFolder = unsentMailFolder;
        }

        public IEnumerator<MimeMessage> GetEnumerator()
        {
            foreach (FileInfo mailFile in this.unsentMailFolder.GetFiles("*.eml"))
            {
                using (Stream stream = new FileStream(mailFile.FullName, FileMode.Open))
                {
                    MimeMessage mime = MimeMessage.Load(stream);

                    yield return mime;
                }

                mailFile.Delete();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}