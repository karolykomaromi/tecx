namespace TecX.Playground
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public class FromPlainToGenericFixture
    {
        [Fact]
        public void Should()
        {
            
        }
    }

    public interface IIterator
    {
        T UniqueResult<T>() where T : FrameworkBaseClass;
    }

    public class Iterator : IIterator, IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new FrameworkBaseClass { Id = 1 };
            yield return new FrameworkBaseClass { Id = 2 };
            yield return new FrameworkBaseClass { Id = 3 };
        }

        public T UniqueResult<T>()
            where T : FrameworkBaseClass
        {
            return (T)new FrameworkBaseClass { Id = 1 };
        }
    }

    public interface IIterator<out T> : IIterator, IEnumerable<T>
        where T : FrameworkBaseClass
    {
        new T UniqueResult();
    }

    public class Iterator<T> : IIterator<T> where T : FrameworkBaseClass
    {
        private readonly Iterator iterator;

        public Iterator(Iterator iterator)
        {
            this.iterator = iterator;
        }

        public T UniqueResult()
        {
            return this.iterator.UniqueResult<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.iterator.OfType<T>().GetEnumerator();
        }

        #region Explicit Interface Implementations

        T1 IIterator.UniqueResult<T1>()
        {
            return iterator.UniqueResult<T1>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion Explicit Interface Implementations
    }

    public class FrameworkBaseClass
    {
        public long Id { get; set; }
    }
}
