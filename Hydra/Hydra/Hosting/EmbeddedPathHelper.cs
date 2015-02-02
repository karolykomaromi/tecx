namespace Hydra.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public static class EmbeddedPathHelper
    {
        public static string ToAppRelative(string assemblyName, string manifestResourceName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(assemblyName));
            Contract.Requires(!string.IsNullOrWhiteSpace(manifestResourceName));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            int idx = manifestResourceName.IndexOf(assemblyName, StringComparison.Ordinal);

            string path = "~/" + manifestResourceName.Substring(idx + assemblyName.Length + 1);

            path = path.Replace('.', '/');

            idx = path.LastIndexOf("/", StringComparison.Ordinal);

            path = path.Remove(idx, 1);

            path = path.Insert(idx, ".");

            return path;
        }

        public static IEnumerable<string> GetDirectories(string assemblyName, string manifestResourceName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(assemblyName));
            Contract.Requires(!string.IsNullOrWhiteSpace(manifestResourceName));
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            int idx = manifestResourceName.IndexOf(assemblyName, StringComparison.OrdinalIgnoreCase);

            string path = manifestResourceName.Substring(idx + assemblyName.Length + 1);

            string[] parts = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i <= parts.Length - 2; i++)
            {
                string directory = "~/" + string.Join("/", parts.Take(i)) + "/";

                yield return directory;
            }
        }

        public static EmbeddedDirectory ToDirectoryStructure(Assembly assembly, string manifestResourceName)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(!string.IsNullOrEmpty(manifestResourceName));
            Contract.Ensures(Contract.Result<EmbeddedDirectory>() != null);

            string assemblyName = assembly.GetName().Name;

            string[] directories = EmbeddedPathHelper.GetDirectories(assemblyName, manifestResourceName).OrderByDescending(d => d.Length).ToArray();

            string fileName = EmbeddedPathHelper.ToAppRelative(assemblyName, manifestResourceName);

            EmbeddedFile file = new EmbeddedFile(fileName, manifestResourceName, assembly);

            EmbeddedDirectory subDir = new EmbeddedDirectory(directories[0], null, new[] { file });

            foreach (string directory in directories.Skip(1))
            {
                subDir = new EmbeddedDirectory(directory, new[] { subDir }, null);
            }

            return subDir;
        }

        public static EmbeddedDirectory ToDirectoryStructure(Assembly assembly)
        {
            Contract.Requires(assembly != null);
            Contract.Ensures(Contract.Result<EmbeddedDirectory>() != null);

            string[] manifestResourceNames = assembly.GetManifestResourceNames();

            var directories = manifestResourceNames.Select(manifestResourceName => EmbeddedPathHelper.ToDirectoryStructure(assembly, manifestResourceName));

            directories = EmbeddedDirectory.Merge(directories);

            return new EmbeddedDirectory("~/", directories, null);
        }

        public static EmbeddedDirectory ToDirectoryStructure(params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                return new EmbeddedDirectory("~/", null, null);
            }

            if (assemblies.Length == 1)
            {
                return EmbeddedPathHelper.ToDirectoryStructure(assemblies[0]);
            }

            var directories = from assembly in assemblies
                              from res in assembly.GetManifestResourceNames()
                              select EmbeddedPathHelper.ToDirectoryStructure(assembly, res);

            directories = EmbeddedDirectory.Merge(directories);

            return new EmbeddedDirectory("~/", directories, null);
        }
    }
}