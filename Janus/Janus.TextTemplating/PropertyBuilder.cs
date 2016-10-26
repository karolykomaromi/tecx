using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Janus.TextTemplating
{
    public class PropertyBuilder : Builder<string>
    {
        private readonly string indents;

        private string name;

        private string type;

        private string body;

        public PropertyBuilder(string indents)
        {
            Contract.Requires(!string.IsNullOrEmpty(indents));

            this.indents = indents;
            this.name = string.Empty;
            this.type = string.Empty;
            this.body = string.Empty;
        }

        public PropertyBuilder Name(string name)
        {
            Contract.Ensures(Contract.Result<PropertyBuilder>() != null);

            this.name = name;

            return this;
        }

        public PropertyBuilder String()
        {
            Contract.Ensures(Contract.Result<PropertyBuilder>() != null);

            this.type = "string";

            StringBuilder sb = new StringBuilder(100);

            sb.Append(this.Indent(4)).AppendLine("return ResourceManager.GetString(\"{0}\", resourceCulture);");

            this.body = sb.ToString();

            return this;
        }

        public PropertyBuilder ByteArray()
        {
            Contract.Ensures(Contract.Result<PropertyBuilder>() != null);

            this.type = "byte[]";

            StringBuilder sb = new StringBuilder(100);

            sb.Append(this.Indent(4)).AppendLine("object obj = ResourceManager.GetObject(\"{0}\", resourceCulture);");
            sb.Append(this.Indent(4)).AppendLine("return (byte[])obj;");

            this.body = sb.ToString();

            return this;
        }

        public override string Build()
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(this.name));
            Contract.Requires(!string.IsNullOrWhiteSpace(this.type));
            Contract.Requires(!string.IsNullOrWhiteSpace(this.body));
            Contract.Ensures(Contract.Result<string>() != null);

            StringBuilder sb = new StringBuilder(500);

            sb.AppendLine();
            sb.Append(this.Indent(2)).Append("public static ").Append(this.type).Append(" ").AppendLine(name);
            sb.AppendLine(this.OpenCurlyBrace(2));
            sb.Append(this.Indent(3)).AppendLine("get");
            sb.AppendLine(this.OpenCurlyBrace(3));
            sb.AppendFormat(this.body, this.name);
            sb.AppendLine(this.CloseCurlyBrace(3));
            sb.Append(this.CloseCurlyBrace(2));

            return sb.ToString();
        }

        private string Indent(int indentationLevel)
        {
            return string.Concat(Enumerable.Repeat(this.indents, indentationLevel));
        }

        private string OpenCurlyBrace(int indentationLevel)
        {
            return this.Indent(indentationLevel) + "{";
        }

        private string CloseCurlyBrace(int indentationLevel)
        {
            return this.Indent(indentationLevel) + "}";
        }
    }
}