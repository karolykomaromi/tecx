using System;
using System.Collections.Generic;

namespace TecX.Playground.QueryAbstractionLayer
{
    using System.Linq;

    using Xunit;

    using TecX.Playground.QueryAbstractionLayer.PD;

    public class InterceptionFixture
    {
        [Fact]
        public void Should_Wrap()
        {
            IQueryable<Foo> query = new[] { new Foo { PrincipalId = 1 }, new Foo { PrincipalId = 2 }, new Foo { PrincipalId = 1337 } }.AsQueryable();

            //IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDOperator());
            IQueryable<Foo> intercepted = new QueryInterceptor<Foo>(query, new PDIteratorOperator()).Where(f => string.IsNullOrEmpty(f.Bar));

            Assert.Equal(1, intercepted.Count());
        }

        [Fact]
        public void Show_Usage()
        {
            ISession session = new SessionImpl();

            PDIteratorOperator pdOperator = new PDIteratorOperator();

            IQueryable<Foo> query = session.Query<Foo>(pdOperator);

            // ...
        }
    }

    public static class Extensions
    {
        public static IQueryable<T> Query<T>(this ISession session, PDIteratorOperator pdOperator = null)
            where T : PersistentObject
        {
            pdOperator = pdOperator ?? new PDIteratorOperator();

            var builder = new FooBuilder();

            IEnumerable<Foo> foos = builder.Build(10);

            IQueryable<Foo> rawQuery = foos.AsQueryable();

            IQueryable<Foo> interceptedQuery = new QueryInterceptor<Foo>(rawQuery, pdOperator);

            return interceptedQuery as IQueryable<T>;
        }
    }

    public class FooBuilder
    {
        private int nextID = 1;

        private long id;

        private long principalId;

        private string bar;

        private bool isDeleted;

        public IEnumerable<Foo> Build(int numberOfFoosToCreate)
        {
            List<Foo> foos = new List<Foo>();

            for (int i = 0; i < numberOfFoosToCreate; i++)
            {
                Foo foo = this.WithId(nextID++).Build();

                foos.Add(foo);
            }

            return foos;
        }

        public Foo Build()
        {
            return new Foo{ PDO_ID = this.id, PrincipalId = GetPrincipalID(), Bar = GetBar(), IsDeleted = this.isDeleted };
        }

        public FooBuilder WithId(long id)
        {
            this.id = id;

            return this;
        }

        public FooBuilder WithIsDeleted(bool isDeleted)
        {
            this.isDeleted = isDeleted;

            return this;
        }

        public FooBuilder WithBar(string bar)
        {
            this.bar = bar;

            return this;
        }

        private long GetPrincipalID()
        {
            if (this.principalId != 0)
            {
                return this.principalId;
            }

            if (this.principalId%2 == 0)
            {
                return 42;
            }

            return 1337;
        }

        private string GetBar()
        {
            if (string.IsNullOrEmpty(this.bar))
            {
                return "MyID is " + this.id;
            }

            return this.bar;
        }
    }

    /// <summary>
    /// Dummy interface simulating NHibernate's ISession
    /// </summary>
    public interface ISession
    {
    }

    /// <summary>
    /// Dummy implementation of dummy interface to demonstrate usage
    /// </summary>
    public class SessionImpl : ISession
    {
    }
}
