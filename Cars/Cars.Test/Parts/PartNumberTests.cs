using Cars.Parts;
using Xunit;

namespace Cars.Test.Parts
{
    public class PartNumberTests
    {
        [Fact]
        public void Should_Upper_Case_Part_Number()
        {
            PartNumber sut = "color_Blue";

            Assert.Equal("COLOR_BLUE", sut.ToString());
        }

        [Fact]
        public void Should_Be_Equal()
        {
            PartNumber pn1 = "1";
            PartNumber pn2 = "1";

            Assert.True(pn1 == pn2);
        }
    }
}
