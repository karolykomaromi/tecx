namespace TecX.Search.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;

    using TecX.Common;
    using TecX.Search.Data.EF.Reader;
    using TecX.Search.Model;

    public class MessageEntities : DbContext, IMessageEntities
    {
        public MessageEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<MessageEntities>(null);
        }

        public IDbSet<Message> Messages { get; set; }

        public IEnumerable<Message> SearchByInterfaceAndTimeFrame(int maxResultCount, out int totalRowsCount, string interfaceName, DateTime after, DateTime before)
        {
            Guard.AssertNotEmpty(interfaceName, Constants.ParameterNames.InterfaceName);

            SqlParameter max = new SqlParameter(Constants.ParameterNames.MaxResultCount, maxResultCount);
            SqlParameter total = new SqlParameter(Constants.ParameterNames.TotalRowsCount, SqlDbType.Int)
            {
                Value = 0,
                Direction = ParameterDirection.Output
            };

            interfaceName = SqlServerHelper.AdjustWildcards(interfaceName);

            SqlParameter name = new SqlParameter(Constants.ParameterNames.InterfaceName, interfaceName);
            SqlParameter dt1 = new SqlParameter(Constants.ParameterNames.After, after);
            SqlParameter dt2 = new SqlParameter(Constants.ParameterNames.Before, before);

            IEnumerable<Message> messages = Database.SqlQuery<Message>(Constants.Sql.SearchBySourceAndTimeFrame, max, total, name, dt1, dt2);

            // output parameters are not evaluated until the underlying SqlCommand is completed.
            // thus we need to force the materialization of the query
            messages = messages.ToList();

            totalRowsCount = (int)total.Value;

            return messages;
        }

        public IEnumerable<Message> FindNonProcessedMessagesAndTagWithMarker()
        {
            IEnumerable<Message> unprocessedMessages = this.Database.SqlQuery<Message>(Constants.Sql.FindNonProcessedMessagesAndTagWithMarker);

            return unprocessedMessages;
        }

        public void AddSearchTerms(IEnumerable<SearchTerm> searchTerms)
        {
            Guard.AssertNotNull(searchTerms, "searchTerms");

            SqlConnection connection = (SqlConnection)this.Database.Connection;

            DbDataReader reader = (DbDataReader)searchTerms.AsDataReader();

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(Constants.Sql.SearchTermsBatch, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = Constants.CommandTimeout })
                {
                    SqlParameter parameter = cmd.Parameters.AddWithValue("searchTerms", reader);

                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.TypeName = Constants.SqlTypeNames.SearchTerm;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
