namespace TecX.TestTools
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    public class EmbeddedResourceExtractor
    {
        public static class Defaults
        {
            /// <summary>
            /// Resources
            /// </summary>
            public const string ResourceFolder = "Resources";
        }

        public void Extract()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            this.Extract(assembly, Defaults.ResourceFolder);
        }

        public void Extract(Assembly assembly, string resourceFolderName)
        {
            Guard.AssertNotNull(assembly, "assembly");
            Guard.AssertNotEmpty(resourceFolderName, "resourceFolderName");

            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                // remove all parts up to the name of the resource folder. they are related to the namespace of the assembly
                string[] nameParts =
                    resourceName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                        .SkipWhile(part => !part.Equals(resourceFolderName, StringComparison.OrdinalIgnoreCase))
                        .ToArray();

                // need at least one folder in file path
                if (nameParts.Length > 2)
                {
                    // filename + extension
                    string fileName = nameParts[nameParts.Length - 2] + "." + nameParts[nameParts.Length - 1];

                    string[] filePathParts = nameParts.Take(nameParts.Length - 2).ToArray();

                    string filePath = Path.Combine(filePathParts);

                    DirectoryInfo resourceFolder = new DirectoryInfo(filePath);

                    if (!resourceFolder.Exists)
                    {
                        resourceFolder.Create();
                    }

                    FileInfo resourceFile = new FileInfo(Path.Combine(filePath, fileName));

                    if (!resourceFile.Exists)
                    {
                        using (Stream output = resourceFile.Create())
                        {
                            using (Stream input = assembly.GetManifestResourceStream(resourceName))
                            {
                                if (input != null)
                                {
                                    input.CopyTo(output);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}