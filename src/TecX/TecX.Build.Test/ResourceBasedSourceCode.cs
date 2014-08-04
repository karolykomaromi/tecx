using System;
using System.IO;
using System.Linq;
using System.Reflection;
using StyleCop;
using TecX.Common;

namespace TecX.Build.Test
{
    public class ResourceBasedSourceCode : SourceCode
    {
        private readonly string _ResourceName;

        public ResourceBasedSourceCode(CodeProject project, SourceParser parser, string resourceName)
            : this(project, parser, resourceName, Assembly.GetExecutingAssembly())
        {
            
        }

        public ResourceBasedSourceCode(CodeProject project, SourceParser parser, string resourceName, Assembly assembly)
            : base(project, parser)
        {
            Guard.AssertNotEmpty(resourceName, "resourceName");
            Guard.AssertNotNull(assembly, "assembly");

            string fullResourceName = assembly.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(fullResourceName))
            {
                throw new ArgumentException(string.Format("No resource has a name that ends with '{0}'", resourceName), "resourceName");
            }

            _ResourceName = fullResourceName;
        }

        public static SourceCode Create(string path, CodeProject project, SourceParser parser, object context)
        {
            return new ResourceBasedSourceCode(project, parser, path);
        }

        public override TextReader Read()
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(_ResourceName);

            return new StreamReader(stream);
        }

        public override bool Exists
        {
            get { return true; }
        }

        public override string Name
        {
            get { return _ResourceName; }
        }

        public override string Path
        {
            get { return _ResourceName; }
        }

        public override DateTime TimeStamp
        {
            get { return DateTime.MinValue; }
        }

        public override string Type
        {
            get { return "cs"; }
        }
    }
}