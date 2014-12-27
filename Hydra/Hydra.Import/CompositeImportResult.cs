namespace Hydra.Import
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public sealed class CompositeImportResult : ImportResult, IEnumerable<ImportResult>
    {
        private readonly HashSet<ImportResult> results;

        private readonly CompositeImportMessages messages;

        public CompositeImportResult(params ImportResult[] results)
        {
            this.results = new HashSet<ImportResult>(results ?? new ImportResult[0]);

            this.messages = new CompositeImportMessages(this);
        }

        public override string Summary
        {
            get
            {
                if (this.results.All(r => r is ImportSucceeded))
                {
                    return Properties.Resources.ImportSuccessful;
                }

                if (this.results.All(r => r is ImportFailed))
                {
                    return Properties.Resources.ImportFailed;
                }

                return Properties.Resources.ImportPartiallyFailed;
            }
        }

        public override ImportMessages Messages
        {
            get { return this.messages; }
        }

        public CompositeImportResult Add(ImportResult result)
        {
            Contract.Requires(result != null);
            Contract.Ensures(Contract.Result<CompositeImportResult>() != null);

            this.results.Add(result);

            return this;
        }

        public IEnumerator<ImportResult> GetEnumerator()
        {
            return this.results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        private class CompositeImportMessages : ImportMessages
        {
            private readonly CompositeImportResult result;

            public CompositeImportMessages(CompositeImportResult result)
            {
                Contract.Requires(result != null);

                this.result = result;
            }

            public override int Count
            {
                get
                {
                    return this.result.results.Sum(r => r.Messages.Count);
                }
            }

            public override IEnumerable<Error> Errors
            {
                get { return this.result.results.SelectMany(r => r.Messages.Errors); }
            }

            public override IEnumerable<Warning> Warnings
            {
                get { return this.result.results.SelectMany(r => r.Messages.Warnings); }
            }

            public override IEnumerable<Info> Infos
            {
                get { return this.result.results.SelectMany(r => r.Messages.Infos); }
            }

            public override ImportMessages Add(ImportMessage message)
            {
                throw new InvalidOperationException();
            }
        }
    }
}