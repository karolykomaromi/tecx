namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using MimeKit;

    public class EmbeddedResourceMailSource : IUnsentMailSource
    {
        private readonly Assembly assembly;

        private readonly HashSet<string> manifestResourceNames;

        public EmbeddedResourceMailSource(Assembly assembly, params string[] manifestResourceNames)
        {
            Contract.Requires(assembly != null);

            if (manifestResourceNames == null || manifestResourceNames.Length == 0)
            {
                manifestResourceNames = assembly.GetManifestResourceNames()
                    .Where(rn => rn.EndsWith(".eml", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }

            this.assembly = assembly;
            this.manifestResourceNames = new HashSet<string>(manifestResourceNames);
        }

        public IEnumerator<MimeMessage> GetEnumerator()
        {
            foreach (string resourceName in this.manifestResourceNames)
            {
                using (Stream stream = this.assembly.GetManifestResourceStream(resourceName))
                {
                    MimeMessage mime = MimeMessage.Load(stream);

                    yield return mime;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}