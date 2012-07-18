namespace TecX.Search
{
    using System;
    using System.Linq;

    using TecX.Common;

    public static class SearchParameterHelper
    {
        public static void ReorderForInterfaceAndTimeFrameSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            if (!IsInterfaceAndTimeFrameSearch(searchParameters))
            {
                throw new ArgumentException();
            }

            var s = searchParameters.OfType<SearchParameter<string>>().Single();
            var dt = searchParameters.OfType<SearchParameter<DateTime>>().OrderBy(p => p.Value);
            var dt1 = dt.First();
            var dt2 = dt.Last();

            searchParameters.Clear();

            searchParameters.Add(s);
            searchParameters.Add(dt1);
            searchParameters.Add(dt2);
        }

        public static bool IsInterfaceAndDateSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            return searchParameters.Count == 2 &&
                   searchParameters.OfType<SearchParameter<string>>().Count() == 1 &&
                   searchParameters.OfType<SearchParameter<DateTime>>().Count() == 1;
        }

        public static bool IsInterfaceAndTimeFrameSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            return searchParameters.Count == 3 &&
                   searchParameters.OfType<SearchParameter<string>>().Count() == 1 &&
                   searchParameters.OfType<SearchParameter<DateTime>>().Count() == 2;
        }

        public static void ReorderForInterfaceAndDateSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            if (!IsInterfaceAndDateSearch(searchParameters))
            {
                throw new ArgumentException();
            }

            var s = searchParameters.OfType<SearchParameter<string>>().Single();
            var dt = searchParameters.OfType<SearchParameter<DateTime>>().Single();

            searchParameters.Clear();

            searchParameters.Add(s);
            searchParameters.Add(dt);
        }

        public static bool IsTimeFrameSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            return searchParameters.Count == 2 &&
                searchParameters.OfType<SearchParameter<DateTime>>().Count() == 2;
        }

        public static void ReorderForTimeFrameSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            if (!IsTimeFrameSearch(searchParameters))
            {
                throw new ArgumentException();
            }

            var dt = searchParameters.OfType<SearchParameter<DateTime>>().OrderBy(p => p.Value);
            var dt1 = dt.First();
            var dt2 = dt.Last();

            searchParameters.Clear();

            searchParameters.Add(dt1);
            searchParameters.Add(dt2);
        }

        public static bool IsInterfaceSearch(SearchParameterCollection searchParameters)
        {
            Guard.AssertNotNull(searchParameters, "searchParameters");

            if (searchParameters.Count == 1 && searchParameters[0].Value is string)
            {
                return true;
            }

            return false;
        }
    }
}