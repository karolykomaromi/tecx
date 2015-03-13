namespace Hydra.Infrastructure.Test.Extensions
{
    using System.Collections.Generic;
    using Hydra.Infrastructure.Extensions;
    using Xunit;

    public class DictionaryTests
    {
        [Fact]
        public void Should_Not_Throw_Totally_Useless_Exception_On_Duplicate_Key()
        {
            IDictionary<int, string> sut = new Dictionary<int, string>().Wrap();

            sut.Add(1, "1");

            DuplicateKeyException exception = Assert.Throws<DuplicateKeyException>(() => sut.Add(1, "2"));

            Assert.Equal("1", exception.DuplicateKey);

            exception = Assert.Throws<DuplicateKeyException>(() => sut.Add(new KeyValuePair<int, string>(1, "3")));

            Assert.Equal("1", exception.DuplicateKey);
        }

        [Fact]
        public void Should_Not_Throw_Totally_Useless_Exception_On_Key_Not_Found()
        {
            IDictionary<int, string> sut = new Dictionary<int, string>().Wrap();

            var exception = Assert.Throws<MissingKeyException>(() => sut[1]);

            Assert.Equal("1", exception.MissingKey);
        }
    }
}