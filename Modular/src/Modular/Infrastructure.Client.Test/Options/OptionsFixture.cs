namespace Infrastructure.Client.Test.Options
{
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.Events;
    using Infrastructure.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OptionsFixture
    {
        [TestMethod]
        public void Should_Write_Property_Value()
        {
            var ea = new Mock<IEventAggregator>();

            MyOptions options = new MyOptions { EventAggregator = ea.Object };

            options[Option.Create(options, o => o.Foo)] = "Bar";

            Assert.AreEqual("Bar", options.Foo);
        }

        [TestMethod]
        public void Should_Return_Property_Value()
        {
            var ea = new Mock<IEventAggregator>();

            MyOptions options = new MyOptions
            {
                EventAggregator = ea.Object,
                Foo = "Bar"
            };

            object actual = options[Option.Create(options, o => o.Foo)];

            Assert.AreEqual("Bar", actual);
        }

        [TestMethod]
        public void Should_Trigger_Message_On_SetProperty()
        {
            var ea = new Mock<IEventAggregator>();

            MyOptions options = new MyOptions { EventAggregator = ea.Object };

            Option option = Option.Create(options, o => o.Foo);
            options[option] = "Bar";

            ea.Verify(e => e.Publish(It.Is<OptionsChanged<MyOptions>>(msg => msg.OptionName == option && ReferenceEquals(options, msg.Options))));
        }

        [TestMethod]
        public void Should_Not_Know_About_Foreign_Options()
        {
            var ea = new Mock<IEventAggregator>();

            YourOptions yours = new YourOptions { EventAggregator = ea.Object };

            MyOptions mine = new MyOptions { EventAggregator = ea.Object };

            Option option = Option.Create(yours, y => y.Bar);

            Assert.IsFalse(mine.KnowsAbout(option));
        }

        [TestMethod]
        public void Should_Know_About_OWn_Options()
        {
            var ea = new Mock<IEventAggregator>();

            MyOptions mine = new MyOptions { EventAggregator = ea.Object };

            Option option = Option.Create(mine, m => m.Foo);

            Assert.IsTrue(mine.KnowsAbout(option));
        }
    }
}
