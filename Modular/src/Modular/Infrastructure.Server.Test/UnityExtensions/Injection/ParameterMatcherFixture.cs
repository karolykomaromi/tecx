using Infrastructure.Server.Test.TestObjects;

namespace Infrastructure.Server.Test.UnityExtensions.Injection
{
    using System.Linq;
    using System.Reflection;
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Xunit;

    public class ParameterMatcherFixture
    {
        [Fact]
        public void Should_Find_Ctors_That_Accept_All_Specified_Parameters()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new[] { new Parameter("connectionString", SmartConstructorFixture.Constants.ConnectionStringValue) },
                new CompositeConvention(
                    new StringAsMappingNameConvention(),
                    new SpecifiedNameConvention(),
                    new ConnectionStringConvention(),
                    new FileNameConvention(),
                    new ByTypeConvention()));

            var ctors = typeof(CtorTest).GetConstructors();

            var result = ctors.Where(matcher.ConstructorTakesAllParameters);

            // must find Ctortest(string,ILogger), CtorTest(string,ILogger,string) and CtorTest(string,ILogger,int)
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Should_Not_Match_Single_Provided_String_Parameter_To_Multiple_Ctor_Arguments()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new[] { new Parameter("connectionString", SmartConstructorFixture.Constants.ConnectionStringValue) },
                new CompositeConvention(
                    new StringAsMappingNameConvention(),
                    new SpecifiedNameConvention(),
                    new ConnectionStringConvention(),
                    new FileNameConvention(),
                    new ByTypeConvention()));

            var ctor = typeof(CtorTest).GetConstructor(new[] { typeof(string), typeof(ILogger), typeof(string) });

            Assert.False(matcher.AllPrimitiveArgsSatisfied(ctor));
        }

        [Fact]
        public void Should_Find_Ctors_All_Primitive_Arguments_Are_Satisfied()
        {
            ParameterMatcher matcher = new ParameterMatcher(
                new[] { new Parameter("connectionString", SmartConstructorFixture.Constants.ConnectionStringValue) },
                new CompositeConvention(
                    new StringAsMappingNameConvention(),
                    new SpecifiedNameConvention(),
                    new ConnectionStringConvention(),
                    new FileNameConvention(),
                    new ByTypeConvention()));

            var ctors = typeof(CtorTest).GetConstructors();

            ConstructorInfo[] result = ctors.Where(matcher.AllPrimitiveArgsSatisfied).ToArray();

            // must find CtorTest(ILogger) and CtorTest(string,ILogger)
            // the other two have primitive parameters that are not satisfied
            Assert.Equal(2, result.Length);
            Assert.Equal(1, result[0].GetParameters().Length);
            Assert.Equal(2, result[1].GetParameters().Length);
        }
    }
}