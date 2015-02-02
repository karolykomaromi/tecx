namespace Hydra.Import
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Xml.Linq;
    using Hydra.Infrastructure.I18n;

    public class ResXFileDataReader : DataReader<ResourceItem>
    {
        private readonly Stream stream;

        private readonly CultureInfo culture;

        public ResXFileDataReader(Stream stream, CultureInfo culture)
        {
            Contract.Requires(stream != null);
            Contract.Requires(culture != null);

            this.stream = stream;
            this.culture = culture;
        }

        public override IEnumerator<ResourceItem> GetEnumerator()
        {
            XDocument document = XDocument.Load(this.stream);

            if (document.Root == null)
            {
                this.Messages.Add(new Error(Properties.Resources.ErrorResxFileStreamMissingRootElement));
                yield break;
            }

            foreach (XElement dataElement in document.Root.Descendants("data"))
            {
                XAttribute n;
                if ((n = dataElement.Attribute("name")) == null)
                {
                    continue;
                }

                string name = n.Value;

                XElement v;
                if ((v = dataElement.Element("value")) == null)
                {
                    continue;
                }

                string value = v.Value;

                ResourceItem ri = new ResourceItem
                                  {
                                      Language = this.culture,
                                      Name = name,
                                      Value = value
                                  };

                yield return ri;
            }
        }
    }
}