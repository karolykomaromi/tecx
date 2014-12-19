namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Logging;
    using NHibernate;

    public class NhBulkImportWriter<T> : IImportWriter<T>
    {
        private readonly IStatelessSession session;

        public NhBulkImportWriter(IStatelessSession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public ImportResult Write(IEnumerable<T> items)
        {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<ImportResult>() != null);

            try
            {
                foreach (T item in items)
                {
                    this.session.Insert(item);
                }

                return new ImportSucceeded();
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new ImportFailed(ex);
            }
        }
    }
}