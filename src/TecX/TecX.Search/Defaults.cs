namespace TecX.Search
{
    using System.Globalization;

    public static class Defaults
    {
        /// <summary>
        /// en-US
        /// </summary>
        public static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-US");

        /// <summary>
        /// 100
        /// </summary>
        public const int MaxResultCount = 100;
    }
}