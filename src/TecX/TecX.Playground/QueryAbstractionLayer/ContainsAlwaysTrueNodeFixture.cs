namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Linq.Expressions;

    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Visitors;

    using Xunit;

    public class ContainsAlwaysTrueNodeFixture
    {
        [Fact]
        public void Should_Find_AlwaysTrueNodes()
        {
            Expression<Func<Foo, bool>> where = foo => (string.IsNullOrEmpty(foo.Bar) && true);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(where);

            Assert.True(visitor.Found);
        }

        [Fact]
        public void Should_Ignore_IfNoAlwaysTrueNodes()
        {
            Expression<Func<Foo, bool>> where = foo => string.IsNullOrEmpty(foo.Bar);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(where);

            Assert.False(visitor.Found);
        }
    }
}