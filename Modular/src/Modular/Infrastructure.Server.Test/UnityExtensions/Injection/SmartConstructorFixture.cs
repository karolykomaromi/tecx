namespace Infrastructure.Server.Test.UnityExtensions.Injection
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Infrastructure.Server.Test.TestObjects;
    using Infrastructure.UnityExtensions.Injection;
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;

    public class SmartConstructorFixture
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed. Suppression is OK here.")]
        public static class Constants
        {
            /// <summary>blablub</summary>
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
            public const string ConnectionStringValue = "blablub";
        }

        [Fact]
        public void Should_Use_Specified_Parameters_And_Auto_Resolve_Others()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<ICtorTest, CtorTest>(
                new SmartConstructor("connectionString", Constants.ConnectionStringValue));

            ICtorTest sut = container.Resolve<ICtorTest>();
            Assert.NotNull(sut);
        }

        [Fact]
        public void Should_Correctly_Match_Multiple_Parameters_Of_Same_Type()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, TestLogger>();

            container.RegisterType<CtorTest>(new SmartConstructor(
                new Parameter("connectionString", Constants.ConnectionStringValue),
                new Parameter("anotherParam", "I'm a string")));

            CtorTest sut = container.Resolve<CtorTest>();
            
            Assert.Equal(Constants.ConnectionStringValue, sut.ConnectionString);
            Assert.Equal("I'm a string", sut.AnotherParam);
        }
        
        [Fact]
        public void Should_Match_Specified_Parameter()
        {
            var container = new UnityContainer();

            var instance = new MatchByConvention();

            container.RegisterType<MyService>(
                new SmartConstructor(new[] { new Parameter(instance) }));

            var sut = container.Resolve<MyService>();

            Assert.NotNull(sut.Convention);
            Assert.Same(instance, sut.Convention);
        }

        [Fact]
        public void Should_Invoke_Default_Ctor_When_No_Better_Match()
        {
            var container = new UnityContainer();

            container.RegisterType<DefaultCtorOnly>(new SmartConstructor());

            var sut = container.Resolve<DefaultCtorOnly>();

            Assert.NotNull(sut);
        }

        [Fact]
        public void Should_Override_String_Dependency_With_Value_From_Anonymous_Object()
        {
            var container = new UnityContainer();

            container.RegisterType<ClassWithStringCtorParameter>(new SmartConstructor(new { someString = "1" }));

            var sut = container.Resolve<ClassWithStringCtorParameter>();

            Assert.Equal("1", sut.SomeString);
        }

        [Fact]
        public void Should_Override_Interface_Dependency_With_Value_From_Anonymous_Object()
        {
            var container = new UnityContainer();

            container.RegisterType<IFoo, Foo>("Bar");

            container.RegisterType<ClassWithInterfaceCtorParameter>(new SmartConstructor(new { foo = "Bar" }));

            var sut = container.Resolve<ClassWithInterfaceCtorParameter>();

            Assert.IsType(typeof(Foo), sut.Foo);
        }

        [Fact]
        public void Should_Override_Class_Type_Dependendy_With_Value_From_Anonymous_Object()
        {
            var container = new UnityContainer();

            container.RegisterType<ClassWithInterfaceCtorParameter>(new SmartConstructor(new { foo = new Foo() }));
            
            var sut = container.Resolve<ClassWithInterfaceCtorParameter>();

            Assert.IsType(typeof(Foo), sut.Foo);
        }

        [Fact]
        public void Should_Map_On_Non_Default_Names()
        {
            var searchCommand = new Mock<ICommand>();
            var suggestionsCommand = new Mock<ICommand>();
            var searchService = new Mock<ISearchService>();

            var container = new UnityContainer();
            container.RegisterType<MapToRegNameViewModel>(new SmartConstructor());
            container.RegisterInstance<ICommand>("searchCommand", searchCommand.Object);
            container.RegisterInstance<ICommand>("suggestionsCommand", suggestionsCommand.Object);
            container.RegisterInstance<ISearchService>(searchService.Object);

            MapToRegNameViewModel vm = container.Resolve<MapToRegNameViewModel>();

            Assert.Same(vm.SearchCommand, searchCommand.Object);
            Assert.Same(vm.SuggestionsCommand, suggestionsCommand.Object);
            Assert.Same(vm.SearchService, searchService.Object);
        }

        [Fact]
        public void Should_Not_Map_When_Param_Name_Equals_Type_Name()
        {
            DontMapToRegName dontMap = new DontMapToRegName();

            var container = new UnityContainer();
            container.RegisterInstance(dontMap);
            container.RegisterType<Consumer1>(new SmartConstructor());

            Consumer1 sut = container.Resolve<Consumer1>();

            Assert.Same(dontMap, sut.DontMapToRegName);
        }
    }
}
