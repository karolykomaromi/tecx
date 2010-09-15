using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class FolderAssemblyLocator : AssemblyLocator
    {
        #region Fields

        private readonly DirectoryInfo _folder;

        #endregion Fields

        #region c'tor

        public FolderAssemblyLocator(string path)
            : this(new DirectoryInfo(path))
        {
        }

        public FolderAssemblyLocator(DirectoryInfo folder)
        {
            Guard.AssertNotNull(folder, "folder");

            _folder = folder;
        }

        #endregion c'tor
        
        #region Overrides of AssemblyLocator

        public override IEnumerable<Assembly> GetAssemblies()
        {
            if (!_folder.Exists)
                throw new DirectoryNotFoundException("Directory does not exist");

            var assemblyFiles = _folder.GetFiles()
                .Where(f => f.Extension.EndsWith("dll", StringComparison.OrdinalIgnoreCase) ||
                            f.Extension.EndsWith("exe", StringComparison.OrdinalIgnoreCase));

            List<Assembly> assemblies = new List<Assembly>();

            foreach (FileInfo file in assemblyFiles)
            {
                Assembly assembly = Assembly.LoadFrom(file.FullName);
                assemblies.Add(assembly);
            }

            return assemblies;
        }

        #endregion Overrides of AssemblyLocator
    }
}