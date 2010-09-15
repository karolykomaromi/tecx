using TecX.Common;

namespace TecX.Unity.Registration
{
    public static class From
    {
        public static FolderAssemblyLocator Folder(string path)
        {
            Guard.AssertNotEmpty(path, "path");

            return new FolderAssemblyLocator(path);
        }
    }
}