namespace Hydra.TextTemplating
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class FileInventoryClassTemplate
    {
        private const string DefaultIndentation = "    ";

        private readonly StringBuilder sb;
        
        public FileInventoryClassTemplate()
        {
            this.sb = new StringBuilder(100);
            this.UseThisForIndentation = DefaultIndentation;
        }

        public string UseThisForIndentation { get; set; }

        public void TakeStock(DirectoryInfo directory, int indentLevel = 0)
        {
            Contract.Requires(indentLevel >= 0);
            Contract.Requires(directory.Exists);

            if (directory != null)
            {
                string indentation = this.Indentation(indentLevel);

                this.sb.Append(indentation).Append("public static class ").AppendLine(directory.Name);
                this.sb.Append(indentation).AppendLine("{");

                FileInfo[] files = directory.GetFiles().OrderBy(f => f.Name).ToArray();

                for (int index = 0; index < files.Length; index++)
                {
                    FileInfo file = files[index];

                    this.WriteConstantForFile(file, indentLevel + 1);

                    if (index < files.Length - 1)
                    {
                        this.sb.AppendLine();
                    }
                }

                DirectoryInfo[] subDirectories = directory.GetDirectories().OrderBy(d => d.Name).ToArray();

                if (files.Length > 0 && subDirectories.Length > 0)
                {
                    this.sb.AppendLine();
                }

                for (int index = 0; index < subDirectories.Length; index++)
                {
                    DirectoryInfo subDirectory = subDirectories[index];

                    this.TakeStock(subDirectory, indentLevel + 1);

                    if (index < subDirectories.Length - 1)
                    {
                        this.sb.AppendLine();
                    }
                }

                this.sb.Append(indentation).AppendLine("}");
            }
        }

        public override string ToString()
        {
            return this.sb.ToString();
        }

        private static string ConvertFileNameToManifestResourceName(FileInfo file)
        {
            int idx = file.FullName.IndexOf("Hydra", StringComparison.Ordinal);

            string manifestResourceName = file.FullName.Substring(idx + 6)
                .Replace(@"\", ".")
                .Replace("bin.Debug.", string.Empty)
                .Replace("bin.Release.", string.Empty);

            return manifestResourceName;
        }

        private string Indentation(int indentLevel)
        {
            return string.Concat(Enumerable.Repeat(this.UseThisForIndentation ?? DefaultIndentation, indentLevel));
        }

        private void WriteConstantForFile(FileInfo file, int indentLevel)
        {
            string indentation = this.Indentation(indentLevel);

            string manifestResourceName = ConvertFileNameToManifestResourceName(file);

            this.sb.Append(indentation).AppendLine(@"/// <summary>");
            this.sb.Append(indentation).Append(@"/// ").AppendLine(manifestResourceName);
            this.sb.Append(indentation).AppendLine(@"/// </summary>");

            this.sb.Append(indentation)
                .Append("public const string ")
                .Append(file.Name.Replace(file.Extension, string.Empty))
                .Append(" = \"")
                .Append(manifestResourceName)
                .AppendLine("\";");
        }
    }
}