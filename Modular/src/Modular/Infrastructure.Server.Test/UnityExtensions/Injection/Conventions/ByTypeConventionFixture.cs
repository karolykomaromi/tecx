namespace Infrastructure.Server.Test.UnityExtensions.Injection.Conventions
{
    using System.Reflection;
    using Infrastructure.Server.Test.TestObjects;
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Injection.Conventions;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class ByTypeConventionFixture
    {
        [Fact]
        public void Should_Match_Argument_Type_To_Param_Type()
        {
            IParameterMatchingConvention convention = new ByTypeConvention();

            Parameter parameter = new Parameter(new Foo());

            ParameterInfo info = new FakeParameterInfo("Abc", typeof(Foo));

            Assert.True(convention.IsMatch(parameter, info));
        }

        [Fact]
        public void Should_Match_Interface_Argument_Type_To_Concrete_Param_Type()
        {
            IParameterMatchingConvention convention = new ByTypeConvention();

            Parameter parameter = new Parameter(new Foo());

            ParameterInfo info = new FakeParameterInfo("Abc", typeof(IFoo));

            Assert.True(convention.IsMatch(parameter, info));
        }

        [Fact]
        public void Should_Match_Resolved_Parameter_To_Param_Type()
        {
            IParameterMatchingConvention convention = new ByTypeConvention();

            Parameter parameter = new Parameter(new ResolvedParameter(typeof(Foo)));

            ParameterInfo info = new FakeParameterInfo("Abc", typeof(Foo));

            Assert.True(convention.IsMatch(parameter, info));
        }

        [Fact]
        public void Should_Match_Interface_Argument_Type_To_Resolved_Param_Type()
        {
            IParameterMatchingConvention convention = new ByTypeConvention();

            Parameter parameter = new Parameter(new ResolvedParameter(typeof(IFoo)));

            ParameterInfo info = new FakeParameterInfo("Abc", typeof(IFoo));

            Assert.True(convention.IsMatch(parameter, info));
        }

        [Fact]
        public void Should_Match_By_Various_Snippets_Of_Type_Name()
        {
            IParameterMatchingConvention convention = new ByTypeConvention();

            var parameter = new Parameter(new NamingConventionTestObject());

            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("NamingConventionTestObject", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("ConventionTestObject", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("TestObject", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("Object", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("SomeBaseClass", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("BaseClass", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("Class", typeof(NamingConventionTestObject))));
            Assert.True(convention.IsMatch(parameter, new FakeParameterInfo("Foo", typeof(NamingConventionTestObject))));
        }

        [Fact]
        public void Should_Not_Match_ConnectionString_Parameter_To_String_Argument()
        {
            IParameterMatchingConvention convention = new ByTypeConvention();

            var parameter = new Parameter("connectionString", "cs");

            Assert.False(convention.IsMatch(parameter, new FakeParameterInfo("AnotherParam", typeof(string))));
        }
    }
}
