namespace TecX.Search.Test.TestObjects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;

    public class InMemoryDbSet<T> : IDbSet<T>
        where T : class
    {
        private readonly HashSet<T> set;

        private readonly IQueryable<T> queryableSet;

        public InMemoryDbSet()
            : this(Enumerable.Empty<T>())
        {
        }

        public InMemoryDbSet(IEnumerable<T> entities)
        {
            Guard.AssertNotNull(entities, "entities");

            this.set = new HashSet<T>();

            foreach (var entity in entities)
            {
                this.set.Add(entity);
            }

            this.queryableSet = this.set.AsQueryable();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression
        {
            get
            {
                return this.queryableSet.Expression;
            }
        }

        public Type ElementType
        {
            get
            {
                return this.queryableSet.ElementType;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return this.queryableSet.Provider;
            }
        }

        public T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            Guard.AssertNotNull(entity, "entity");

            this.set.Add(entity);

            return entity;
        }

        public T Remove(T entity)
        {
            Guard.AssertNotNull(entity, "entity");

            this.set.Remove(entity);

            return entity;
        }

        public T Attach(T entity)
        {
            Guard.AssertNotNull(entity, "entity");

            this.set.Add(entity);

            return entity;
        }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<T> Local
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
