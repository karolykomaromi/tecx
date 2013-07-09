using TecX.Playground.QueryAbstractionLayer.Filters;
using TecX.Playground.QueryAbstractionLayer.PD;

using Xunit;

namespace TecX.Playground.QueryAbstractionLayer
{
    public class DeletedItemsFilterFixture
    {
        [Fact]
        public void Exclude_Should_BeFalseOnDeleted()
        {
            Assert.False(DeletedItemsFilter.Exclude.Filter<Foo>().Compile()(new Foo { IsDeleted = true }));
        }

        [Fact]
        public void Exclude_Should_BeTrueOnNotDeleted()
        {
            Assert.True(DeletedItemsFilter.Exclude.Filter<Foo>().Compile()(new Foo { IsDeleted = false }));
        }

        [Fact]
        public void Include_Should_AlwaysBeTrue()
        {
            Assert.True(DeletedItemsFilter.Include.Filter<Foo>().Compile()(new Foo { IsDeleted = true }));
            Assert.True(DeletedItemsFilter.Include.Filter<Foo>().Compile()(new Foo { IsDeleted = false }));
        }
    }
}