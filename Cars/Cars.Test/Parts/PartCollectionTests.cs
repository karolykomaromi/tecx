using Cars.Parts;
using Xunit;

namespace Cars.Test.Parts
{
    public class PartCollectionTests
    {
        [Fact]
        public void Should_Replace_Steel_Wheels_With_Alu_Wheels()
        {
            var steel = new Part("16_STEEL");
            var alu = new Part("17_ALU", new [] { new PartNumber("16_STEEL") }, PartNumber.None);

            var sut = new PartCollection();

            Assert.Equal(0, sut.AddNewAndReplaceExisting(steel).Count);
            Assert.Equal(1, sut.AddNewAndReplaceExisting(alu).Count);
        }

        [Fact]
        public void Should_Not_Add_Part_Multiple_Time()
        {
            var steel = new Part("16_STEEL");

            var sut = new PartCollection();

            Assert.Equal(0, sut.AddNewAndReplaceExisting(steel).Count);
            Assert.Equal(0, sut.AddNewAndReplaceExisting(steel).Count);
        }
    }
}