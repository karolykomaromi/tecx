using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Janus.TextTemplating
{
    public class ResourcesTemplate
    {
        private readonly XDocument document;

        private string indents;

        public ResourcesTemplate(XDocument document)
        {
            Contract.Requires(document != null);
            Contract.Requires(document.Root != null);

            this.document = document;
            this.indents = "    ";
        }

        public static ResourcesTemplate FromStream(Stream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<ResourcesTemplate>() != null);

            XDocument document = XDocument.Load(stream);

            return new ResourcesTemplate(document);
        }

        public static ResourcesTemplate FromNode(XDocument document)
        {
            Contract.Requires(document != null);
            Contract.Ensures(Contract.Result<ResourcesTemplate>() != null);

            return new ResourcesTemplate(document);
        }

        public ResourcesTemplate UseTabs()
        {
            Contract.Ensures(Contract.Result<ResourcesTemplate>() != null);

            this.indents = "\t";

            return this;
        }

        public ResourcesTemplate UseSpaces()
        {
            Contract.Ensures(Contract.Result<ResourcesTemplate>() != null);

            this.indents = "    ";

            return this;
        }

        public ResourcesTemplate IndentWith(string indents)
        {
            Contract.Requires(!string.IsNullOrEmpty(indents));
            Contract.Ensures(Contract.Result<ResourcesTemplate>() != null);

            this.indents = indents;

            return this;
        }

        public string Properties()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            StringBuilder sb = new StringBuilder(500);

            foreach (XElement data in this.document.Root.Descendants("data"))
            {
                string name = (string)data.Attribute("name");

                string value = (string)data.Element("value");

                sb.AppendLine(IsBinaryResx(value)
                    ? this.Property(p => p.Name(name).ByteArray())
                    : this.Property(p => p.Name(name).String()));
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.Properties();
        }

        private static bool IsBinaryResx(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            string[] s = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Type type;
            return s.Length == 2 &&
                   (type = Type.GetType(s[1], false)) != null &&
                   type == typeof(byte[]);
        }

        private string Property(Action<PropertyBuilder> action)
        {
            var builder = new PropertyBuilder(this.indents);

            action(builder);

            return builder;
        }
    }
}