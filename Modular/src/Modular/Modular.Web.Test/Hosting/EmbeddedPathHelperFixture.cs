namespace Modular.Web.Test.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Modular.Web.Hosting;
    using Xunit;

    public class EmbeddedPathHelperFixture
    {
        [Fact]
        public void Should_Transform_ManifestResourceName_To_App_Relative_Path()
        {
            string manifestResourceName = "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt";

            string expected = "~/Texts/Long/VeryLong.txt";

            string actual = EmbeddedPathHelper.ToAppRelative(manifestResourceName);

            Assert.Equal(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Get_AppRelative_Directories_From_ManifestResourceName()
        {
            string manifestResourceName = "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt";

            IEnumerable<string> expected = new[] { "~/Texts/", "~/Texts/Long/" };

            IEnumerable<string> actual = EmbeddedPathHelper.GetDirectories(manifestResourceName);

            Assert.Equal(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_Produce_Correct_Directory_Structure()
        {
            Assembly assembly = this.GetType().Assembly;
            string manifestResourceName = "Modular.Web.Test.Assets.Texts.Long.VeryLong.txt";

            EmbeddedDirectory dir = EmbeddedPathHelper.ToDirectoryStructure(assembly, manifestResourceName);

            Assert.NotNull(dir);
            Assert.Equal("~/Texts/", dir.AppRelativePath);
            Assert.Equal(1, dir.EmbeddedDirectories.Count);
            Assert.Equal(0, dir.EmbeddedFiles.Count);

            EmbeddedDirectory subDir = dir.EmbeddedDirectories[0];
            Assert.Equal("~/Texts/Long/", subDir.AppRelativePath);
            Assert.Equal(0, subDir.EmbeddedDirectories.Count);
            Assert.Equal(1, subDir.EmbeddedFiles.Count);

            EmbeddedFile file = subDir.EmbeddedFiles[0];
            Assert.Equal("~/Texts/Long/VeryLong.txt", file.AppRelativePath);
            Assert.Equal("Modular.Web.Test.Assets.Texts.Long.VeryLong.txt", file.ResourceName);
        }

        [Fact]
        public void Should_Produce_Correct_Directory_Structure_For_Assembly()
        {
            Assembly assembly = this.GetType().Assembly;

            EmbeddedDirectory root = EmbeddedPathHelper.ToDirectoryStructure(assembly);

            Assert.Equal(2, root.EmbeddedDirectories.Count);
            Assert.Equal(0, root.EmbeddedFiles.Count);

            EmbeddedDirectory texts = root.EmbeddedDirectories.First(d => d.AppRelativePath == "~/Texts/");
            Assert.Equal(2, texts.EmbeddedDirectories.Count);

            EmbeddedDirectory @long = texts.EmbeddedDirectories.First(d => d.AppRelativePath == "~/Texts/Long/");
            Assert.Equal(2, @long.EmbeddedFiles.Count);
            EmbeddedFile anotherLongTxt = @long.EmbeddedFiles.FirstOrDefault(f => f.AppRelativePath == "~/Texts/Long/AnotherLong.txt");
            Assert.NotNull(anotherLongTxt);
            EmbeddedFile veryLongTxt = @long.EmbeddedFiles.FirstOrDefault(f => f.AppRelativePath == "~/Texts/Long/VeryLong.txt");
            Assert.NotNull(veryLongTxt);

            EmbeddedDirectory @short = texts.EmbeddedDirectories.First(d => d.AppRelativePath == "~/Texts/Short/");
            Assert.Equal(1, @short.EmbeddedFiles.Count);
            EmbeddedFile veryShortTxt = @short.EmbeddedFiles.FirstOrDefault(f => f.AppRelativePath == "~/Texts/Short/VeryShort.txt");
            Assert.NotNull(veryShortTxt);

            EmbeddedDirectory images = root.EmbeddedDirectories.First(d => d.AppRelativePath == "~/Images/");
            Assert.Equal(1, images.EmbeddedFiles.Count);
            EmbeddedFile redPng = images.EmbeddedFiles.FirstOrDefault(f => f.AppRelativePath == "~/Images/Red.png");
            Assert.NotNull(redPng);
        }
    }
}
