namespace Modular.Web.Hosting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web.Hosting;

    [DebuggerDisplay("Path={AppRelativePath} Directories={directories.Count} Files={files.Count}")]
    public class EmbeddedDirectory : VirtualDirectory
    {
        private readonly List<EmbeddedFile> files;
        private readonly List<EmbeddedDirectory> directories;
        private readonly string appRelativePath;

        public EmbeddedDirectory(string appRelativePath, IEnumerable<EmbeddedDirectory> subDirectories, IEnumerable<EmbeddedFile> files)
            : base(appRelativePath)
        {
            Contract.Requires(!string.IsNullOrEmpty(appRelativePath));

            this.appRelativePath = appRelativePath;
            this.files = new List<EmbeddedFile>(files ?? new EmbeddedFile[0]);
            this.directories = new List<EmbeddedDirectory>(subDirectories ?? new EmbeddedDirectory[0]);
        }

        public ReadOnlyCollection<EmbeddedDirectory> EmbeddedDirectories
        {
            get { return new ReadOnlyCollection<EmbeddedDirectory>(this.directories); }
        }

        public ReadOnlyCollection<EmbeddedFile> EmbeddedFiles
        {
            get { return new ReadOnlyCollection<EmbeddedFile>(this.files); }
        }

        public override IEnumerable Directories
        {
            get
            {
                return this.directories;
            }
        }

        public override IEnumerable Files
        {
            get
            {
                return this.files;
            }
        }

        public override IEnumerable Children
        {
            get
            {
                foreach (var directory in this.directories)
                {
                    yield return directory;
                }

                foreach (var file in this.files)
                {
                    yield return file;
                }
            }
        }

        public string AppRelativePath
        {
            get { return this.appRelativePath; }
        }

        public static EmbeddedDirectory Merge(EmbeddedDirectory dir1, EmbeddedDirectory dir2)
        {
            Contract.Requires(dir1 != null);
            Contract.Requires(dir2 != null);
            Contract.Ensures(Contract.Result<EmbeddedDirectory>() != null);

            if (!string.Equals(dir1.AppRelativePath, dir2.AppRelativePath, StringComparison.OrdinalIgnoreCase))
            {
                string msg = string.Format(
                        "You are trying to merge two directories with different paths. Dir1='{0}'. Dir2='{1}'.",
                        dir1.AppRelativePath,
                        dir2.AppRelativePath);

                throw new InvalidOperationException(msg);
            }

            return new EmbeddedDirectory(dir1.AppRelativePath, Merge(dir1.directories.Concat(dir2.directories)), dir1.files.Concat(dir2.files));
        }

        public static IEnumerable<EmbeddedDirectory> Merge(IEnumerable<EmbeddedDirectory> directories)
        {
            Contract.Requires(directories != null);
            Contract.Ensures(Contract.Result<IEnumerable<EmbeddedDirectory>>() != null);

            var groupedByPath = directories.GroupBy(d => d.AppRelativePath, StringComparer.OrdinalIgnoreCase);

            foreach (var groupByPath in groupedByPath)
            {
                var group = new List<EmbeddedDirectory>(groupByPath);

                if (group.Count == 1)
                {
                    yield return group[0];
                }
                else if (group.Count == 2)
                {
                    yield return Merge(group[0], group[1]);
                }
                else
                {
                    EmbeddedDirectory merged = Merge(group[0], group[1]);

                    for (int i = 2; i < group.Count; i++)
                    {
                        merged = Merge(merged, group[i]);
                    }

                    yield return merged;
                }
            }
        }

        public void Accept(EmbeddedVisitor visitor)
        {
            visitor.Visit(this);

            foreach (var directory in this.directories)
            {
                directory.Accept(visitor);
            }

            foreach (var file in this.files)
            {
                file.Accept(visitor);
            }
        }

        public override int GetHashCode()
        {
            return this.AppRelativePath.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            EmbeddedDirectory other = obj as EmbeddedDirectory;

            if (other == null)
            {
                return false;
            }

            return string.Equals(this.AppRelativePath, other.AppRelativePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}