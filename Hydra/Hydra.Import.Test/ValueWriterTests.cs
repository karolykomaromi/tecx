namespace Hydra.Import.Test
{
    using Xunit;

    public class ValueWriterTests
    {
        [Fact]
        public void Value_Writers_With_Same_Type_And_Name_Should_Be_Equal()
        {
            IValueWriter vw1 = new NullValueWriter("Foo");
            IValueWriter vw2 = new NullValueWriter("Foo");

            Assert.Equal(vw1, vw2);
        }

        [Fact]
        public void Should_Reuse_NullValueWriters()
        {
            IValueWriter vw1 = ValueWriter.Null("Foo");
            IValueWriter vw2 = ValueWriter.Null("Foo");

            Assert.Same(vw1, vw2);
        }
    }
}
