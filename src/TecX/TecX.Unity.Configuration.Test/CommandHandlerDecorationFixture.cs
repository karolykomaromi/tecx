namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;
    using TecX.Unity.Decoration;

    [TestClass]
    public class CommandHandlerDecorationFixture
    {
        [TestMethod]
        public void CanUseConventionToRegisterOpenGenericHandlers()
        {
            var builder = new ConfigurationBuilder();

            var convention = new CommandHandlerConvention().WithTransaction().WithDeadlockRetry();

            builder.Extension<DecoratorExtension>();
            builder.Scan(x =>
                {
                    x.With(convention);
                    x.AssemblyContainingType(typeof(BarCommand));
                });

            var container = new UnityContainer();

            container.AddExtension(builder);

            ICommandHandler<BarCommand> barHandler = container.Resolve<ICommandHandler<BarCommand>>("BarHandler");

            var cmd = new BarCommand();

            barHandler.Handle(cmd);

            Assert.AreEqual("-->Retry-->Transaction-->BarHandler", cmd.HandledBy);
        }

        [TestMethod]
        public void CanUseManualStacking()
        {
            var handler = new DeadlockRetryHandler<BarCommand>(new TransactionHandler<BarCommand>(new BarHandler()));

            var cmd = new BarCommand();

            handler.Handle(cmd);

            Assert.AreEqual("-->Retry-->Transaction-->BarHandler", cmd.HandledBy);
        }
    }
}
