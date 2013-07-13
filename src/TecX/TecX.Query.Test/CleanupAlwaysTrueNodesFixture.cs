namespace TecX.Query.Test
{
    using System;
    using System.Linq.Expressions;

    using TecX.Query.PD;
    using TecX.Query.Visitors;

    using Xunit;

    public class CleanupAlwaysTrueNodesFixture
    {
        [Fact]
        public void Should_Remove_LeftSide_AlwaysTrueExpressions()
        {
            Expression<Func<Foo, bool>> where = foo => (true && string.IsNullOrEmpty(foo.Description));

            Expression clean = new CleanupAlwaysTrueNodes().Visit(where);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(clean);

            Assert.False(visitor.Found);
        }

        [Fact]
        public void Should_Remove_RightSide_AlwaysTrueExpressions()
        {
            Expression<Func<Foo, bool>> where = foo => (string.IsNullOrEmpty(foo.Description) && true);

            Expression clean = new CleanupAlwaysTrueNodes().Visit(where);

            var visitor = new ContainsAlwaysTrueNode();

            visitor.Visit(clean);

            Assert.False(visitor.Found);
        }

        [Fact]
        public void Should_Remove_BothSide_AlwaysTrueExpressions()
        {
            Expression<Func<Foo, bool>> where = foo => (true && string.IsNullOrEmpty(foo.Description) && true);

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
