using TecX.Common.Event;

namespace TecX.Common.Test.TestObjects
{
    internal class SimplePublisher
    {
        private readonly IEventAggregator _eventAggregator;
        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePublisher"/> class
        /// </summary>
        public SimplePublisher(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
        }

        public void Publish()
        {
            _eventAggregator.Publish(new SimpleMessage());
        }
    }
}