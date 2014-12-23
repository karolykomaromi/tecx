namespace Hydra.Import.Test
{
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class ValueWriterCollectionBuilderTests
    {
        [Fact]
        public void Should_Create_Writers_For_All_Public_Writable_Properties()
        {
            ValueWriterCollection collection = ValueWriterCollectionBuilder.ForAllPropertiesOf<ValueWriterTestObject>().Build();

            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.DateTime)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.String)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Int32)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Decimal)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Double)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Float)]);
        }
    }
}
