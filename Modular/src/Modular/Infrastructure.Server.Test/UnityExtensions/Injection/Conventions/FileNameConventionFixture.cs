namespace Infrastructure.Server.Test.UnityExtensions.Injection.Conventions
{
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Xunit;

    public class FileNameConventionFixture
    {
        [Fact]
        public void Should_Match_String_On_Parameter_Names_That_Hint_To_File()
        {
            IParameterMatchingConvention convention = new FileNameConvention();

            Assert.True(convention.IsMatch(new Parameter("1"), new FakeParameterInfo("File", typeof(string))));
            Assert.True(convention.IsMatch(new Parameter("1"), new FakeParameterInfo("FileName", typeof(string))));
            Assert.True(convention.IsMatch(new Parameter("1"), new FakeParameterInfo("Path", typeof(string))));
        }
    }
}