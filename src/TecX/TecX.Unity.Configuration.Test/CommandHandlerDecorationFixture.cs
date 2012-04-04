namespace TecX.Unity.Configuration.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;
    using TecX.Unity.Configuration.Conventions;
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

            Assert.AreEqual("RetryTransactionBarHandler", cmd.HandledBy);
        }
    }

    public class CommandHandlerConvention : IRegistrationConvention
    {
        private Func<Type, string> getName;

        private readonly List<Type> wrapperTypes;

        public CommandHandlerConvention()
        {
            this.getName = t => t.Name;
            this.wrapperTypes = new List<Type>();
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "builder");

            if (type.IsInterface)
            {
                return;
            }

            if (type.IsGenericType)
            {
                return;
            }

            var interfaces = type.AllInterfaces();

            var handlerInterface = interfaces.FirstOrDefault(i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            if (handlerInterface != null)
            {
                var name = this.getName(type);

                if (this.wrapperTypes.Count > 0)
                {
                    var wrappers = this.wrapperTypes.ToArray().Reverse();

                    var closingType = handlerInterface.GetGenericArguments()[0];

                    foreach (Type wrapper in wrappers)
                    {
                        Type closedGeneric = wrapper.MakeGenericType(closingType);

                        builder.For(handlerInterface).Add(closedGeneric).Named(name);
                    }
                }

                builder.For(handlerInterface).Add(type).Named(name);
            }
        }

        public CommandHandlerConvention Named(Func<Type, string> getName)
        {
            Guard.AssertNotNull(getName, "getName");

            this.getName = getName;

            return this;
        }

        public CommandHandlerConvention WithTransaction()
        {
            this.wrapperTypes.Add(typeof(TransactionHandler<>));
            return this;
        }

        public CommandHandlerConvention WithDeadlockRetry()
        {
            this.wrapperTypes.Add(typeof(DeadlockRetryHandler<>));
            return this;
        }
    }

    public interface ICommandHandler<in TCommand> where TCommand : class
    {
        void Handle(TCommand command);
    }

    public class FooHandler : ICommandHandler<FooCommand>
    {
        public void Handle(FooCommand command)
        {
            command.HandledBy += @"-->FooHandler";
        }
    }

    public class BarHandler : ICommandHandler<BarCommand>
    {
        public void Handle(BarCommand command)
        {
            command.HandledBy += @"-->BarHandler";
        }
    }

    public class TransactionHandler<TCommand> : ICommandHandler<TCommand> where TCommand : BaseCommand
    {
        private readonly ICommandHandler<TCommand> inner;

        public TransactionHandler(ICommandHandler<TCommand> inner)
        {
            this.inner = inner;
        }

        public void Handle(TCommand command)
        {
            command.HandledBy += @"-->Transaction";
            this.inner.Handle(command);
        }
    }

    public class DeadlockRetryHandler<TCommand> : ICommandHandler<TCommand> where TCommand : BaseCommand
    {
        private readonly ICommandHandler<TCommand> inner;

        public DeadlockRetryHandler(ICommandHandler<TCommand> inner)
        {
            this.inner = inner;
        }

        public void Handle(TCommand command)
        {
            command.HandledBy += @"-->Retry";
            this.inner.Handle(command);
        }
    }

    public class BaseCommand
    {
        public BaseCommand()
        {
            this.HandledBy = string.Empty;
        }

        public string HandledBy { get; set; }
    }

    public class BarCommand : BaseCommand
    {
    }

    public class FooCommand : BaseCommand
    {
    }
}
