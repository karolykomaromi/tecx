namespace Hydra.CodeQuality.Test
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class FileInventoryClassTemplateTests
    {
        [Fact]
        public void Should_Generate_Constants_For_All_Files()
        {
            DirectoryInfo testFiles = new DirectoryInfo(@"D:\Evaluation\TecX\Hydra\Hydra.CodeQuality.Test\TestFiles");

            FileInventoryClassTemplate sut = new FileInventoryClassTemplate();

            sut.TakeStock(testFiles);

            string actual = sut.ToString();

            string[] manifestResourceNames = this.GetType().Assembly
                .GetManifestResourceNames()
                .Where(rn => !rn.EndsWith(".resources", StringComparison.Ordinal))
                .ToArray();

            foreach (string manifestResourceName in manifestResourceNames)
            {
                Assert.Contains(manifestResourceName, actual, StringComparison.Ordinal);
            }
        }
    }

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

                foreach (FileInfo file in directory.GetFiles().OrderBy(f => f.Name))
                {
                    this.WriteConstantForFile(file, indentLevel + 1);
                }

                foreach (DirectoryInfo subDirectory in directory.GetDirectories().OrderBy(d => d.Name))
                {
                    this.TakeStock(subDirectory, indentLevel + 1);
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

            string manifestResourceName = file.FullName.Substring(idx + 6).Replace(@"\", ".");

            return manifestResourceName;
        }

        private string Indentation(int indentLevel)
        {
            return string.Concat(Enumerable.Repeat(this.UseThisForIndentation ?? DefaultIndentation, indentLevel));
        }

        private void WriteConstantForFile(FileInfo file, int indentLevel)
        {
            string indentation = this.Indentation(indentLevel);

            this.sb.Append(indentation)
                .Append("public const string ")
                .Append(file.Name.Replace(file.Extension, string.Empty))
                .Append(" = \"")
                .Append(ConvertFileNameToManifestResourceName(file))
                .AppendLine("\";");
        }
    }
}
