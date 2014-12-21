namespace Hydra.Import.Test
{
    using System;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class ValueWriterCollectionBuilderTests
    {
        [Fact]
        public void Should_Create_Writers_For_All_Public_Writable_Properties()
        {
            ValueWriterCollection collection = new ValueWriterCollectionBuilder<HasLotsOfProperties>().ForAll().Build();

            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((HasLotsOfProperties x) => x.Timestamp)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((HasLotsOfProperties x) => x.Description)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((HasLotsOfProperties x) => x.Price)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((HasLotsOfProperties x) => x.Quantity)]);
            Assert.NotSame(ValueWriter.Null, collection[TypeHelper.GetPropertyName((HasLotsOfProperties x) => x.Length)]);
        }
    }

    public class HasLotsOfProperties
    {
        public DateTime Timestamp { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public double Length { get; set; }
    }
}
