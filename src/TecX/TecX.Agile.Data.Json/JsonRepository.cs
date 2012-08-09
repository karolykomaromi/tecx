using System.IO;

using TecX.Agile.Data.File;

namespace TecX.Agile.Data.Json
{
    public class JsonRepository : FileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRepository"/> class
        /// </summary>
        public JsonRepository(DirectoryInfo baseFolder)
            : base(baseFolder, new JsonProjectToStringConverter())
        {
        }

        public override string FileExtension
        {
            get { return ".json"; }
        }
    }
}
