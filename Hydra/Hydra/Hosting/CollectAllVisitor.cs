namespace Hydra.Hosting
{
    using System.Collections.Generic;

    public class CollectAllVisitor : EmbeddedVisitor
    {
        private readonly List<EmbeddedFile> files;
        private readonly List<EmbeddedDirectory> directories;

        public CollectAllVisitor()
        {
            this.files = new List<EmbeddedFile>();
            this.directories = new List<EmbeddedDirectory>();
        }

        public ICollection<EmbeddedFile> Files
        {
            get { return this.files; }
        }

        public ICollection<EmbeddedDirectory> Directories
        {
            get { return this.directories; }
        }

        public override void Visit(EmbeddedDirectory directory)
        {
            this.Directories.Add(directory);
        }

        public override void Visit(EmbeddedFile file)
        {
            this.Files.Add(file);
        }
    }
}