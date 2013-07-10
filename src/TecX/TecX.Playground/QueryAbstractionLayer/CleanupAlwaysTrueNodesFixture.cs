namespace TecX.Playground.QueryAbstractionLayer
{
    using System;
    using System.Linq.Expressions;

    using TecX.Playground.QueryAbstractionLayer.PD;
    using TecX.Playground.QueryAbstractionLayer.Visitors;

    using Xunit;
    public class CleanupAlwaysTrueNodesFixture
    {
        [Fact]
        public void Should_Remove_LeftSide_AlwaysTrueExpressions()
        {
            Expression<Func<Foo, bool>> where = foo => (true && string.IsNullOrEmpty(foo.Bar));

            Expression clean = new CleanupAlwaysTrueNodes().Visit(where);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(clean);

            Assert.False(visitor.Found);
        }

        [Fact]
        public void Should_Remove_RightSide_AlwaysTrueExpressions()
        {
            Expression<Func<Foo, bool>> where = foo => (string.IsNullOrEmpty(foo.Bar) && true);

            Expression clean = new CleanupAlwaysTrueNodes().Visit(where);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(clean);

            Assert.False(visitor.Found);
        }

        [Fact]
        public void Should_Remove_BothSide_AlwaysTrueExpressions()
        {
            Expression<Func<Foo, bool>> where = foo => (true && string.IsNullOrEmpty(foo.Bar) && true);

            Expression clean = new CleanupAlwaysTrueNodes().Visit(where);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(clean);

            Assert.False(visitor.Found);
        }

        [Fact]
        public void Should_Not_Touch_AlwaysTrueNode_IfNotPartOfAndNode()
        {
            Expression<Func<Foo, bool>> where = foo => true;

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(where);

            Assert.True(visitor.Found);
        }
    }
}
