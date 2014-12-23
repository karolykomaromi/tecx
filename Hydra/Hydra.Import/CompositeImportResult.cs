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

        public CompositeImportResult(params ImportResult[] results)
        {
            this.results = new HashSet<ImportResult>(results ?? new ImportResult[0]);
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

        public override IEnumerable<Exception> Errors
        {
            get { return this.results.SelectMany(r => r.Errors); }
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
    }
}