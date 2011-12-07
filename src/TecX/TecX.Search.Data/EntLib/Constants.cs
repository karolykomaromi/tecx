namespace TecX.Search.Data.EntLib
{
    public static class Constants
    {
        /// <summary>
        /// Data Source=nemo02;Initial Catalog=MessageStore;Integrated Security=SSPI;
        /// </summary>
        public const string ConnectionString = "Data Source=.;Initial Catalog=MessageStore;Integrated Security=SSPI;";

        public static class ProcedureNames
        {
            /// <summary>
            /// SearchBySourceAndTimeFrame
            /// </summary>
            public const string SearchByInterfaceAndTimeFrame = "SearchBySourceAndTimeFrame";

            /// <summary>
            /// SearchByInterfaceAndDate
            /// </summary>
            public const string SearchByInterfaceAndDate = "SearchByInterfaceAndDate";

            /// <summary>
            /// SearchByTimeFrame
            /// </summary>
            public const string SearchByTimeFrame = "SearchByTimeFrame";

            /// <summary>
            /// SearchByInterface
            /// </summary>
            public const string SearchByInterface = "SearchByInterface";
        }
    }
}