namespace Cars.CodeQuality.Test
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using StyleCop;

    public class ResourceBasedSourceCode : SourceCode
    {
        private readonly string manifestResourceName;

        private ResourceBasedSourceCode(string manifestResourceName, CodeProject project, SourceParser parser)
            : base(project, parser)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(manifestResourceName));
            Contract.Requires(typeof(ResourceBasedSourceCode).Assembly.GetManifestResourceNames().Contains(manifestResourceName, StringComparer.Ordinal));

            this.manifestResourceName = manifestResourceName;
        }

        public override bool Exists
        {
            get { return true; }
        }

        public override string Name
        {
            get { return this.manifestResourceName; }
        }

        public override string Path
        {
            get { return this.manifestResourceName; }
        }

        public override DateTime TimeStamp
        {
            get { return DateTime.MinValue; }
        }

        public override string Type
        {
            get { return "cs"; }
        }

        public static SourceCode Create(string path, CodeProject project, SourceParser parser, object context)
        {
            return new ResourceBasedSourceCode(path, project, parser);
        }

        public override TextReader Read()
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(this.manifestResourceName);

            if (stream == null)
            {
                throw new MissingManifestResourceException(Properties.Resources.NoResourceWithName);
            }

            return new StreamReader(stream);
        }
    }
}