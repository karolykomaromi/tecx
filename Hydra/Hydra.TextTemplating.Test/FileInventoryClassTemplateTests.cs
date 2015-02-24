namespace Hydra.TextTemplating.Test
{
    using System;
    using System.IO;
    using Xunit;

    public class FileInventoryClassTemplateTests
    {
        [Fact]
        public void Should_Generate_Constants_For_All_Files()
        {
            DirectoryInfo testFiles = new DirectoryInfo(@".\InventoryTestFiles");

            FileInventoryClassTemplate sut = new FileInventoryClassTemplate();

            sut.TakeStock(testFiles);

            string actual = sut.ToString();

            Assert.Contains("Hydra.TextTemplating.Test.InventoryTestFiles.Foo.cs", actual, StringComparison.Ordinal);
            Assert.Contains("Hydra.TextTemplating.Test.InventoryTestFiles.Bar.cs", actual, StringComparison.Ordinal);
            Assert.Contains("Hydra.TextTemplating.Test.InventoryTestFiles.CodeQualityRules.Baz.cs", actual, StringComparison.Ordinal);
            Assert.Contains("Hydra.TextTemplating.Test.InventoryTestFiles.NamingRules.Chat.txt", actual, StringComparison.Ordinal);
            Assert.Contains("Hydra.TextTemplating.Test.InventoryTestFiles.NamingRules.Chit.txt", actual, StringComparison.Ordinal);
        }
    }
}
