namespace Hydra.Infrastructure.Test
{
    using Xunit;

    public class FlagsTests
    {
        [Fact]
        public void Should_Return_Name_Of_Default_If_Only_Value()
        {
            Colors sut = Colors.Default;
            Assert.Equal(Colors.None.Name, sut.Name);
        }

        [Fact]
        public void Should_Not_Return_Name_Of_Default_If_Multiple_Values()
        {
            Colors sut = Colors.Default | Colors.Red | Colors.Blue;

            Assert.Equal(Colors.Red.Name + ", " + Colors.Blue.Name, sut.Name);
        }

        [Fact]
        public void Should_Identify_Defined_Names_Regardless_Of_Case()
        {
            Assert.True(Colors.IsDefined("Blue"));
            Assert.True(Colors.IsDefined("blUE"));
        }

        [Fact]
        public void Should_Identify_Defined_Value()
        {
            Assert.True(Colors.IsDefined(1));
            Assert.True(Colors.IsDefined(4));
        }

        [Fact]
        public void Should_Identify_NonDefined_Names()
        {
            Assert.False(Colors.IsDefined("lilac"));
        }

        [Fact]
        public void Should_Identify_NonDefined_Values()
        {
            Assert.False(Colors.IsDefined(4711));
        }
    }
}
