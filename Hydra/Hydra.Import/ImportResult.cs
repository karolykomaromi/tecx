namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public abstract class ImportResult
    {
        private readonly List<Exception> errors;

        protected ImportResult()
        {
            this.errors = new List<Exception>();
        }

        public abstract string Summary { get; }

        public virtual IEnumerable<Exception> Errors
        {
            get { return this.errors; }
        }

        public ImportResult AddError(Exception exception)
        {
            Contract.Requires(exception != null);

            this.errors.Add(exception);

            return this;
        }
    }
}