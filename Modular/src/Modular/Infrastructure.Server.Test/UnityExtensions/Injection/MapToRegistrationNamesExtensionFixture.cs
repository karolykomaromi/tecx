namespace Infrastructure.Server.Test.UnityExtensions.Injection
{
    using System.Windows.Input;
    using Infrastructure.Server.Test.TestObjects;
    using Infrastructure.UnityExtensions.Injection;
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;

    public class MapToRegistrationNamesExtensionFixture
    {
        [Fact]
        public void Should_Map_On_Non_Default_Names()
        {
            var searchCommand = new Mock<ICommand>();
            var suggestionsCommand = new Mock<ICommand>();
            var searchService = new Mock<ISearchService>();

            var container = new UnityContainer().AddNewExtension<MapToRegistrationNamesExtension>();
            container.RegisterType<MapToRegNameViewModel>(new MapToRegistrationNames());
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

            var container = new UnityContainer().AddNewExtension<MapToRegistrationNamesExtension>();
            container.RegisterInstance(dontMap);
            container.RegisterType<Consumer1>(new MapToRegistrationNames());

            Consumer1 sut = container.Resolve<Consumer1>();

            Assert.Same(dontMap, sut.DontMapToRegName);
        }
    }
}
