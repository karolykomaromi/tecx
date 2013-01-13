namespace TecX.Search.Parse
{
    using System;
    using System.Globalization;

    public class DateTimeParseStrategy : ParameterParseStrategy
    {
        protected override void ParseCore(ParameterParseContext context)
        {
            DateTime dt;
            if (DateTime.TryParse(context.StringToParse.Value, FormatProvider, DateTimeStyles.None, out dt))
            {
                context.Parameter = new SearchParameter<DateTime>(dt);
            }
        }
    }
}