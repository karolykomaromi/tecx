namespace Cars.Test.Parts
{
    using Cars.Parts;
    using Xunit;

    public class PackageTests
    {
        [Fact]
        public void Should_Replace_Parts_And_Package()
        {
            var alu = new Part("17_ALU", new[] { new PartNumber("16_STEEL") }, PartNumber.None);

            var surplus = new Package("SURPLUS", new[] { new PartNumber("BASIC") }, PartNumber.None) { alu };

            Assert.Contains(new PartNumber("16_STEEL"), surplus.ReplacesTheseParts);
            Assert.Contains(new PartNumber("BASIC"), surplus.ReplacesTheseParts);
        }
    }
}
