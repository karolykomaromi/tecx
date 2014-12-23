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

        public IEnumerable<Error> Errors
        {
            get { return this.messages.OfType<Error>(); }
        }

        public IEnumerable<Warning> Warnings
        {
            get { return this.messages.OfType<Warning>(); }
        }

        public IEnumerable<Info> Infos
        {
            get { return this.messages.OfType<Info>(); }
        }

        public ImportMessages Add(ImportMessage message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<ImportMessages>() != null);

            this.messages.Add(message);

            return this;
        }
    }
}