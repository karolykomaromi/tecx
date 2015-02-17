namespace Hydra.CodeQuality.Test
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Hydra.CodeQuality.Test.Properties;
    using StyleCop;

    public class ResourceBasedSourceCode : SourceCode
    {
        private readonly string manifestResourceName;

        private ResourceBasedSourceCode(string resourceName, CodeProject project, SourceParser parser)
            : base(project, parser)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(resourceName));

            Assembly assembly = typeof(ResourceBasedSourceCode).Assembly;

            string fullResourceName = assembly.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith("." + resourceName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(fullResourceName))
            {
                throw new ArgumentException(string.Format(Resources.NoResourceNameEndsWith, resourceName), "resourceName");
            }

            this.manifestResourceName = fullResourceName;
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
                throw new MissingManifestResourceException(Resources.NoResourceWithName);
            }

            return new StreamReader(stream);
        }
    }
}