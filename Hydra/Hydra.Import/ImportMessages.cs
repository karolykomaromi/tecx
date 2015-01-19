namespace Hydra.Import
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class ImportMessages
    {
        private readonly HashSet<ImportMessage> messages;

        public ImportMessages(params ImportMessage[] messages)
        {
            this.messages = new HashSet<ImportMessage>(messages ?? new ImportMessage[0]);
        }

        public virtual IEnumerable<Error> Errors
        {
            get { return this.messages.OfType<Error>(); }
        }

        public virtual IEnumerable<Warning> Warnings
        {
            get { return this.messages.OfType<Warning>(); }
        }

        public virtual IEnumerable<Info> Infos
        {
            get { return this.messages.OfType<Info>(); }
        }

        public virtual int Count
        {
            get { return this.messages.Count; }
        }

        public virtual ImportMessages Add(ImportMessage message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<ImportMessages>() != null);

            if (message != ImportMessage.Empty)
            {
                this.messages.Add(message);
            }

            return this;
        }

        public virtual ImportMessages Add(IEnumerable<ImportMessage> messages)
        {
            Contract.Requires(messages != null);
            Contract.Ensures(Contract.Result<ImportMessages>() != null);

            foreach (ImportMessage message in messages)
            {
                this.Add(message);
            }

            return this;
        }
    }
}