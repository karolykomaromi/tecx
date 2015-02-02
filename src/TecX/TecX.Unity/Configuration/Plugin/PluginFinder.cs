namespace TecX.Unity.Configuration.Plugin
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    public class PluginFinder
    {
        private readonly ConfigurationBuilder parent;

        private readonly FileSystemWatcher watcher;

        public PluginFinder(ConfigurationBuilder parent, string path)
        {
            Guard.AssertNotNull(parent, "parent");
            Guard.AssertNotEmpty(path, "path");

            if (!Directory.Exists(path))
            {
                string msg = string.Format("Directory '{0}' does not exist. Can't monitor non-existent folders.", path);
                throw new ArgumentException(msg, "path");
            }

            this.parent = parent;

            this.watcher = new FileSystemWatcher(path);

            this.watcher.Created += this.OnFileCreated;
        }

        public void ImportBuildersFromNewAssembly(string assemblyFile)
        {
            Guard.AssertNotEmpty(assemblyFile, "assemblyFile");

            if (!File.Exists(assemblyFile))
            {
                throw new ArgumentException(string.Format("File '{0}' does not exist.", assemblyFile));
            }

            if (!IsAssemblyFilePath(assemblyFile))
            {
                throw new ArgumentException(string.Format("File '{0}' is not an assembly (extension is neither *.dll nor *.exe).", assemblyFile));
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFile);

            var builderTypes = assembly.GetExportedTypes().Where(ConfigurationBuilder.IsPublicBuilder);

            foreach (var builder in builderTypes)
            {
                ConfigurationBuilder child = (ConfigurationBuilder)Activator.CreateInstance(builder);

                this.parent.ImportBuilder(child);
            }
        }

        private static bool IsAssemblyFilePath(string assemblyFile)
        {
            return assemblyFile.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) ||
                   assemblyFile.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase);
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            string assemblyFile = e.FullPath;

            if (IsAssemblyFilePath(assemblyFile))
            {
                this.ImportBuildersFromNewAssembly(assemblyFile);
            }
        }
    }
}
