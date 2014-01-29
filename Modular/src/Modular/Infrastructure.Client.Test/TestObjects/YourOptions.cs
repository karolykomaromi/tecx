namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.Events;
    using Infrastructure.Options;

    public class YourOptions : Options
    {
        public YourOptions(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public string Bar { get; set; }
    }
}