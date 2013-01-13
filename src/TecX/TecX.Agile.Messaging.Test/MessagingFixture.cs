namespace TecX.Agile.Messaging.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Messaging.Context;

    [TestClass]
    public class MessagingFixture
    {
        [TestMethod]
        public void PropertyChangedContext()
        {
            Guid id = Guid.NewGuid();

            var command = new ChangeProperty(id, "1", 1, 2);

            var @event = new PropertyChanged(id, "1", 1, 2);

            var ctx = new ChangePropertyContext(command);

            Assert.IsTrue(ctx.MatchesEvent(@event));
        }
    }
}