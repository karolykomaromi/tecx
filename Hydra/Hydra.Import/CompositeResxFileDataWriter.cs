namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Logging;

    public class CompositeResXFileDataWriter : IDataWriter<ResourceItem>, IDisposable
    {
        private readonly string baseName;

        private readonly string directoryForResXFiles;

        private readonly HashSet<Stream> streams;

        private readonly IDictionary<CultureInfo, ResXFileDataWriter> targetFiles;

        public CompositeResXFileDataWriter(string baseName, string directoryForResXFiles)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseName));
            Contract.Requires(!string.IsNullOrWhiteSpace(directoryForResXFiles));
            Contract.Requires(Directory.Exists(directoryForResXFiles));

            this.baseName = baseName.EndsWith(".", StringComparison.Ordinal) ? baseName : baseName + ".";
            this.directoryForResXFiles = directoryForResXFiles;
            this.targetFiles = new Dictionary<CultureInfo, ResXFileDataWriter>();
            this.streams = new HashSet<Stream>();
        }

        public ImportResult Write(IEnumerable<ResourceItem> items)
        {
            foreach (ResourceItem ri in items)
            {
                if (ri.Language == null)
                {
                    continue;
                }

                ResXFileDataWriter writer;
                if (!this.targetFiles.TryGetValue(ri.Language, out writer))
                {
                    string resxFilePath = Path.Combine(this.directoryForResXFiles, this.baseName + ri.Language.Name + ".resources");

                    Stream stream = new FileStream(resxFilePath, FileMode.Create);

                    writer = new ResXFileDataWriter(stream);

                    this.targetFiles.Add(ri.Language, writer);

                    this.streams.Add(stream);
                }

                writer.Write(new[] { ri });
            }

            return new ImportFailed();
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Stream stream in this.streams)
                {
                    try
                    {
                        stream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                    }
                }
            }
        }
    }
}