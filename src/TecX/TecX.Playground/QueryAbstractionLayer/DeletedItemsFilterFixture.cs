namespace TecX.Playground.QueryAbstractionLayer
{
    using System;

    using TecX.Playground.QueryAbstractionLayer.Filters;
    using TecX.Playground.QueryAbstractionLayer.PD;

    using Xunit;

    public class DeletedItemsFilterFixture
    {
        [Fact]
        public void Exclude_Should_BeFalseOnDeleted()
        {
            Assert.False(DeletedItemsFilter.Exclude.Filter<Foo>().Compile()(new Foo { PDO_DELETED = DateTime.Now }));
        }

        [Fact]
        public void Exclude_Should_BeTrueOnNotDeleted()
        {
            Assert.True(DeletedItemsFilter.Exclude.Filter<Foo>().Compile()(new Foo { PDO_DELETED = null }));
        }

        [Fact]
        public void Include_Should_AlwaysBeTrue()
        {
            Assert.True(DeletedItemsFilter.Include.Filter<Foo>().Compile()(new Foo { PDO_DELETED = DateTime.Now }));
            Assert.True(DeletedItemsFilter.Include.Filter<Foo>().Compile()(new Foo { PDO_DELETED = null }));
        }
    }
}