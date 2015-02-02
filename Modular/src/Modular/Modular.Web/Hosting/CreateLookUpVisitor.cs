namespace Modular.Web.Hosting
{
    using System;
    using System.Collections.Generic;

    public class CreateLookUpVisitor : EmbeddedVisitor
    {
        private readonly IDictionary<string, EmbeddedFile> files;
        private readonly IDictionary<string, EmbeddedDirectory> directories;

        public CreateLookUpVisitor()
        {
            this.files = new Dictionary<string, EmbeddedFile>(StringComparer.OrdinalIgnoreCase);
            this.directories = new Dictionary<string, EmbeddedDirectory>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, EmbeddedFile> Files
        {
            get { return this.files; }
        }

        public IDictionary<string, EmbeddedDirectory> Directories
        {
            get { return this.directories; }
        }

        public override void Visit(EmbeddedDirectory directory)
        {
            if (!this.Directories.ContainsKey(directory.AppRelativePath))
            {
                this.Directories.Add(directory.AppRelativePath, directory);
            }
        }

        public override void Visit(EmbeddedFile file)
        {
            if (!this.Files.ContainsKey(file.AppRelativePath))
            {
                this.Files.Add(file.AppRelativePath, file);
            }
        }
    }
}