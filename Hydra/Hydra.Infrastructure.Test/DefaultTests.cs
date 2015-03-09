namespace Hydra.Infrastructure.Test
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Hydra.Infrastructure.Configuration;
    using Xunit;

    public class DefaultTests
    {
        private interface IFoo
        {
        }

        private interface IBar
        {
        }

        [Fact]
        public void Should_Use_Null_Field_On_Class()
        {
            Assert.Same(Stream.Null, Default.Value<Stream>());
        }

        [Fact]
        public void Should_Use_Empty_Field_On_Class()
        {
            Assert.Same(SettingName.Empty, Default.Value<SettingName>());
        }

        [Fact]
        public void Should_Use_Null_Field_From_Class_By_Matching_Interface_Name()
        {
            Assert.Same(Bar.Null, Default.Value<IBar>());
        }

        [Fact]
        public void Should_Use_Empty_Field_From_Class_By_Matching_Interface_Name()
        {
            Assert.Same(Foo.Empty, Default.Value<IFoo>());
        }

        [Fact]
        public void Default_For_String_Should_Be_Empty_String()
        {
            Assert.Equal(string.Empty, Default.Value<string>());
        }

        [Fact]
        public void Default_For_Array_Should_Be_Empty_Array()
        {
            Assert.IsType<int[]>(Default.Value<int[]>());
        }

        [Fact]
        public void Default_For_Generic_Collection_Interface_Should_Be_Empty_Array()
        {
            Assert.IsType<int[]>(Default.Value<IList<int>>());
            Assert.IsType<int[]>(Default.Value<ICollection<int>>());
            Assert.IsType<int[]>(Default.Value<IEnumerable<int>>());
        }

        [Fact]
        public void Default_For_Non_Generic_Collection_Interface_Should_Be_Empty_Array()
        {
            Assert.IsType<object[]>(Default.Value<IList>());
            Assert.IsType<object[]>(Default.Value<ICollection>());
            Assert.IsType<object[]>(Default.Value<IEnumerable>());
        }

        [Fact]
        public void Default_Should_Be_Empty_List()
        {
            Assert.IsType<List<int>>(Default.Value<List<int>>());
        }

        [Fact]
        public void Default_Should_Be_Empty_Dictionary()
        {
            Assert.IsType<Dictionary<string, long>>(Default.Value<Dictionary<string, long>>());
            Assert.IsType<Dictionary<string, long>>(Default.Value<IDictionary<string, long>>());
        }

        [Fact]
        public void Default_Should_Be_Empty_Stack()
        {
            Assert.IsType<Stack<string>>(Default.Value<Stack<string>>());
        }

        private class Foo : IFoo
        {
            public static readonly IFoo Empty = new Foo();
        }

        private class Bar : IBar
        {
            public static readonly IBar Null = new Bar();
        }
    }
}