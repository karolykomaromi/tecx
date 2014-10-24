namespace Hydra.Infrastructure.Test.Input
{
    using System;
    using System.Linq;
    using Hydra.Infrastructure.Input;
    using Xunit;
    using Xunit.Extensions;

    public class CmdLineParameterParserTests
    {
        [Theory, ContainerData]
        public void Should_Recognize_Simple_Parameter(CmdLineParameterParser sut)
        {
            string s = @"/d foobar";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(1, actual.Length);
            Assert.Equal("d", actual[0].Name, StringComparer.Ordinal);
            Assert.Equal("foobar", actual[0].Value, StringComparer.Ordinal);
        }

        [Theory, ContainerData]
        public void Should_Recognize_Parameter_Without_Value(CmdLineParameterParser sut)
        {
            string s = @"/d";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(1, actual.Length);
            Assert.Equal("d", actual[0].Name, StringComparer.Ordinal);
            var p = Assert.IsType<NoValueCmdLineParameter>(actual[0]);
            Assert.Equal(string.Empty, p.Value);
        }

        [Theory, ContainerData]
        public void Should_Recognize_Parameter_Without_Value_In_Between_Other_Parameters(CmdLineParameterParser sut)
        {
            string s = @"/d foobar snafu /z /x baz ";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(3, actual.Length);
            Assert.Equal("d", actual[0].Name, StringComparer.Ordinal);

            var mp = Assert.IsType<MultiValueCmdLineParameter>(actual[0]);
            Assert.Equal(2, mp.Values.Count);

            Assert.Equal("z", actual[1].Name);
            var np = Assert.IsType<NoValueCmdLineParameter>(actual[1]);
            Assert.Equal(string.Empty, np.Value);

            Assert.Equal("x", actual[2].Name);
            var p = Assert.IsType<CmdLineParameter>(actual[2]);
            Assert.Equal("baz", p.Value);
        }

        [Theory, ContainerData]
        public void Should_Recognize_Parameter_With_Multiple_Values(CmdLineParameterParser sut)
        {
            string s = @"/d foobar snafu";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(1, actual.Length);
            Assert.Equal("d", actual[0].Name, StringComparer.Ordinal);
            Assert.Equal("foobar snafu", actual[0].Value, StringComparer.Ordinal);
        }

        [Theory, ContainerData]
        public void Should_Return_MultiValue_For_Parameter_With_Multiple_Values(CmdLineParameterParser sut)
        {
            string s = @"/d foobar snafu";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(1, actual.Length);
            var p = Assert.IsType<MultiValueCmdLineParameter>(actual[0]);
            Assert.Equal(2, p.Values.Count);
        }

        [Theory, ContainerData]
        public void Should_Recognize_Multiple_Parameters_With_One_Or_Multiple_Values(CmdLineParameterParser sut)
        {
            string s = @"/d foobar snafu /x baz";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(2, actual.Length);
            Assert.Equal("d", actual[0].Name, StringComparer.Ordinal);
            Assert.Equal("foobar snafu", actual[0].Value, StringComparer.Ordinal);
            Assert.Equal("x", actual[1].Name, StringComparer.Ordinal);
            Assert.Equal("baz", actual[1].Value, StringComparer.Ordinal);
        }

        [Theory, ContainerData]
        public void Should_Recognize_Multiple_Parameters(CmdLineParameterParser sut)
        {
            string s = @"/d foobar /x snafu";

            var actual = sut.Parse(s).ToArray();

            Assert.Equal(2, actual.Length);
            Assert.Equal("d", actual[0].Name, StringComparer.Ordinal);
            Assert.Equal("foobar", actual[0].Value, StringComparer.Ordinal);
            Assert.Equal("x", actual[1].Name, StringComparer.Ordinal);
            Assert.Equal("snafu", actual[1].Value, StringComparer.Ordinal);
        }
    }
}
