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
    }
}
