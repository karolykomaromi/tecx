namespace Hydra.Import.Results
{
    using Hydra.Import.Messages;

    public abstract class ImportResult
    {
        private readonly ImportMessages messages;

        protected ImportResult()
        {
            this.messages = new ImportMessages();
        }

        public abstract string Summary { get; }

        public virtual ImportMessages Messages
        {
            get { return this.messages; }
        }
    }
}