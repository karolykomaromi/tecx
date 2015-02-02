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

            Assert.IsType<DateTimeValueWriter>(collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.DateTime)]);
            Assert.IsType<StringValueWriter>(collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.String)]);
            Assert.IsType<Int32ValueWriter>(collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Int32)]);
            Assert.IsType<DecimalValueWriter>(collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Decimal)]);
            Assert.IsType<DoubleValueWriter>(collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Double)]);
            Assert.IsType<FloatValueWriter>(collection[TypeHelper.GetPropertyName((ValueWriterTestObject x) => x.Float)]);
        }
    }
}
