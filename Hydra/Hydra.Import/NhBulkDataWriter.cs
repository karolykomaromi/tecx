namespace Hydra.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Logging;
    using NHibernate;

    public class NhBulkDataWriter<T> : IDataWriter<T>
    {
        private readonly IStatelessSession session;

        public NhBulkDataWriter(IStatelessSession session)
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

                ImportFailed failed = new ImportFailed();

                string msg = string.Format(Properties.Resources.ErrorDuringBulkInsert, typeof(T).AssemblyQualifiedName);

                failed.Messages.Add(new Error(msg));

                return failed;
            }
        }
    }
}