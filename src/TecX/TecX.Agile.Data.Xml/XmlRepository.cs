using System.IO;

using TecX.Agile.Data.File;

namespace TecX.Agile.Data.Xml
{
    public class XmlRepository : FileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRepository"/> class
        /// </summary>
        public XmlRepository(DirectoryInfo baseFolder)
            : base(baseFolder, new XmlProjectToStringConverter())
        {
        }

        public override string FileExtension
        {
            get { return ".xml"; }
        }
    }
}
