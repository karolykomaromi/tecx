namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public abstract class ImportResult
    {
    }

    public class ImportFailed : ImportResult
    {
        private readonly List<Exception> errors;

        public ImportFailed(Exception exception)
        {
            Contract.Requires(exception != null);

            this.errors = new List<Exception> { exception };
        }

        public IEnumerable<Exception> Errors
        {
            get { return errors; }
        }

        public ImportFailed AddError(Exception exception)
        {
            Contract.Requires(exception != null);

            this.errors.Add(exception);
            
            return this;
        }
    }

    public class ImportSucceeded : ImportResult
    {
    }
}