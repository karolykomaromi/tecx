using Moq;

namespace TecX.Playground
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public class FromPlainToGenericFixture
    {
        [Fact]
        public void Should_Be_Able_To_Fake_Items_With_Generic_Interface()
        {
            var iterator = new Mock<IIterator<FrameworkBaseClass>>();

            var items = new List<FrameworkBaseClass>
                {
                    new FrameworkBaseClass {Id = 1},
                    new FrameworkBaseClass {Id = 2},
                    new FrameworkBaseClass {Id = 3}
                };

            iterator.Setup(i => i.GetEnumerator()).Returns(items.GetEnumerator());

            int count = 0;
            foreach (FrameworkBaseClass item in iterator.Object)
            {
                count++;
                Assert.Equal(count, item.Id);
            }

            Assert.Equal(3, count);
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
