namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.Injection;
    using TecX.Unity.Test.TestObjects;

    using Xunit;

    public class MapParameterNamesToRegistrationNamesFixture
    {
        [Fact]
        public void CanResolveCtorParametersUsingParamNamesAsRegistrationNames()
        {
            var container = new UnityContainer();

            container.AddNewExtension<MapParameterNamesToRegistrationNamesExtension>();

            container.RegisterType<ICommand, LoadCommand>("loadCommand");
            container.RegisterType<ICommand, SaveCommand>("saveCommand");

            container.RegisterType<ViewModel>(new MapParameterNamesToRegistrationNames());

            var vm = container.Resolve<ViewModel>();

            Assert.IsType(typeof(LoadCommand), vm.LoadCommand);
            Assert.IsType(typeof(SaveCommand), vm.SaveCommand);
        }
    }
}
