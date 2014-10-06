namespace TecX.Common.Time
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Calculates the day of the week for any given date (very handy when you can't use <see cref="DateTime.DayOfWeek"/> like in
    /// Entity Framework queries).
    /// See Wikipedia for further explanations: http://en.wikipedia.org/w/index.php?title=Zeller%27s_congruence&oldid=364258258
    /// </summary>
    public static class ZellersCongruence
    {
        /// <summary>
        /// Zellers congruence calculates the day of a  week (e.g. Wednesday) given a specific date
        /// </summary>
        /// <remarks>Why this when there is <see cref="DateTime.DayOfWeek"/>? Because EF does not support any of the advanced 
        /// <see cref="DateTime"/> features and this algorithm came in pretty handy when I needed to group data by the day 
        /// of the week. You can just copy and paste this snippet and make the appropriate adjustments</remarks>
        /// <param name="dt">Date for which to calculate the <see cref="DayOfWeek"/></param>
        /// <returns>Sunday, Monday,...</returns>
        public static DayOfWeek GetDayOfWeek(DateTime dt)
        {
            int q = dt.Day; // q is the day of the month
            int m = (dt.Month < 3) ? dt.Month + 12 : dt.Month;

            // m is the month (3 = March, 4 = April, 5 = May, ..., 14 = February)
            int y = (dt.Month < 3) ? dt.Year - 1 : dt.Year;

            // is the day of the week (0 = Saturday, 1 = Sunday, 2 = Monday, ...
            // int h = (q + ((m + 1) * 26) / 10 + y + y / 4 + 6 * (y / 100) + y / 400) % 7;
            int h = (q + (((m + 1) * 26) / 10) + y + (y / 4) + (6 * (y / 100)) + (y / 400)) % 7;

            // (int)DayOfWeek.Sunday == 0 so we need to add 1 to our result and make sure we stay inside the [0..6] range
            return (DayOfWeek)((h + 1) % 7);
        }

        /// <summary>
        /// Equivalent to <see cref="ZellersCongruence.GetDayOfWeek"/> but optimized for situations where you can't declare intermediate
        /// parameters (like a GroupBy clause in a Linq2Entities query). <seealso cref="ZellersCongruence.GetDayOfWeek"/>
        /// </summary>
        /// <param name="dt">Date for which to calculate the <see cref="DayOfWeek"/></param>
        /// <returns>Does not correspond to <see cref="DayOfWeek"/> but returns
        /// <list type="bullet">
        ///     <item>Saturday == 0</item>
        ///     <item>Sunday == 1</item>
        ///     <item>Monday == 2</item>
        ///     <item>Tuesday == 3</item>
        ///     <item>Wednesday == 4</item>
        ///     <item>Thursday == 5</item>
        ///     <item>Friday == 6</item>
        /// </list>
        /// </returns>
        /// 
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", 
            Justification = "Reviewed. Suppression is OK here.")]
        public static int GetDayOfWeekUsingYMethod(DateTime dt)
        {
            int h = (dt.Day
                     + ((((dt.Month < 3) ? dt.Month + 12 : dt.Month) + 1) * 26) / 10
                     + ((dt.Month < 3) ? dt.Year - 1 : dt.Year)
                     + ((dt.Month < 3) ? dt.Year - 1 : dt.Year) / 4
                     + 6 * (((dt.Month < 3) ? dt.Year - 1 : dt.Year) / 100)
                     + ((dt.Month < 3) ? dt.Year - 1 : dt.Year) / 400) % 7;

            return h;
        }
    }
}