namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Xml.Linq;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Logging;

    public class ResxFileDataWriter : IDataWriter<ResourceItem>
    {
        private readonly Stream stream;

        public ResxFileDataWriter(Stream stream)
        {
            Contract.Requires(stream != null);

            this.stream = stream;
        }

        public ImportResult Write(IEnumerable<ResourceItem> items)
        {
            XDocument resxDocument;

            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("Hydra.Import.ResourceFileTemplate.xml"))
            {
                resxDocument = XDocument.Load(s);
            }

            List<ImportMessage> messages = new List<ImportMessage>();

            foreach (ResourceItem ri in items)
            {
                try
                {
                    XNamespace ns = XNamespace.Get("xml");

                    XElement xml = new XElement("data", new XAttribute("name", ri.Name), new XAttribute(XNamespace.Xml + "space", "preserve"), new XElement("value", ri.Value));

                    resxDocument.Root.Add(xml);
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    messages.Add(new Error(Properties.Resources.ErrorWritingResourceItemToResxFile));
                }
            }

            resxDocument.Save(this.stream, SaveOptions.OmitDuplicateNamespaces);

            if (messages.Count > 0)
            {
                ImportFailed fail = new ImportFailed();

                foreach (ImportMessage message in messages)
                {
                    fail.Messages.Add(message);
                }

                return fail;
            }

            return new ImportSucceeded();
        }
    }
}