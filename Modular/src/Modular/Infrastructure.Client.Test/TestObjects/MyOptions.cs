namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.Events;

    internal class MyOptions : Infrastructure.Options.Options
    {
        public MyOptions(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public string Foo { get; set; }
    }
}