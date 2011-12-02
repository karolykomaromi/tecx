namespace TecX.Event.Test.TestObjects
{
    using TecX.Event;

    internal class SimplePublisher
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePublisher"/> class
        /// </summary>
        public SimplePublisher(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void Publish()
        {
            this.eventAggregator.Publish(new SimpleMessage());
        }
    }
}
