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

            MyOptions options = new MyOptions(ea.Object);

            options[new OptionName("Foo")] = "Bar";

            Assert.AreEqual("Bar", options.Foo);
        }

        [TestMethod]
        public void Should_Return_Property_Value()
        {
            var ea = new Mock<IEventAggregator>();

            MyOptions options = new MyOptions(ea.Object) { Foo = "Bar" };

            object actual = options[new OptionName("Foo")];

            Assert.AreEqual("Bar", actual);
        }

        [TestMethod]
        public void Should_Trigger_Message_On_SetProperty()
        {
            var ea = new Mock<IEventAggregator>();

            MyOptions options = new MyOptions(ea.Object);

            OptionName optionName = new OptionName("Foo");
            options[optionName] = "Bar";

            ea.Verify(e => e.Publish(It.Is<OptionsChanged>(msg => msg.OptionName == optionName && ReferenceEquals(options, msg.Options))));
        }
    }
}
