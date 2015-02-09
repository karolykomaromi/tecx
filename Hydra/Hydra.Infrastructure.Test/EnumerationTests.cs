namespace Hydra.Infrastructure.Test
{
    using Xunit;

    public class EnumerationTests
    {
        [Fact]
        public void Should_Cast_Enumeration_Value_To_Int32()
        {
            Assert.Equal(1, (int)Numbers.One);
        }

        [Fact]
        public void Should_Parse_Name_Correctly()
        {
            object o;
            Assert.True(Enumeration.TryParse(typeof(Numbers), "one", out o));
            Assert.Same(Numbers.One, o);
        }
    }
}
