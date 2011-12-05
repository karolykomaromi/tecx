namespace TecX.Search.Data.EntLib
{
    using System.Collections.Generic;

    using Microsoft.Practices.EnterpriseLibrary.Data;

    using TecX.Common;
    using TecX.Search;
    using TecX.Search.Data;

    public class EntLibMessageRepository : IMessageRepository
    {
        private readonly Database db;

        private readonly AccessorFactory accessorFactory;

        public EntLibMessageRepository(Database db, AccessorFactory accessorFactory)
        {
            Guard.AssertNotNull(db, "db");
            Guard.AssertNotNull(accessorFactory, "accessorFactory");

            this.db = db;
            this.accessorFactory = accessorFactory;
        }

        public void IndexMessagesForFullTextSearch()
        {
        }

        public void AddMessage(Message message)
        {
        }

        public IEnumerable<Message> Search(int maxResultCount, out int totalRowsCount, SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            var accessor = this.accessorFactory.CreateAccessor(this.db, searchParameters);

            var parameters = PrepareParameters(maxResultCount, searchParameters);

            var result = accessor.Execute(parameters);

            totalRowsCount = accessor.TotalRowCount;

            return result;
        }

        private static object[] PrepareParameters(int maxResultCount, SearchParameterCollection searchParameters)
        {
            var sp = searchParameters.GetParameterValues();

            object[] sp2 = new object[sp.Length + 2];

            sp.CopyTo(sp2, 2);

            sp2[0] = maxResultCount;
            sp2[1] = 0;

            return sp2;
        }
    }
}
