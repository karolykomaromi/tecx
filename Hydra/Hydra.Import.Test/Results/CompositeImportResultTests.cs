namespace Hydra.Import.Test.Results
{
    using System.Linq;
    using Hydra.Import.Messages;
    using Hydra.Import.Results;
    using Xunit;

    public class CompositeImportResultTests
    {
        [Fact]
        public void Should_Properly_Compose_Infos()
        {
            ImportResult r1 = new ImportFailed();
            r1.Messages.Add(new Warning("W1"));
            ImportResult r2 = new ImportSucceeded();
            r2.Messages.Add(new Info("I2"));
            ImportResult r3 = new ImportFailed();
            r3.Messages.Add(new Warning("W3"));
            ImportResult r4 = new ImportFailed();
            r4.Messages.Add(new Error("E4"));
            r4.Messages.Add(new Error("E5"));

            ImportResult sut = new CompositeImportResult(r1, r2, r3, r4);

            Assert.Equal(1, sut.Messages.Infos.Count());
        }

        [Fact]
        public void Should_Properly_Compose_Warnings()
        {
            ImportResult r1 = new ImportFailed();
            r1.Messages.Add(new Warning("W1"));
            ImportResult r2 = new ImportSucceeded();
            r2.Messages.Add(new Info("I2"));
            ImportResult r3 = new ImportFailed();
            r3.Messages.Add(new Warning("W3"));
            ImportResult r4 = new ImportFailed();
            r4.Messages.Add(new Error("E4"));
            r4.Messages.Add(new Error("E5"));

            ImportResult sut = new CompositeImportResult(r1, r2, r3, r4);

            Assert.Equal(2, sut.Messages.Warnings.Count());
        }

        [Fact]
        public void Should_Properly_Compose_Errors()
        {
            ImportResult r1 = new ImportFailed();
            r1.Messages.Add(new Warning("W1"));
            ImportResult r2 = new ImportSucceeded();
            r2.Messages.Add(new Info("I2"));
            ImportResult r3 = new ImportFailed();
            r3.Messages.Add(new Warning("W3"));
            ImportResult r4 = new ImportFailed();
            r4.Messages.Add(new Error("E4"));
            r4.Messages.Add(new Error("E5"));

            ImportResult sut = new CompositeImportResult(r1, r2, r3, r4);

            Assert.Equal(2, sut.Messages.Errors.Count());
        }
    }
}
