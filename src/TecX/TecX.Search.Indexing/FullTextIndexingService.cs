namespace TecX.Search.Indexing
{
    using System;
    using System.Timers;

    using TecX.Common;
    using TecX.Search.Data;

    public class FullTextIndexingService
    {
        private readonly IMessageRepository repository;

        private readonly Timer timer;

        public FullTextIndexingService(TimeSpan indexingIntervall, IMessageRepository repository)
        {
            Guard.AssertNotNull(repository, "repository");

            this.timer = new Timer(indexingIntervall.TotalSeconds);
            this.timer.Elapsed += this.OnElapsed;
            this.repository = repository;
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            this.repository.IndexMessagesForFullTextSearch();
        }
    }
}
