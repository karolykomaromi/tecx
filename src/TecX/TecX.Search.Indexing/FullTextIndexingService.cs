namespace TecX.Search.Indexing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

    public class FullTextIndexingService
    {
        private readonly Timer timer;

        public FullTextIndexingService(TimeSpan indexingIntervall)
        {
            this.timer = new Timer(indexingIntervall.TotalSeconds);
            this.timer.Elapsed += this.OnElapsed;
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
        }
    }
}
