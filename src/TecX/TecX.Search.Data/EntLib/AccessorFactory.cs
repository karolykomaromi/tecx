namespace TecX.Search.Data.EntLib
{
    using System;

    using Microsoft.Practices.EnterpriseLibrary.Data;

    using TecX.Common;
    using TecX.Search;
    
    public class AccessorFactory
    {
        private readonly IRowMapper<Message> rowMapper;

        public AccessorFactory()
        {
            this.rowMapper = MapBuilder<Message>.MapAllProperties()
                .Build();
        }

        public virtual CustomSprocAccessor<Message> CreateAccessor(Database db, SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(db, "db");
            Guard.AssertNotNull(searchParameters, "searchParameters");

            if (SearchParameterHelper.IsInterfaceAndTimeFrameSearch(searchParameters))
            {
                SearchParameterHelper.ReorderForInterfaceAndTimeFrameSearch(searchParameters);

                return new CustomSprocAccessor<Message>(db, Constants.ProcedureNames.SearchByInterfaceAndTimeFrame, this.rowMapper);
            }

            if (SearchParameterHelper.IsInterfaceAndDateSearch(searchParameters))
            {
                SearchParameterHelper.ReorderForInterfaceAndDateSearch(searchParameters);

                return new CustomSprocAccessor<Message>(db, Constants.ProcedureNames.SearchByInterfaceAndDate, this.rowMapper);
            }

            if (SearchParameterHelper.IsTimeFrameSearch(searchParameters))
            {
                SearchParameterHelper.ReorderForTimeFrameSearch(searchParameters);

                return new CustomSprocAccessor<Message>(db, Constants.ProcedureNames.SearchByTimeFrame, this.rowMapper);
            }

            if (SearchParameterHelper.IsInterfaceSearch(searchParameters))
            {
                return new CustomSprocAccessor<Message>(db, Constants.ProcedureNames.SearchByInterface, this.rowMapper);
            }

            throw new NotSupportedException();
        }
    }
}