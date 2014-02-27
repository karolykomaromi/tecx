namespace Modular.Web.Test.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Modular.Web.Hosting;
    using Xunit;

    public class EmbeddedDirectoryFixture
    {
        [Fact]
        public void Should_Merge_Directories_Without_SubDirectories()
        {
            Assembly assembly = this.GetType().Assembly;

            EmbeddedFile file1 = new EmbeddedFile("~/Texts/Long/VeryLong.txt", "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt", assembly);
            EmbeddedDirectory dir1 = new EmbeddedDirectory("~/Texts/Long/", null, new[] { file1 });

            EmbeddedFile file2 = new EmbeddedFile("~/Texts/Long/AnotherLong.txt", "Modular.Web.Test.Assets.Texts.Long.AnotherLong.txt", assembly);
            EmbeddedDirectory dir2 = new EmbeddedDirectory("~/Texts/Long/", null, new[] { file2 });

            EmbeddedDirectory merged = EmbeddedDirectory.Merge(dir1, dir2);

            Assert.Equal("~/Texts/Long/", merged.AppRelativePath);
            Assert.Contains(file1, merged.Files.OfType<EmbeddedFile>());
            Assert.Contains(file2, merged.Files.OfType<EmbeddedFile>());
        }

        [Fact]
        public void Should_Merge_Directories_With_Identical_SubDirectories()
        {
            Assembly assembly = this.GetType().Assembly;

            EmbeddedDirectory subDir1 = new EmbeddedDirectory("~/Texts/Long/", null, null);
            EmbeddedDirectory dir1 = new EmbeddedDirectory("~/Texts/", new[] { subDir1 }, null);

            EmbeddedDirectory subDir2 = new EmbeddedDirectory("~/Texts/Long/", null, null);
            EmbeddedDirectory dir2 = new EmbeddedDirectory("~/Texts/", new[] { subDir2 }, null);

            EmbeddedDirectory merged = EmbeddedDirectory.Merge(dir1, dir2);

            Assert.Equal(1, merged.Directories.OfType<EmbeddedDirectory>().Count());
            Assert.Equal("~/Texts/Long/", merged.Directories.OfType<EmbeddedDirectory>().Single().AppRelativePath, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Merge_List_Of_Two_Identical_Directories()
        {
            EmbeddedDirectory dir1 = new EmbeddedDirectory("~/Texts/", null, null);
            EmbeddedDirectory dir2 = new EmbeddedDirectory("~/Texts/", null, null);
            var unmerged = new List<EmbeddedDirectory> { dir1, dir2 };

            var merged = EmbeddedDirectory.Merge(unmerged).ToList();

            Assert.Equal(1, merged.Count);
            Assert.Equal("~/Texts/", merged[0].AppRelativePath);
        }

        [Fact]
        public void Should_Merge_List_Of_Three_Identical_Directories()
        {
            EmbeddedDirectory dir1 = new EmbeddedDirectory("~/Texts/", null, null);
            EmbeddedDirectory dir2 = new EmbeddedDirectory("~/Texts/", null, null);
            EmbeddedDirectory dir3 = new EmbeddedDirectory("~/Texts/", null, null);

            var unmerged = new List<EmbeddedDirectory> { dir1, dir2, dir3 };

            var merged = EmbeddedDirectory.Merge(unmerged).ToList();

            Assert.Equal(1, merged.Count);
            Assert.Equal("~/Texts/", merged[0].AppRelativePath);
        }

        [Fact]
        public void Should_Merge_List_Of_Two_Identical_And_One_Off_Directories()
        {
            EmbeddedDirectory dir1 = new EmbeddedDirectory("~/Texts/", null, null);
            EmbeddedDirectory dir2 = new EmbeddedDirectory("~/Texts/", null, null);
            EmbeddedDirectory dir3 = new EmbeddedDirectory("~/Texts/Long/", null, null);

            var unmerged = new List<EmbeddedDirectory> { dir1, dir2, dir3 };

            var merged = EmbeddedDirectory.Merge(unmerged).OrderBy(d => d.AppRelativePath.Length).ToList();

            Assert.Equal(2, merged.Count);
            Assert.Equal("~/Texts/", merged[0].AppRelativePath);
            Assert.Equal("~/Texts/Long/", merged[1].AppRelativePath);
        }
    }
}