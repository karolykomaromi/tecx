namespace TecX.Search.Data.EF
{
    public static class Constants
    {
        /// <summary>360 </summary>
        public const int CommandTimeout = 360;

        public static class ErrorMessages
        {
            /// <summary>
            /// Search terms must not contain more than 2 date values.
            /// </summary>
            public const string SearchTermsMustNotContainMoreThan2DateValues = "Search terms must not contain more than 2 date values.";
        }

        public static class ParameterNames
        {
            /// <summary>
            /// interfaceName
            /// </summary>
            public const string InterfaceName = "interfaceName";

            /// <summary>
            /// after
            /// </summary>
            public const string After = "after";

            /// <summary>
            /// before
            /// </summary>
            public const string Before = "before";

            /// <summary>
            /// maxResultCount
            /// </summary>
            public const string MaxResultCount = "maxResultCount";

            /// <summary>
            /// totalRowsCount
            /// </summary>
            public const string TotalRowsCount = "totalRowsCount";
        }

        public static class SqlTypeNames
        {
            /// <summary>
            /// SearchTerm
            /// </summary>
            public const string SearchTerm = "SearchTerm";
        }

        public static class Sql
        {
            /// <summary>
            /// SearchByTimeFrame @maxResultCount, @totalRowsCount OUT, @after, @before
            /// </summary>
            public const string SearchByTimeFrame =
                "SearchByTimeFrame @maxResultCount, @totalRowsCount OUT, @after, @before";

            /// <summary>
            /// SearchByInterfaceAndDate @maxResultCount, @totalRowsCount OUT, @interfaceName, @after
            /// </summary>
            public const string SearchByInterfaceAndDate =
                "SearchByInterfaceAndDate @maxResultCount, @totalRowsCount OUT, @interfaceName, @after";

            /// <summary>
            /// SearchByInterfaceAndTimeFrame @maxResultCount, @totalRowsCount OUT, @interfaceName, @after, @before
            /// </summary>
            public const string SearchByInterfaceAndTimeFrame = 
                "SearchByInterfaceAndTimeFrame @maxResultCount, @totalRowsCount OUT, @interfaceName, @after, @before";

            /// <summary>
            /// SearchByInterface @maxResultCount, @totalRowsCount OUT, @interfaceName
            /// </summary>
            public const string SearchByInterface =
                "SearchByInterface @maxResultCount, @totalRowsCount OUT, @interfaceName";

            /// <summary>
            /// FindNonProcessedMessagesAndTagWithMarker
            /// </summary>
            public const string FindNonProcessedMessagesAndTagWithMarker = "FindNonProcessedMessagesAndTagWithMarker";

            /// <summary>
            /// SearchTermsBatch
            /// </summary>
            public const string SearchTermsBatch = "SearchTermsBatch";
        }
    }
}